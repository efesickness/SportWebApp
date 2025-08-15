using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Areas.Student.Models;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = "CanAccessStudentPanel")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //kurslarım sayfası
            var userId = _userManager.GetUserId(User);
            var registrations = await _context.Registrations
                .Where(r => r.UserId == userId)
                .Include(c => c.Course)
                .ThenInclude(b => b.Branch)
                .Include(r => r.CourseSession)
                .ThenInclude(cs => cs.Course)
                .ToListAsync();
            ViewBag.Registrations = registrations;
            return View();
        }

        public async Task<IActionResult> Details(int courseId, int courseSessionId)
        {
            var user = await _userManager.GetUserAsync(User);

            var course = await _context.Courses
                .Where(c => c.CourseId == courseId)
                .Include(c => c.Branch)
                .Include(c => c.CourseSessions)
                .ThenInclude(cs => cs.ApplicationUser)
                .FirstOrDefaultAsync();

            var registration = await _context.Registrations
                .Where(r => r.UserId == user.Id && r.CourseSessionId == courseSessionId)
                .FirstOrDefaultAsync();

            var sub = await _context.StudentSubscriptions
                .Where(s => s.RegistrationId == registration.RegistrationId)
                .FirstOrDefaultAsync();

            var payment = await _context.Payments
                    .Where(p => p.UserId == user.Id && p.SubscriptionId == sub.SubscriptionId)
                    .OrderByDescending(p => p.PaymentDate)
                    .FirstOrDefaultAsync();

            CourseDetailsViewModel courseDetails = new CourseDetailsViewModel()
            {
                branch = course.Branch.Name,
                courseName = course.Title,
                courseDescription = course.Description,
                instructor = $"{course.ApplicationUser.Name} {course.ApplicationUser.Surname}",
                lastPayment = payment.PaymentDate,
                nextPayment = payment.PaymentDate.AddMonths(1),
                courseSession = course.CourseSessions.Where(x => x.CourseSessionId == courseSessionId).Select(x => x.Title).FirstOrDefault(),
                lastPaymentAmount = payment.Amount,
            }
            ;


            return View(courseDetails);
        }

        public async Task<IActionResult> MyPayments()
        {
            var userId = _userManager.GetUserId(User);
            var payments = await _context.Payments
                .Where(p => p.UserId == userId)
                .Include(p => p.StudentSubscription)
                .ThenInclude(s => s.Registration)
                .ThenInclude(r => r.Course)
                .ThenInclude(b=>b.Branch)
                .ToListAsync();
            return View(payments);
        }
    }
}
