using System.ComponentModel.DataAnnotations;
using Uycha.Models;

namespace Uycha.ViewModels
{
    // ViewModel used for creating and updating property listings via forms
    public class PropertyViewModel
    {
        [Required]
        public int Id { get; set; } // ID (used in edit/update)

        [Required]
        public string City { get; set; } // City name

        [Required]
        public string Address { get; set; } // Full address

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


        [Required]
        public string Description { get; set; } // Description of the property

        [Required]
        public int Price { get; set; } // Price of the property

        [Required]
        [Range(1, 1000, ErrorMessage = "Area value is invalid!")]
        public int Area { get; set; } // Area in square meters

        [Range(1, 10, ErrorMessage = "Room count is invalid!")]
        public int Rooms { get; set; } // Number of rooms

        public int Floor { get; set; } // Floor number

        [Required]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Invalid phone number!")]
        public string PhoneNumber { get; set; } // Contact number (9 digits)

        [Required]
        public string TelegramLink { get; set; } // Telegram username (e.g., @username)

        public string ImagePaths { get; set; } // Semicolon-separated uploaded image paths

        [Required(ErrorMessage = "At least one image must be uploaded!")]
        public List<IFormFile> ImageFiles { get; set; } // List of uploaded image files (used in form)

        [Required]
        public ListingType ListingType { get; set; } // Sale or Rent

        [Required]
        public PropertyType PropertyType { get; set; } // House, Apartment, etc.

        public RentDuration? RentDuration { get; set; } // Only for rental listings

        // Additional features
        [Display(Name = "Balcony")]
        public bool HasBalcony { get; set; }

        [Display(Name = "Elevator")]
        public bool HasElevator { get; set; }

        [Display(Name = "Furnished")]
        public bool HasFurniture { get; set; }

        [Display(Name = "Heating")]
        public bool HasHeating { get; set; }

        [Display(Name = "Parking")]
        public bool HasParking { get; set; }

        [Display(Name = "Negotiable Price")]
        public bool IsNegotiable { get; set; }

        [Display(Name = "Premium")]
        public bool IsPremium { get; set; }
    }
}
