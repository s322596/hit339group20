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

        public DbSet<MemberSchedule> MemberSchedules { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public async Task<int> GetNextMemberIdAsync()
        {
            var maxId = await Users.OfType<Member>().MaxAsync(u => (int?)u.MemberId);
            return (maxId ?? 0) + 1; // Start from 1 if no members exist
        }

        public async Task<int> GetNextCoachIdAsync()
        {
            var maxId = await Users.OfType<Coach>().MaxAsync(u => (int?)u.CoachId);
            return (maxId ?? 0) + 1; // Start from 1 if no coaches exist
        }
    }
}
