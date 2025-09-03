using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
using System.Security.Claims;

namespace SporWebDeneme1.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = "CanAccessStudentPanel")]
    public class ActiveCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActiveCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(x=>x.Branch)
                .Where(x => x.IsActive == true).ToListAsync();
            ViewBag.Courses = courses;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.CourseSessions)
                .Include(b => b.Branch)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (course == null)
            {
                return NotFound();
            }   
            return View(course);
        }

        //bitir burayı
        [HttpPost]
        public async Task<IActionResult> Register(Entities.Models.Course course)
        {
            var IsUserAlreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && r.CourseId == course.CourseId && !r.IsDeleted);
            if (IsUserAlreadyRegistered)
            {
                TempData["ErrorMessage"] = "Bu kursa zaten kayıtlısınız!";
                return RedirectToAction("Index", "ActiveCourses", new { area = "Student" });
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var registration = new Registration
            {
                UserId = userId,
                RegistrationDate = DateTime.Now,
                CourseId = course.CourseId
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            var studentSubscriptions = new StudentSubscription
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1).AddDays(1),
                UserId = userId,
                RegistrationId = registration.RegistrationId
            };
            _context.StudentSubscriptions.Add(studentSubscriptions);
            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                PaymentDate = DateTime.Now,
                UserId = userId,
                Method = PaymentMethod.EFT,
                SubscriptionId = studentSubscriptions.SubscriptionId,
                Status = PaymentStatus.Pending,
                PaymentProvider = "Manual",
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kursa başarıyla kaydoldunuz!";
            return RedirectToAction("Index");
        }
    }
}
