using Microsoft.EntityFrameworkCore;
using Uycha.Data;
using Uycha.Models;
using Uycha.Repositories;

namespace Uycha.Tests
{
    public class PropertyForSaleRepositoryTests 
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        public PropertyForSaleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("HouseForSaleDb")
                .Options;
        }

        private ApplicationDbContext CreateDbContext()
        {
            return new ApplicationDbContext(_options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddHouse()
        {
            // db context
            var db = CreateDbContext();

            // house repository
            var repository = new PropertyRepository(db);

            // house posting
            var houseForSale = new Property
            {
                City = "Test City",
                Address = "Test Address",
                Description = "Test Description",
                Price = 100000,
                Area = 100,
                Floor = 5,
                UserId = "TestUserId"
            };

            // execute
            await repository.AddAsync(houseForSale);

            // result
            var result = db.PropertiesForSale.Find(houseForSale.Id);

            // assert
            Assert.NotNull(result);
            Assert.Equal("Test Address", result.Address);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnHouse()
        {
            var db = CreateDbContext();

            var repository = new PropertyRepository(db);

            var houseForSale = new Property
            {
                City = "Test City",
                Address = "Test Address",
                Description = "Test Description",
                Price = 100000,
                Area = 100,
                Floor = 5,
                UserId = "TestUserId"
            };

            await db.PropertiesForSale.AddAsync(houseForSale);
            await db.SaveChangesAsync();

            var result = await repository.GetByIdAsync(houseForSale.Id);

            Assert.NotNull(result);
            Assert.Equal("Test City", result.City);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException()
        {
            var db = CreateDbContext();
            var repository = new PropertyRepository(db);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetByIdAsync(999));
        }


        [Fact]
        public async Task GetTaskAsync_ShouldReturnAllHousesForSale()
        {
            var db = CreateDbContext();

            var repository = new PropertyRepository(db);

            var houseForSale1 = new Property
            {
                City = "Test City 1",
                Address = "Test Address 1",
                Description = "Test Description 1",
                Price = 100000,
                Area = 100,
                Floor = 5,
                UserId = "TestUserId 1"
            };


            var houseForSale2 = new Property
            {
                City = "Test City 2",
                Address = "Test Address 2",
                Description = "Test Description 2",
                Price = 200000,
                Area = 200,
                Floor = 10,
                UserId = "TestUserId 2"
            };

            await db.PropertiesForSale.AddRangeAsync(houseForSale1, houseForSale2);

            await db.SaveChangesAsync();

            var result = await repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateHouseForSale()
        {
            var db = CreateDbContext();
            var repository = new PropertyRepository(db);

            var houseForSale = new Property
            {
                City = "Test City 1",
                Address = "Test Address 1",
                Description = "Updated Description",
                Price = 100000,
                Area = 100,
                Floor = 5,
                UserId = "TestUserId 1"
            };

            await db.PropertiesForSale.AddAsync(houseForSale);
            await db.SaveChangesAsync();

            houseForSale.Description = "Updated Description";

            await repository.UpdateAsync(houseForSale);

            var result = db.PropertiesForSale.Find(houseForSale.Id);

            Assert.NotNull(result);
            Assert.Equal("Updated Description", result.Description);

        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteHouseForSale()
        {
            var db = CreateDbContext();
            var repository = new PropertyRepository(db);

            var houseForSale = new Property
            {
                City = "Test City 4",
                Address = "Test Address 4",
                Description = "Updated Description",
                Price = 100000,
                Area = 100,
                Floor = 5,
                UserId = "TestUserId 4"
            };

            await db.PropertiesForSale.AddAsync(houseForSale);
            await db.SaveChangesAsync();

            await repository.DeleteAsync(houseForSale.Id);

            var result = db.PropertiesForSale.Find(houseForSale.Id);

            Assert.Null(result);

        }
    }
}
