using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uycha.Models;

namespace Uycha.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Property> PropertiesForSale{ get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Сохранение PropertyType как строки
            modelBuilder.Entity<Property>()
                .Property(h => h.PropertyType)
                .HasConversion<string>();

            // Сохранение ListingType как строки
            modelBuilder.Entity<Property>()
                .Property(h => h.ListingType)
                .HasConversion<string>();
        }
    }
}
