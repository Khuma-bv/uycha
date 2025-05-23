using Microsoft.AspNetCore.Identity;
using Uycha.Constants;

namespace Uycha.Data
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            if (!await roleManager.RoleExistsAsync(Roles.Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Customer));
            }
            if (!await roleManager.RoleExistsAsync(Roles.Seller))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Seller));
            }

        }
    }
}
