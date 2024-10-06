using Anyone4Tennis.Models; // Ensure this namespace includes your Coach and Member models
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Anyone4Tennis.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            // Ensure the database is created
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();

            // Add roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRoleExists(roleManager, "Admin");
            await EnsureRoleExists(roleManager, "Coach");
            await EnsureRoleExists(roleManager, "Member");

            // Add default users
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await EnsureAdminExists(userManager, "admin@anyone4tennis.com", "Admin@123", "Admin", "Admin", "User");
            await EnsureCoachExists(userManager, "coach1@anyone4tennis.com", "Password123!", "Coach", "John", "Doe");
            await EnsureMemberExists(userManager, "member1@anyone4tennis.com", "Password123!", "Member", "Jane", "Smith");
        }

        private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            // Check if the role exists and create it if it doesn't
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task EnsureAdminExists(UserManager<ApplicationUser> userManager, string email, string password, string roleName, string firstName, string lastName)
        {
            // Check if the admin user already exists
            if (await userManager.FindByEmailAsync(email) == null)
            {
                // Create the admin user with FirstName and LastName
                var admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName
                };

                // Create the admin user in the database
                var result = await userManager.CreateAsync(admin, password);

                // If successful, add the user to the Admin role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, roleName);
                }
                else
                {
                    // Handle errors (optional)
                    Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors)}");
                }
            }
        }

        private static async Task EnsureCoachExists(UserManager<ApplicationUser> userManager, string email, string password, string roleName, string firstName, string lastName)
        {
            // Check if the coach user already exists
            if (await userManager.FindByEmailAsync(email) == null)
            {
                // Create the coach user with FirstName and LastName
                var coach = new Coach
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    Photo = await GetPhotoAsByteArray("wwwroot/images/coach1.jpg") // Load the photo as a byte array
                };

                // Create the user in the database
                var result = await userManager.CreateAsync(coach, password);

                // If successful, add the user to the Coach role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(coach, roleName);
                }
                else
                {
                    // Handle errors (optional)
                    Console.WriteLine($"Error creating coach user: {string.Join(", ", result.Errors)}");
                }
            }
        }

        private static async Task EnsureMemberExists(UserManager<ApplicationUser> userManager, string email, string password, string roleName, string firstName, string lastName)
        {
            // Check if the member user already exists
            if (await userManager.FindByEmailAsync(email) == null)
            {
                // Create the member user with FirstName and LastName
                var member = new Member
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    Active = true
                };

                // Create the user in the database
                var result = await userManager.CreateAsync(member, password);

                // If successful, add the user to the Member role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(member, roleName);
                }
                else
                {
                    // Handle errors (optional)
                    Console.WriteLine($"Error creating member user: {string.Join(", ", result.Errors)}");
                }
            }
        }

        // Helper method to read the image file and convert it to a byte array
        private static async Task<byte[]> GetPhotoAsByteArray(string filePath)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
