using Anyone4Tennis.Data;
using Anyone4Tennis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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

    public async Task<IActionResult> ScheduleList(string searchString)
    {
        // Get the currently logged-in user
        var currentUser = await _userManager.GetUserAsync(User);
        var today = DateTime.Today;

        IQueryable<Schedules> schedulesQuery = _context.Schedules
            .Include(s => s.Coach)  // Include the Coach entity to access coach details
            .Where(s => s.StartTime >= today);

        // Apply search filtering if searchString is provided
        if (!string.IsNullOrEmpty(searchString))
        {
            schedulesQuery = schedulesQuery.Where(s =>
                s.Title.Contains(searchString) ||               // Search by Title
                s.Location.Contains(searchString) ||            // Search by Location
                (s.Coach.FirstName.Contains(searchString) ||    // Search by Coach's First Name
                 s.Coach.LastName.Contains(searchString)));      // Search by Coach's Last Name
        }

        if (currentUser is Coach)
        {
            schedulesQuery = schedulesQuery.Where(s => s.CoachId == currentUser.Id);
        }

        if (currentUser is Member)
        {
            var enrolledScheduleIds = await _context.MemberSchedules
                .Where(ms => ms.MemberFK == currentUser.Id)
                .Select(ms => ms.ScheduleID)
                .ToListAsync();

            schedulesQuery = schedulesQuery.Where(s => !enrolledScheduleIds.Contains(s.SchedulesID));
        }

        var schedules = await schedulesQuery.ToListAsync();
        return View(schedules);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var schedule = await _context.Schedules
            .Include(s => s.Coach)  // Include the Coach entity to access coach details
            .FirstOrDefaultAsync(m => m.SchedulesID == id);

        if (schedule == null)
        {
            return NotFound();
        }

        return View(schedule);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Enroll(int scheduleId, [FromServices] IEmailSender emailSender)
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var member = await _context.Users.FindAsync(memberId);

        // Fetch the schedule and include the coach details
        var schedule = await _context.Schedules
            .Include(s => s.Coach) // Include the Coach entity to access the coach's details
            .FirstOrDefaultAsync(s => s.SchedulesID == scheduleId);

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

        // Prepare email content, including coach information
        var subject = "Schedule Enrollment Confirmation";
        var message = $@"
        <h1>Schedule Enrollment Confirmation</h1>
        <p>Dear {member.UserName},</p>
        <p>You have successfully enrolled in the following schedule:</p>
        <ul>
            <li><strong>Title:</strong> {schedule.Title}</li>
            <li><strong>Location:</strong> {schedule.Location}</li>
            <li><strong>Start Time:</strong> {schedule.StartTime.ToString("g")}</li>
            <li><strong>End Time:</strong> {schedule.EndTime.ToString("g")}</li>
            <li><strong>Coach:</strong> {schedule.Coach.FirstName} {schedule.Coach.LastName}</li>
        </ul>
        <p>Thank you for enrolling!</p>
    ";

        // Send the email to the user
        await emailSender.SendEmailAsync(member.Email, subject, message);

        return RedirectToAction("ScheduleList"); // Redirect after enrollment
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