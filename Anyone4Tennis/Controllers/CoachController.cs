﻿using Anyone4Tennis.Data;
using Anyone4Tennis.Models;
using Anyone4Tennis.Models.ViewModels;
using Anyone4Tennis.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anyone4Tennis.Controllers
{
    public class CoachController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
    
        public CoachController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> List()
        {
            var coaches = await _context.Users
                .OfType<Coach>()
                .ToListAsync();

            var coachViewModels = new List<CoachViewModel>();

            foreach (var coach in coaches)
            {
                coachViewModels.Add(new CoachViewModel
                {
                    CoachId = coach.Id,
                    FirstName = coach.FirstName,
                    LastName = coach.LastName,
                    Biography = coach.Biography,
                    Photo = coach.Photo
                });
            }

            return View(coachViewModels);
        }
        public async Task<IActionResult> Details(string id)
        {
            var coach = await _context.Users
                .OfType<Coach>()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (coach == null)
            {
                return NotFound();
            }

            var coachViewModel = new CoachViewModel
            {
                CoachId = coach.Id,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Biography = coach.Biography,
                Photo = coach.Photo
            };

            return View(coachViewModel);
        }
    }
}
