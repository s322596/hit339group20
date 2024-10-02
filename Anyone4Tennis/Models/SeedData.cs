using Anyone4Tennis.Models; // Ensure this namespace includes your Coach and Member models
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            await EnsureRoleExists(roleManager, "Coach");
            await EnsureRoleExists(roleManager, "Member");

            // Add default users
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
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
                    LastName = lastName
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
                    LastName = lastName
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
    }
}
