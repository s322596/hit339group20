using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anyone4Tennis.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Anyone4Tennis.Models
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SchedulesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var coaches = await _userManager.GetUsersInRoleAsync("Coach"); // Assuming "Coach" is the role name
            ViewBag.Coaches = coaches; // Pass the list of coaches to the view
            var schedules = await _context.Schedules.Include(s => s.Coach).ToListAsync(); // Fetch schedules
            return View(schedules);
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FirstOrDefaultAsync(m => m.SchedulesID == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // API for FullCalendar

        // GET: /Schedules/GetSchedules
        [HttpGet]
        public async Task<IActionResult> GetSchedules()
        {
            var events = await _context.Schedules.Select(b => new
            {
                id = b.SchedulesID,
                title = b.Title,
                start = b.StartTime,
                end = b.EndTime,
                description = b.Description,
                coachId = b.CoachId // Include CoachId if needed
            }).ToListAsync();

            return new JsonResult(events);
        }

        // POST: /Schedules/CreateSchedule
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] Schedules schedule)
        {
            if (ModelState.IsValid)
            {
                // CoachId is already provided, no need to set Coach manually
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                return Ok();  // Return success response
            }

            return BadRequest(ModelState);  // Return error response if validation fails
        }


    }
}