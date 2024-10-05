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
                    Photo = coach.Photo
                });
            }

            return View(coachViewModels);
        }
    }
}