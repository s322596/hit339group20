using Anyone4Tennis.Data;
using Anyone4Tennis.Models;
using Anyone4Tennis.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anyone4Tennis.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Coaches()
        {
            var coaches = await _context.Users
                .OfType<Coach>()
                .ToListAsync();

            var coachViewModels = new List<CoachViewModel>();

            foreach (var coach in coaches)
            {
                coachViewModels.Add(new CoachViewModel
                {
                    CoachId = coach.CoachId,
                    FirstName = coach.FirstName,
                    LastName = coach.LastName,
                    Biography = coach.Biography,
                });
            }

            return View(coachViewModels);
        }

        public async Task<IActionResult> List()
        {
            var members = await _context.Users
                .OfType<Member>()
                .ToListAsync();

            var memberViewModels = new List<MemberViewModel>();

            foreach (var member in members)
            {
                memberViewModels.Add(new MemberViewModel
                {
                    MemberId = member.MemberId,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    Active = member.Active,
                });
            }

            return View(memberViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMemberStatus(List<MemberViewModel> members)
        {
            foreach (var memberViewModel in members)
            {
                var member = await _context.Users
                    .OfType<Member>()
                    .FirstOrDefaultAsync(m => m.MemberId == memberViewModel.MemberId);

                if (member != null)
                {
                    // Update the Active status
                    member.Active = memberViewModel.Active;
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect back to the list view after the update
            return RedirectToAction("List");
        }

    }
}