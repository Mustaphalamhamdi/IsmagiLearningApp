using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IsmagiLearningApp.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Get the required services
            var userManager = service.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            // First check if roles exist and create them if they don't
            if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }
            if (!await roleManager.RoleExistsAsync(Roles.User.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }

            // Create admin user if it doesn't exist
            var adminEmail = "admin@ismagilearning.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
                }
            }
        }
    }

    public enum Roles
    {
        Admin,
        User
    }
}