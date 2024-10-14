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



        // Updated to reflect the new view name EmailMembers
        public async Task<IActionResult> EmailMembers()
        {
            // Return the view with an empty form to input email subject and message
            return View();
        }

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
    }
}
