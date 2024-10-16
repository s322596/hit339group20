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
                title = string.Format("{0} - {1} - {2:t} - {3:t} - {4} {5}",
                                      b.Title,
                                      b.Location,
                                      b.StartTime,
                                      b.EndTime,
                                      b.Coach.FirstName,
                                      b.Coach.LastName), 
                start = b.StartTime,
                end = b.EndTime,
                location = b.Location,
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

    public async Task<IActionResult> ScheduleList()
    {
        return View(await _context.Schedules.ToListAsync());
    }
}