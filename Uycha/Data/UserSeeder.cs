using Microsoft.AspNetCore.Identity;
using Uycha.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Uycha.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await CreateUserWithRole(userManager, "admin@uycha.com", "Admin123!", Roles.Admin);
            await CreateUserWithRole(userManager, "user@uycha.com", "User123!", Roles.Customer);
            await CreateUserWithRole(userManager, "seller@uycha.com", "Seller123!", Roles.Seller);
        }
        private static async Task CreateUserWithRole(
            UserManager<IdentityUser> userManager,
            string email,
            string password,
            string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception($"Failed creating user with email " +
                        $"{user.Email}. Errors: {string.Join(",", result.Errors)}");
                }
            }
        }
    }
}
