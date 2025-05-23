using Uycha.Models;

namespace Uycha.Repositories
{
    // Generic repository interface to abstract data operations
    public interface IRepository<T> where T : class
    {
        // Get all entities
        Task<IEnumerable<T>> GetAllAsync();

        // Get a single entity by its ID
        Task<T> GetByIdAsync(int id);

        // Add a new entity
        Task AddAsync(T entity);

        // Update an existing entity
        Task UpdateAsync(T entity);

        // Delete an entity by ID
        Task DeleteAsync(int id);

        // Filter entities by listing type (specific to Property model)
        Task<IEnumerable<T>> GetByListingTypeAsync(ListingType type);

        // Filter entities by property type (specific to Property model)
        Task<IEnumerable<T>> GetByPropertyTypeAsync(PropertyType type);
    }
}
