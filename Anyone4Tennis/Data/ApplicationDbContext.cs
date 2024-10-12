using Anyone4Tennis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Anyone4Tennis.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Member> Members { get; set; }

        public DbSet<Anyone4Tennis.Models.Schedules> Schedules { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure the base configurations for Identity are called

            // Configure the relationship between Schedules and Coach using Fluent API
            modelBuilder.Entity<Schedules>()
                .HasOne(s => s.Coach)
                .WithMany() // Assuming a one-to-many relationship, you can adjust if it's different
                .HasForeignKey(s => s.CoachId)
                .IsRequired(); // Require CoachId, not Coach navigation property

            // Additional Fluent API configurations can be placed here if needed
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
