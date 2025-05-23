using Microsoft.AspNetCore.Mvc;
using Uycha.Models;
using Uycha.Repositories;

namespace Uycha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesApiController : ControllerBase
    {
        private readonly IRepository<Property> _repository;

        public PropertiesApiController(IRepository<Property> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetAll()
        {
            var properties = await _repository.GetAllAsync();
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetById(int id)
        {
            var property = await _repository.GetByIdAsync(id);
            return property == null ? NotFound() : Ok(property);
        }

        // Фильтрация по различным параметрам   
        [HttpGet("filtered")]
        public async Task<ActionResult> GetFiltered(
    [FromQuery] ListingType? listingType,
    [FromQuery] PropertyType? propertyType,
    [FromQuery] string? city,
    [FromQuery] int? minPrice,
    [FromQuery] int? maxPrice,
    [FromQuery] int? minArea,
    [FromQuery] int? maxArea,
    [FromQuery] int? minRooms,
    [FromQuery] int? maxRooms,
    [FromQuery] RentDuration? rentDuration,
    [FromQuery] bool? isPremium,
    [FromQuery] bool? hasBalcony,
    [FromQuery] bool? hasElevator,
    [FromQuery] bool? hasFurniture,
    [FromQuery] bool? hasHeating,
    [FromQuery] bool? hasParking,
    [FromQuery] bool? isNegotiable,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var all = await _repository.GetAllAsync();

            // Текстовые и числовые фильтры
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

            // Фильтрация по опциям
            if (rentDuration.HasValue)
                all = all.Where(p => p.RentDuration == rentDuration.Value);

            if (isPremium.HasValue)
                all = all.Where(p => p.IsPremium == isPremium.Value);

            if (hasBalcony.HasValue)
                all = all.Where(p => p.HasBalcony == hasBalcony.Value);

            if (hasElevator.HasValue)
                all = all.Where(p => p.HasElevator == hasElevator.Value);

            if (hasFurniture.HasValue)
                all = all.Where(p => p.HasFurniture == hasFurniture.Value);

            if (hasHeating.HasValue)
                all = all.Where(p => p.HasHeating == hasHeating.Value);

            if (hasParking.HasValue)
                all = all.Where(p => p.HasParking == hasParking.Value);

            if (isNegotiable.HasValue)
                all = all.Where(p => p.IsNegotiable == isNegotiable.Value);

            // Пагинация
            var totalItems = all.Count();
            var items = all
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                totalItems,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                items
            });
        }



        [HttpPost]
        public async Task<ActionResult> Create(Property property)
        {
            await _repository.AddAsync(property);
            return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Property property)
        {
            if (id != property.Id) return BadRequest("ID mismatch");
            await _repository.UpdateAsync(property);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }

}
