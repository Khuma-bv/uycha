using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uycha.Constants;
using Uycha.Data;
using Uycha.Models;
using Uycha.Repositories;
using Uycha.ViewModels;

namespace Uycha.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IRepository<Property> _repository;
        private readonly UserManager<IdentityUser> _userManager;

        [ActivatorUtilitiesConstructor]
        public PropertiesController(IRepository<Property> repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(ListingType? listingType, PropertyType? propertyType)
        {
            // Retrieve all properties
            var properties = await _repository.GetAllAsync();

            // If user is Customer, filter properties by their UserId
            if (User.IsInRole(Roles.Customer))
            {
                var userId = _userManager.GetUserId(User);
                properties = properties.Where(p => p.UserId == userId);
            }

            // Apply ListingType filter if provided
            if (listingType.HasValue)
            {
                properties = properties.Where(p => p.ListingType == listingType.Value);
            }

            // Apply PropertyType filter if provided
            if (propertyType.HasValue)
            {
                properties = properties.Where(p => p.PropertyType == propertyType.Value);
            }

            return View("~/Views/Properties/Index.cshtml", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AllProperties(int page = 1)
        {
            int pageSize = 20; // Number of items per page
            var allProperties = await _repository.GetAllAsync(); // Get all properties

            // Apply pagination
            var properties = allProperties.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Pass current user ID to the view
            var userId = _userManager.GetUserId(User);
            ViewBag.UserId = userId;

            return View(properties);
        }

        [Authorize(Roles = "Admin,Seller")]
        public IActionResult Create()
        {
            return View();
        }

        // Handle property creation
        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Create(PropertyViewModel propertyVM)
        {
            if (propertyVM.ImageFiles == null || propertyVM.ImageFiles.Count < 3)
            {
                ModelState.AddModelError("ImageFiles", "Please upload at least 3 images.");
                return View(propertyVM);
            }

            var imagePaths = new List<string>();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            // Process each uploaded image
            foreach (var image in propertyVM.ImageFiles)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFiles", "Only JPG, JPEG, PNG, and GIF formats are allowed.");
                    return View(propertyVM);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
                var filePath = Path.Combine("wwwroot/uploads", uniqueFileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    imagePaths.Add("uploads/" + uniqueFileName);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred: " + ex.Message);
                    return View(propertyVM);
                }
            }

            // Create new Property instance
            var property = new Property
            {
                City = propertyVM.City,
                Address = propertyVM.Address,
                Latitude = propertyVM.Latitude,
                Longitude = propertyVM.Longitude,
                Description = propertyVM.Description,
                Price = propertyVM.Price,
                Area = propertyVM.Area,
                Rooms = propertyVM.Rooms,
                Floor = propertyVM.Floor,
                PhoneNumber = propertyVM.PhoneNumber,
                TelegramLink = propertyVM.TelegramLink,
                ImagePaths = string.Join(";", imagePaths),
                ListingType = propertyVM.ListingType,
                PropertyType = propertyVM.PropertyType,
                RentDuration = propertyVM.RentDuration,
                HasBalcony = propertyVM.HasBalcony,
                HasFurniture = propertyVM.HasFurniture,
                HasHeating = propertyVM.HasHeating,
                HasParking = propertyVM.HasParking,
                HasElevator = propertyVM.HasElevator,
                IsNegotiable = propertyVM.IsNegotiable,
                IsPremium = propertyVM.IsPremium,
                UserId = _userManager.GetUserId(User)
            };

            await _repository.AddAsync(property);
            return RedirectToAction("Index", "Properties");
        }

        [HttpGet]
        public async Task<IActionResult> LoadMore(int page = 1)
        {
            int pageSize = 8;
            var all = await _repository.GetAllAsync();
            var paged = all
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PropertyWithUserIdViewModel
            {
                Properties = paged,
                CurrentUserId = _userManager.GetUserId(User),
                IsAdmin = User.IsInRole("Admin")
            };

            return PartialView("_PropertyCardsPartial", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Update(int id)
        {
            var property = await _repository.GetByIdAsync(id);
            if (property == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole(Roles.Admin) && property.UserId != userId)
            {
                return Forbid();
            }

            // Map existing property to ViewModel
            var propertyVM = new PropertyViewModel
            {
                Id = property.Id,
                City = property.City,
                Address = property.Address,
                Description = property.Description,
                Price = property.Price,
                Area = property.Area,
                Rooms = property.Rooms,
                Floor = property.Floor,
                PhoneNumber = property.PhoneNumber,
                HasElevator = property.HasElevator,
                TelegramLink = property.TelegramLink,
                ListingType = property.ListingType,
                PropertyType = property.PropertyType,
                ImagePaths = property.ImagePaths
            };

            return View(propertyVM);
        }

        [HttpGet]
        public async Task<IActionResult> FilteredCards(
            ListingType? listingType,
            PropertyType? propertyType,
            string? city,
            int? minPrice,
            int? maxPrice,
            int? minArea,
            int? maxArea,
            int? minRooms,
            int? maxRooms)
        {
            var all = await _repository.GetAllAsync();

            if (listingType.HasValue)
                all = all.Where(p => p.ListingType == listingType.Value);

            if (propertyType.HasValue)
                all = all.Where(p => p.PropertyType == propertyType.Value);

            if (!string.IsNullOrWhiteSpace(city))
                all = all.Where(p => p.City.ToLower().Contains(city.Trim().ToLower()));

            if (minPrice.HasValue)
                all = all.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                all = all.Where(p => p.Price <= maxPrice.Value);

            if (minArea.HasValue)
                all = all.Where(p => p.Area >= minArea.Value);

            if (maxArea.HasValue)
                all = all.Where(p => p.Area <= maxArea.Value);

            if (minRooms.HasValue)
                all = all.Where(p => p.Rooms >= minRooms.Value);

            if (maxRooms.HasValue)
                all = all.Where(p => p.Rooms <= maxRooms.Value);

            var viewModel = new PropertyWithUserIdViewModel
            {
                Properties = all.ToList(),
                CurrentUserId = _userManager.GetUserId(User),
                IsAdmin = User.IsInRole("Admin")
            };

            return PartialView("_PropertyCardsPartial", viewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        [ActionName("Update")]
        public async Task<IActionResult> Update(int id, PropertyViewModel propertyVM)
        {
            var property = await _repository.GetByIdAsync(id);
            if (property == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole(Roles.Admin) && property.UserId != userId)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(propertyVM);
            }

            // Update property fields
            property.City = propertyVM.City;
            property.Address = propertyVM.Address;
            property.Description = propertyVM.Description;
            property.Price = propertyVM.Price;
            property.Area = propertyVM.Area;
            property.Rooms = propertyVM.Rooms;
            property.Floor = propertyVM.Floor;
            property.PhoneNumber = propertyVM.PhoneNumber;
            property.HasElevator = propertyVM.HasElevator;
            property.TelegramLink = propertyVM.TelegramLink;
            property.ListingType = propertyVM.ListingType;
            property.PropertyType = propertyVM.PropertyType;

            // If new images uploaded, replace old ones
            if (propertyVM.ImageFiles != null && propertyVM.ImageFiles.Count > 0)
            {
                var oldImages = property.ImagePaths?.Split(';') ?? new string[0];
                foreach (var img in oldImages)
                {
                    var oldImagePath = Path.Combine("wwwroot", img);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var newImagePaths = new List<string>();
                foreach (var image in propertyVM.ImageFiles)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
                    var filePath = Path.Combine("wwwroot/uploads", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    newImagePaths.Add("uploads/" + uniqueFileName);
                }

                property.ImagePaths = string.Join(";", newImagePaths);
            }

            await _repository.UpdateAsync(property);
            return RedirectToAction("Index", "Properties");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _repository.GetByIdAsync(id);
            if (property == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole(Roles.Admin) && property.UserId != userId)
            {
                return Forbid();
            }

            // Delete property images from server
            if (!string.IsNullOrEmpty(property.ImagePaths))
            {
                var images = property.ImagePaths.Split(';');
                foreach (var img in images)
                {
                    var imagePath = Path.Combine("wwwroot", img);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }

            await _repository.DeleteAsync(id);
            return RedirectToAction("AllProperties", "Properties");
        }
    }
}
