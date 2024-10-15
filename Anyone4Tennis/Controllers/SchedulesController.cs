using Anyone4Tennis.Data;
using Anyone4Tennis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // Add this for logging

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
        var schedules = await _context.Schedules.Include(s => s.Coach).ToListAsync();
        return View(schedules);
    }

    // API to retrieve schedules for FullCalendar
    [HttpGet]
    public async Task<IActionResult> GetSchedules()
    {
        var events = await _context.Schedules
            .Include(b => b.Coach)
            .Select(b => new
            {
                id = b.SchedulesID,
                title = b.Title + " - " + b.Coach.FirstName + " " + b.Coach.LastName,  // Show Coach name with title
                start = b.StartTime,
                end = b.EndTime,
                Location = b.Location,
                coachId = b.CoachId,
                coachName = b.Coach.FirstName + " " + b.Coach.LastName
            }).ToListAsync();

        return new JsonResult(events);
    }

    // POST: /Schedules/CreateSchedule
    [HttpPost]
    public async Task<IActionResult> CreateSchedule([FromBody] Schedules schedule)
    {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return Ok();
    }

    // API to fetch coaches for the dropdown
    [HttpGet]
    public async Task<IActionResult> GetCoaches()
    {
        var coaches = await _context.Coaches
            .Select(c => new
            {
                id = c.Id,  // Id is from IdentityUser
                firstName = c.FirstName,
                lastName = c.LastName
            }).ToListAsync();

        return new JsonResult(coaches);
    }
}
