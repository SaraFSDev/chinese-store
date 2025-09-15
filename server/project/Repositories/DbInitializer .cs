using Microsoft.AspNetCore.Identity; // הוספת זה עבור RoleManager, IdentityRole, UserManager
using Microsoft.Extensions.DependencyInjection; // הוספת זה עבור GetRequiredService
using System;
using System.Threading.Tasks;

namespace project.Repositories

{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (!await roleManager.RoleExistsAsync("manager"))
            {
                await roleManager.CreateAsync(new IdentityRole("manager"));
            }

            var user = await userManager.FindByNameAsync("manager1");
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "manager1",
                    Email = "manager1@example.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Password123!");
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "manager");
                }
            }

        }

    }
}