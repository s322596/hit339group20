using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anyone4Tennis.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anyone4Tennis.Models
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            return View(await _context.Schedules.ToListAsync());
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
                description = b.Description
            }).ToListAsync();

            return new JsonResult(events);
        }

        // POST: /Schedules/CreateBooking
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] Schedules schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Add(schedule);  // Add booking to the database
                await _context.SaveChangesAsync();  // Save changes to the database
                return Ok();  // Return success response
            }
            return BadRequest(ModelState);  // Return error response if validation fails
        }
    }
}