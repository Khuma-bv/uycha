using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uycha.Data;
using Uycha.Models;

namespace Uycha.Repositories
{
    // Concrete implementation of IRepository for Property entity
    public class PropertyRepository : IRepository<Property>
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new property to the database
        public async Task AddAsync(Property entity)
        {
            await _context.PropertiesForSale.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Delete a property by ID
        public async Task DeleteAsync(int id)
        {
            var house = await GetByIdAsync(id);
            if (house == null)
            {
                throw new DirectoryNotFoundException();
            }

            _context.PropertiesForSale.Remove(house);
            await _context.SaveChangesAsync();
        }

        // Get all properties
        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.PropertiesForSale.ToListAsync();
        }

        // Get a single property by ID
        public async Task<Property> GetByIdAsync(int id)
        {
            var house = await _context.PropertiesForSale.FindAsync(id);
            if (house == null)
            {
                throw new KeyNotFoundException();
            }
            return house;
        }

        // Get properties filtered by listing type (Sale or Rent)
        public async Task<IEnumerable<Property>> GetByListingTypeAsync(ListingType type)
        {
            return await _context.PropertiesForSale
                .Where(h => h.ListingType == type)
                .ToListAsync();
        }

        // Get properties filtered by property type (Apartment, Land, etc.)
        public async Task<IEnumerable<Property>> GetByPropertyTypeAsync(PropertyType type)
        {
            return await _context.PropertiesForSale
                .Where(h => h.PropertyType == type)
                .ToListAsync();
        }

        // Update an existing property
        public async Task UpdateAsync(Property entity)
        {
            _context.PropertiesForSale.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
