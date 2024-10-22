using Anyone4Tennis.Data;
using Anyone4Tennis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // Add this for logging
using System.Security.Claims;

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
                title = string.Format("{0} - {1:t} - {2:t}",
                                      b.Title,
                                      b.StartTime,
                                      b.EndTime),
                start = b.StartTime,
                end = b.EndTime,
                location = b.Location,
                coachId = b.CoachId,
                coachName = b.Coach.FirstName + " " + b.Coach.LastName
            }).ToListAsync();

        return new JsonResult(events);
    }

    [Authorize(Roles = "Admin,Coach")]
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
        // Get the currently logged-in user
        var currentUser = await _userManager.GetUserAsync(User);
        var today = DateTime.Today;

        if (currentUser is Coach) // Check if the logged-in user is a coach
        {
            // Filter the schedules by CoachId and include Coach details
            var schedules = await _context.Schedules
                .Include(s => s.Coach)  // Include the Coach entity to access coach details
                .Where(s => s.Coach.Id == currentUser.Id && s.StartTime >= today)
                .ToListAsync();
            return View(schedules);
        }

        if (currentUser is Member) // Check if the logged-in user is a member
        {
            // Get the IDs of schedules the member is already enrolled in
            var enrolledScheduleIds = await _context.MemberSchedules
                .Where(ms => ms.MemberFK == currentUser.Id) // Assuming MemberFK stores the member's ID
                .Select(ms => ms.ScheduleID) // Select the ScheduleID
                .ToListAsync();

            // Get all schedules except those the member is already enrolled in and include Coach details
            var availableSchedules = await _context.Schedules
                .Include(s => s.Coach)  // Include the Coach entity to access coach details
                .Where(s => !enrolledScheduleIds.Contains(s.SchedulesID) && s.StartTime >= today) // Filter out enrolled schedules
                .ToListAsync();

            return View(availableSchedules);
        }

        // If no specific role is detected or no user is logged in, return all schedules and include Coach details
        var allSchedules = await _context.Schedules
            .Include(s => s.Coach)  // Include the Coach entity to access coach details
            .Where(s => s.StartTime >= today)
            .ToListAsync();
        return View(allSchedules);
    }



    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Enroll(int scheduleId)
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var member = await _context.Users.FindAsync(memberId);
        var schedule = await _context.Schedules.FindAsync(scheduleId);

        // Log member and schedule for debugging
        if (member == null)
        {
            Console.WriteLine($"Member not found: {memberId}");
            return NotFound("Member not found.");
        }

        if (schedule == null)
        {
            Console.WriteLine($"Schedule not found: {scheduleId}");
            return NotFound("Schedule not found.");
        }

        // Proceed with creating the member-schedule relationship
        var memberSchedule = new MemberSchedule
        {
            MemberFK = memberId,
            ScheduleID = scheduleId
        };

        _context.MemberSchedules.Add(memberSchedule);
        await _context.SaveChangesAsync();

        return RedirectToAction("ScheduleList"); // Or another action
    }

    public async Task<IActionResult> Members(int id)
    {
        var schedule = await _context.Schedules
            .Include(s => s.MemberSchedules)
            .ThenInclude(ms => ms.Member) // Include the related Member
            .FirstOrDefaultAsync(s => s.SchedulesID == id);

        if (schedule == null)
        {
            return NotFound(); // Return a 404 if the schedule is not found
        }

        return View(schedule.MemberSchedules.Select(ms => ms.Member)); // Pass the members to the view
    }
}