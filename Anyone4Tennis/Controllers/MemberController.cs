using Anyone4Tennis.Data;
using Anyone4Tennis.Models;
using Anyone4Tennis.Models.ViewModels;
using Anyone4Tennis.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anyone4Tennis.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public MemberController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List(string searchString)
        {
            // Start with the queryable collection of members
            IQueryable<Member> membersQuery = _context.Users.OfType<Member>();

            // Apply search filtering if searchString is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                membersQuery = membersQuery.Where(m =>
                    m.FirstName.Contains(searchString) ||   // Search by First Name
                    m.LastName.Contains(searchString) ||    // Search by Last Name
                    m.Email.Contains(searchString));         // Search by Email
            }

            // Execute the query and get the list of members
            var members = await membersQuery.ToListAsync();

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

        [Authorize(Roles = "Admin,Coach")]
        [HttpPost]
        public async Task<IActionResult> UpdateMemberStatus(int MemberId, bool Active)
        {
            // Find the member in the database
            var member = await _context.Users
                .OfType<Member>()
                .FirstOrDefaultAsync(m => m.MemberId == MemberId);

            if (member != null)
            {
                // Update the Active status
                member.Active = Active;

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            // Redirect back to the list view after the update
            return RedirectToAction("List");
        }


        [Authorize(Roles = "Admin,Coach")]
        // Updated to reflect the new view name EmailMembers
        public async Task<IActionResult> EmailMembers()
        {
            // Return the view with an empty form to input email subject and message
            return View();
        }
        [Authorize(Roles = "Admin,Coach")]
        [HttpPost]
        public async Task<IActionResult> EmailMembers(string subject, string message)
        {
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                ViewBag.Status = "Subject and Message cannot be empty.";
                return View();
            }

            // Get all active members
            var activeMembers = await _context.Users
                .OfType<Member>()
                .Where(m => m.Active)
                .ToListAsync();

            // Send email to each active member
            foreach (var member in activeMembers)
            {
                try
                {
                    await _emailSender.SendEmailAsync(member.Email, subject, message);
                }
                catch (Exception ex)
                {
                    // Log the failure and continue sending to other members
                    ViewBag.Status = $"Error sending email to {member.Email}: {ex.Message}";
                }
            }

            ViewBag.Status = "Emails sent successfully!";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyEnrollments()
        {
            // Get the logged-in user's ID
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the member's enrolled schedules
            var enrollments = await _context.MemberSchedules
                .Where(ms => ms.MemberFK == memberId)
                .Include(ms => ms.Schedules) // Include the Schedule details
                .ThenInclude(s => s.Coach)
                .Select(ms => new EnrollmentViewModel
                {
                    MemberScheduleId = ms.MemberScheduleId,
                    Title = ms.Schedules.Title,
                    Location = ms.Schedules.Location,
                    StartTime = ms.Schedules.StartTime,
                    EndTime = ms.Schedules.EndTime,
                    CoachName = ms.Schedules.Coach.FirstName + " " + ms.Schedules.Coach.LastName
                })
                .ToListAsync();

            return View(enrollments);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Unenroll(int scheduleId)
        {
            // Get the logged-in user's ID
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the enrollment in the database
            var enrollment = await _context.MemberSchedules
                .FirstOrDefaultAsync(ms => ms.MemberScheduleId == scheduleId && ms.MemberFK == memberId);

            if (enrollment != null)
            {
                // Remove the enrollment
                _context.MemberSchedules.Remove(enrollment);
                await _context.SaveChangesAsync();
                TempData["Message"] = "You have successfully unenrolled from the schedule.";
            }
            else
            {
                TempData["Error"] = "You are not enrolled in this schedule.";
            }

            return RedirectToAction("MyEnrollments");
        }
    }
}
