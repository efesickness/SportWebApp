using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = "CanAccessInstructorPanel")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //dashboard
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "CanAccessPendingRegistration")]
        public async Task<IActionResult> PendingRegistrations()
        {
            var pendingRegistrations = await _context.Registrations
                .Where(r => r.IsApproved == false && r.IsDeleted==false )
                .Include(r => r.ApplicationUser)
                .Include(c => c.Course)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
            if (pendingRegistrations.Count == 0)
            {
                TempData["Message"] = "Bekleyen başvuru yok.";
            }
            return View(pendingRegistrations);
        }

        [HttpPost]
        [Authorize(Policy = "CanApproveAndRejectPendingRegistration")]
        public async Task<IActionResult> PendingRegistrations(int id, bool isApproved)
        {
            var registration = await _context.Registrations
                .Include(r => r.ApplicationUser)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(r => r.RegistrationId == id);
            if (registration != null)
                registration.IsApproved = isApproved;
            if (registration != null)
            {
                if (isApproved)
                {
                    registration.RegistrationDate = DateTime.Now;
                    TempData["approveMessage"] = $"{registration.ApplicationUser.Name} {registration.ApplicationUser.Surname} kişisinin '{registration.Course.Title}' kursu kaydı onaylandı.";
                }
                else
                {
                    registration.IsDeleted = true;
                    TempData["rejectMessage"] = $"{registration.ApplicationUser.Name} {registration.ApplicationUser.Surname} kişisinin '{registration.Course.Title}' kursu kaydı reddedildi";
                }
                _context.Update(registration);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["Error"] = "Registration not found.";
            }
            return RedirectToAction("PendingRegistrations");
        }

        [Authorize(Roles = "CanAccessMyStudentsPanel")]
        public async Task<IActionResult> Students()
        {
            var instructorId = _userManager.GetUserId(User);
            var courses = await _context.CourseSessions
                .Where(c => c.UserId == instructorId && c.IsActive)
                .Select(x => x.CourseSessionId)
                .Distinct()
                .ToListAsync();
            List<ApplicationUser> students = new();
            foreach (var item in courses)
            {
                students = await _context.Registrations
                    .Where(r => r.CourseSessionId == item)
                    .Include(u => u.ApplicationUser)
                    .Select(u => u.ApplicationUser)
                    .Distinct()
                    .ToListAsync();
            }

            var allStudents = await _userManager.GetUsersInRoleAsync("Student");
            ViewBag.AllStudents = allStudents;

            return View(students);
        }

    }
}
