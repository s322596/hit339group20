using Anyone4Tennis.Models;
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
        public static async Task Initialize(IServiceProvider serviceProvider, IWebHostEnvironment env)
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
            await EnsureMemberExists(userManager, "member1@anyone4tennis.com", "Password123!", "Member", "Jane", "Smith", true);


            // Add coaches
            await EnsureCoachExists(userManager, "jane.johnson@anyone4tennis.com", "Password123!", "Coach", "Jane", "Johnson", "/images/coaches/alex.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "emily.smith@anyone4tennis.com", "Password123!", "Coach", "Emily", "Smith", "/images/coaches/bowser.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "emily.wilson@anyone4tennis.com", "Password123!", "Coach", "Emily", "Wilson", "/images/coaches/DK.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "emily.williams@anyone4tennis.com", "Password123!", "Coach", "Emily", "Williams", "/images/coaches/falco.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "michael.smith@anyone4tennis.com", "Password123!", "Coach", "Michael", "Smith", "/images/coaches/falcon.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "laura.miller@anyone4tennis.com", "Password123!", "Coach", "Laura", "Miller", "/images/coaches/fox.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "emily.wilson@anyone4tennis.com", "Password123!", "Coach", "Emily", "Wilson", "/images/coaches/Jigglypuff.png", env.WebRootPath);
            await EnsureCoachExists(userManager, "daniel.johnson@anyone4tennis.com", "Password123!", "Coach", "Daniel", "Johnson", "/images/coaches/kirby.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "daniel.williams@anyone4tennis.com", "Password123!", "Coach", "Daniel", "Williams", "/images/coaches/link.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "jane.davis@anyone4tennis.com", "Password123!", "Coach", "Jane", "Davis", "/images/coaches/lucina.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "robert.jones@anyone4tennis.com", "Password123!", "Coach", "Robert", "Jones", "/images/coaches/luigi.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "david.miller@anyone4tennis.com", "Password123!", "Coach", "David", "Miller", "/images/coaches/mario.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "jessica.williams@anyone4tennis.com", "Password123!", "Coach", "Jessica", "Williams", "/images/coaches/mew2.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "jane.johnson@anyone4tennis.com", "Password123!", "Coach", "Jane", "Johnson", "/images/coaches/peach.png", env.WebRootPath);
            await EnsureCoachExists(userManager, "laura.wilson@anyone4tennis.com", "Password123!", "Coach", "Laura", "Wilson", "/images/coaches/pikachu.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "daniel.williams@anyone4tennis.com", "Password123!", "Coach", "Daniel", "Williams", "/images/coaches/pichu.jpg", env.WebRootPath);
            await EnsureCoachExists(userManager, "sarah.miller@anyone4tennis.com", "Password123!", "Coach", "Sarah", "Miller", "/images/coaches/charmander.png", env.WebRootPath);
            await EnsureCoachExists(userManager, "sarah.johnson@anyone4tennis.com", "Password123!", "Coach", "Sarah", "Johnson", "/images/coaches/steve.png", env.WebRootPath);
            await EnsureCoachExists(userManager, "jessica.williams@anyone4tennis.com", "Password123!", "Coach", "Jessica", "Williams", "/images/coaches/yoshi.png", env.WebRootPath);
            await EnsureCoachExists(userManager, "sarah.williams@anyone4tennis.com", "Password123!", "Coach", "Sarah", "Williams", "/images/coaches/zelda.jpg", env.WebRootPath);


            // Add first 49 members
            await EnsureMemberExists(userManager, "david.miller@anyone4tennis.com", "Password123!", "Member", "David", "Miller", true);
            await EnsureMemberExists(userManager, "john.williams@anyone4tennis.com", "Password123!", "Member", "John", "Williams", true);
            await EnsureMemberExists(userManager, "david.davis@anyone4tennis.com", "Password123!", "Member", "David", "Davis", false);
            await EnsureMemberExists(userManager, "jessica.jones@anyone4tennis.com", "Password123!", "Member", "Jessica", "Jones", true);
            await EnsureMemberExists(userManager, "jessica.brown@anyone4tennis.com", "Password123!", "Member", "Jessica", "Brown", false);
            await EnsureMemberExists(userManager, "daniel.brown@anyone4tennis.com", "Password123!", "Member", "Daniel", "Brown", false);
            await EnsureMemberExists(userManager, "daniel.johnson@anyone4tennis.com", "Password123!", "Member", "Daniel", "Johnson", false);
            await EnsureMemberExists(userManager, "jessica.jones@anyone4tennis.com", "Password123!", "Member", "Jessica", "Jones", true);
            await EnsureMemberExists(userManager, "robert.taylor@anyone4tennis.com", "Password123!", "Member", "Robert", "Taylor", true);
            await EnsureMemberExists(userManager, "john.williams@anyone4tennis.com", "Password123!", "Member", "John", "Williams", false);
            await EnsureMemberExists(userManager, "daniel.smith@anyone4tennis.com", "Password123!", "Member", "Daniel", "Smith", false);
            await EnsureMemberExists(userManager, "jessica.jones@anyone4tennis.com", "Password123!", "Member", "Jessica", "Jones", false);
            await EnsureMemberExists(userManager, "robert.smith@anyone4tennis.com", "Password123!", "Member", "Robert", "Smith", false);
            await EnsureMemberExists(userManager, "emily.wilson@anyone4tennis.com", "Password123!", "Member", "Emily", "Wilson", true);
            await EnsureMemberExists(userManager, "robert.davis@anyone4tennis.com", "Password123!", "Member", "Robert", "Davis", false);
            await EnsureMemberExists(userManager, "john.taylor@anyone4tennis.com", "Password123!", "Member", "John", "Taylor", false);
            await EnsureMemberExists(userManager, "jane.davis@anyone4tennis.com", "Password123!", "Member", "Jane", "Davis", false);
            await EnsureMemberExists(userManager, "laura.smith@anyone4tennis.com", "Password123!", "Member", "Laura", "Smith", true);
            await EnsureMemberExists(userManager, "emily.smith@anyone4tennis.com", "Password123!", "Member", "Emily", "Smith", true);
            await EnsureMemberExists(userManager, "michael.brown@anyone4tennis.com", "Password123!", "Member", "Michael", "Brown", true);
            await EnsureMemberExists(userManager, "laura.davis@anyone4tennis.com", "Password123!", "Member", "Laura", "Davis", true);
            await EnsureMemberExists(userManager, "john.johnson@anyone4tennis.com", "Password123!", "Member", "John", "Johnson", true);
            await EnsureMemberExists(userManager, "john.moore@anyone4tennis.com", "Password123!", "Member", "John", "Moore", false);
            await EnsureMemberExists(userManager, "sarah.taylor@anyone4tennis.com", "Password123!", "Member", "Sarah", "Taylor", true);
            await EnsureMemberExists(userManager, "robert.wilson@anyone4tennis.com", "Password123!", "Member", "Robert", "Wilson", true);
            await EnsureMemberExists(userManager, "jane.jones@anyone4tennis.com", "Password123!", "Member", "Jane", "Jones", false);
            await EnsureMemberExists(userManager, "jessica.taylor@anyone4tennis.com", "Password123!", "Member", "Jessica", "Taylor", false);
            await EnsureMemberExists(userManager, "jessica.williams@anyone4tennis.com", "Password123!", "Member", "Jessica", "Williams", false);
            await EnsureMemberExists(userManager, "john.wilson@anyone4tennis.com", "Password123!", "Member", "John", "Wilson", false);
            await EnsureMemberExists(userManager, "david.miller@anyone4tennis.com", "Password123!", "Member", "David", "Miller", true);
            await EnsureMemberExists(userManager, "jane.moore@anyone4tennis.com", "Password123!", "Member", "Jane", "Moore", false);
            await EnsureMemberExists(userManager, "daniel.smith@anyone4tennis.com", "Password123!", "Member", "Daniel", "Smith", true);
            await EnsureMemberExists(userManager, "john.brown@anyone4tennis.com", "Password123!", "Member", "John", "Brown", false);
            await EnsureMemberExists(userManager, "john.taylor@anyone4tennis.com", "Password123!", "Member", "John", "Taylor", true);
            await EnsureMemberExists(userManager, "emily.davis@anyone4tennis.com", "Password123!", "Member", "Emily", "Davis", false);
            await EnsureMemberExists(userManager, "laura.brown@anyone4tennis.com", "Password123!", "Member", "Laura", "Brown", false);
            await EnsureMemberExists(userManager, "emily.smith@anyone4tennis.com", "Password123!", "Member", "Emily", "Smith", false);
            await EnsureMemberExists(userManager, "jessica.davis@anyone4tennis.com", "Password123!", "Member", "Jessica", "Davis", false);
            await EnsureMemberExists(userManager, "emily.brown@anyone4tennis.com", "Password123!", "Member", "Emily", "Brown", false);
            await EnsureMemberExists(userManager, "john.moore@anyone4tennis.com", "Password123!", "Member", "John", "Moore", false);
            await EnsureMemberExists(userManager, "jane.moore@anyone4tennis.com", "Password123!", "Member", "Jane", "Moore", false);
            await EnsureMemberExists(userManager, "jessica.johnson@anyone4tennis.com", "Password123!", "Member", "Jessica", "Johnson", false);
            await EnsureMemberExists(userManager, "jessica.davis@anyone4tennis.com", "Password123!", "Member", "Jessica", "Davis", false);
            await EnsureMemberExists(userManager, "michael.jones@anyone4tennis.com", "Password123!", "Member", "Michael", "Jones", true);
            await EnsureMemberExists(userManager, "sarah.davis@anyone4tennis.com", "Password123!", "Member", "Sarah", "Davis", true);
            await EnsureMemberExists(userManager, "john.moore@anyone4tennis.com", "Password123!", "Member", "John", "Moore", true);
            await EnsureMemberExists(userManager, "michael.miller@anyone4tennis.com", "Password123!", "Member", "Michael", "Miller", false);
            await EnsureMemberExists(userManager, "jane.wilson@anyone4tennis.com", "Password123!", "Member", "Jane", "Wilson", true);
            await EnsureMemberExists(userManager, "jessica.moore@anyone4tennis.com", "Password123!", "Member", "Jessica", "Moore", false);


            // Add last 51 members
            await EnsureMemberExists(userManager, "jessica.smith@anyone4tennis.com", "Password123!", "Member", "Jessica", "Smith", true);
            await EnsureMemberExists(userManager, "emily.miller@anyone4tennis.com", "Password123!", "Member", "Emily", "Miller", true);
            await EnsureMemberExists(userManager, "daniel.jones@anyone4tennis.com", "Password123!", "Member", "Daniel", "Jones", false);
            await EnsureMemberExists(userManager, "david.moore@anyone4tennis.com", "Password123!", "Member", "David", "Moore", false);
            await EnsureMemberExists(userManager, "sarah.jones@anyone4tennis.com", "Password123!", "Member", "Sarah", "Jones", false);
            await EnsureMemberExists(userManager, "jessica.wilson@anyone4tennis.com", "Password123!", "Member", "Jessica", "Wilson", true);
            await EnsureMemberExists(userManager, "david.smith@anyone4tennis.com", "Password123!", "Member", "David", "Smith", true);
            await EnsureMemberExists(userManager, "jessica.wilson@anyone4tennis.com", "Password123!", "Member", "Jessica", "Wilson", true);
            await EnsureMemberExists(userManager, "david.smith@anyone4tennis.com", "Password123!", "Member", "David", "Smith", true);
            await EnsureMemberExists(userManager, "michael.jones@anyone4tennis.com", "Password123!", "Member", "Michael", "Jones", true);
            await EnsureMemberExists(userManager, "daniel.smith@anyone4tennis.com", "Password123!", "Member", "Daniel", "Smith", true);
            await EnsureMemberExists(userManager, "jessica.davis@anyone4tennis.com", "Password123!", "Member", "Jessica", "Davis", false);
            await EnsureMemberExists(userManager, "emily.smith@anyone4tennis.com", "Password123!", "Member", "Emily", "Smith", true);
            await EnsureMemberExists(userManager, "david.smith@anyone4tennis.com", "Password123!", "Member", "David", "Smith", true);
            await EnsureMemberExists(userManager, "robert.williams@anyone4tennis.com", "Password123!", "Member", "Robert", "Williams", false);
            await EnsureMemberExists(userManager, "emily.jones@anyone4tennis.com", "Password123!", "Member", "Emily", "Jones", false);
            await EnsureMemberExists(userManager, "laura.moore@anyone4tennis.com", "Password123!", "Member", "Laura", "Moore", false);
            await EnsureMemberExists(userManager, "sarah.johnson@anyone4tennis.com", "Password123!", "Member", "Sarah", "Johnson", false);
            await EnsureMemberExists(userManager, "laura.moore@anyone4tennis.com", "Password123!", "Member", "Laura", "Moore", true);
            await EnsureMemberExists(userManager, "michael.miller@anyone4tennis.com", "Password123!", "Member", "Michael", "Miller", true);
            await EnsureMemberExists(userManager, "sarah.brown@anyone4tennis.com", "Password123!", "Member", "Sarah", "Brown", true);
            await EnsureMemberExists(userManager, "robert.moore@anyone4tennis.com", "Password123!", "Member", "Robert", "Moore", true);
            await EnsureMemberExists(userManager, "laura.taylor@anyone4tennis.com", "Password123!", "Member", "Laura", "Taylor", true);
            await EnsureMemberExists(userManager, "david.williams@anyone4tennis.com", "Password123!", "Member", "David", "Williams", false);
            await EnsureMemberExists(userManager, "emily.smith@anyone4tennis.com", "Password123!", "Member", "Emily", "Smith", true);
            await EnsureMemberExists(userManager, "jane.smith@anyone4tennis.com", "Password123!", "Member", "Jane", "Smith", true);
            await EnsureMemberExists(userManager, "jane.taylor@anyone4tennis.com", "Password123!", "Member", "Jane", "Taylor", false);
            await EnsureMemberExists(userManager, "emily.williams@anyone4tennis.com", "Password123!", "Member", "Emily", "Williams", false);
            await EnsureMemberExists(userManager, "daniel.wilson@anyone4tennis.com", "Password123!", "Member", "Daniel", "Wilson", true);
            await EnsureMemberExists(userManager, "michael.williams@anyone4tennis.com", "Password123!", "Member", "Michael", "Williams", true);
            await EnsureMemberExists(userManager, "david.brown@anyone4tennis.com", "Password123!", "Member", "David", "Brown", false);
            await EnsureMemberExists(userManager, "robert.davis@anyone4tennis.com", "Password123!", "Member", "Robert", "Davis", true);
            await EnsureMemberExists(userManager, "jane.williams@anyone4tennis.com", "Password123!", "Member", "Jane", "Williams", false);
            await EnsureMemberExists(userManager, "sarah.williams@anyone4tennis.com", "Password123!", "Member", "Sarah", "Williams", true);
            await EnsureMemberExists(userManager, "jane.williams@anyone4tennis.com", "Password123!", "Member", "Jane", "Williams", true);
            await EnsureMemberExists(userManager, "laura.wilson@anyone4tennis.com", "Password123!", "Member", "Laura", "Wilson", false);
            await EnsureMemberExists(userManager, "jessica.wilson@anyone4tennis.com", "Password123!", "Member", "Jessica", "Wilson", true);
            await EnsureMemberExists(userManager, "laura.brown@anyone4tennis.com", "Password123!", "Member", "Laura", "Brown", true);
            await EnsureMemberExists(userManager, "michael.johnson@anyone4tennis.com", "Password123!", "Member", "Michael", "Johnson", true);
            await EnsureMemberExists(userManager, "sarah.moore@anyone4tennis.com", "Password123!", "Member", "Sarah", "Moore", true);
            await EnsureMemberExists(userManager, "sarah.wilson@anyone4tennis.com", "Password123!", "Member", "Sarah", "Wilson", false);
            await EnsureMemberExists(userManager, "jessica.wilson@anyone4tennis.com", "Password123!", "Member", "Jessica", "Wilson", false);
            await EnsureMemberExists(userManager, "daniel.brown@anyone4tennis.com", "Password123!", "Member", "Daniel", "Brown", false);
            await EnsureMemberExists(userManager, "daniel.miller@anyone4tennis.com", "Password123!", "Member", "Daniel", "Miller", true);
            await EnsureMemberExists(userManager, "laura.smith@anyone4tennis.com", "Password123!", "Member", "Laura", "Smith", true);
            await EnsureMemberExists(userManager, "laura.miller@anyone4tennis.com", "Password123!", "Member", "Laura", "Miller", false);
            await EnsureMemberExists(userManager, "daniel.johnson@anyone4tennis.com", "Password123!", "Member", "Daniel", "Johnson", true);
            await EnsureMemberExists(userManager, "jessica.moore@anyone4tennis.com", "Password123!", "Member", "Jessica", "Moore", false);
            await EnsureMemberExists(userManager, "robert.wilson@anyone4tennis.com", "Password123!", "Member", "Robert", "Wilson", false);
            await EnsureMemberExists(userManager, "laura.smith@anyone4tennis.com", "Password123!", "Member", "Laura", "Smith", false);
            await EnsureMemberExists(userManager, "jessica.wilson@anyone4tennis.com", "Password123!", "Member", "Jessica", "Wilson", false);

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

        private static async Task EnsureCoachExists(UserManager<ApplicationUser> userManager, string email, string password, string roleName, string firstName, string lastName, string photoPath, string webRootPath)
        {
            // Check if the coach user already exists
            if (await userManager.FindByEmailAsync(email) == null)
            {
                // Create the coach user with FirstName, LastName, and Photo
                var coach = new Coach
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    Photo = await GetPhotoAsByteArray(photoPath, webRootPath) // Load the photo as a byte array
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


        private static async Task EnsureMemberExists(UserManager<ApplicationUser> userManager, string email, string password, string roleName, string firstName, string lastName, bool v)
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
                    Active = v,
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
        private static async Task<byte[]> GetPhotoAsByteArray(string photoPath, string webRootPath)
        {
            // Construct the full path to the photo
            var fullPath = Path.Combine(webRootPath, photoPath.TrimStart('/')); // Ensure no leading slash

            // Check if the file exists
            if (File.Exists(fullPath))
            {
                // Read the file and convert it to a byte array
                return await File.ReadAllBytesAsync(fullPath);
            }
            else
            {
                // Handle the case where the photo file doesn't exist (optional)
                Console.WriteLine($"Photo not found: {fullPath}");
                return null; // Or handle as per your requirements
            }
        }

    }
}
