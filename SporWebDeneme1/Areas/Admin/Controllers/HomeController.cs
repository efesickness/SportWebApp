using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Areas.Admin.Models;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
using SporWebDeneme1.Models;

namespace SporWebDeneme1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard", "Home", new { area = "Admin" });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AdminLoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "Admin")))
            {
                ModelState.AddModelError("", "Geçersiz e-posta veya yetki yok.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                user.LastLoginDate = DateTime.Now;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Dashboard", "Home", new { area = "Admin" });
            }

            ModelState.AddModelError("", "Giriş başarısız.");
            return View(model);
        }
        [Authorize(Policy = "CanAccessAdminPanel")]
        public IActionResult Dashboard()
        {
            var InstructorRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Instructor")?.Id;
            var StudentRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Student")?.Id;

            var model = new AdminDashboardViewModel
            {

                TotalCourses = _context.Courses.Count(),
                TotalInstructors = _context.UserRoles.Where(r => r.RoleId == InstructorRoleId).Count(),
                TotalStudents = _context.UserRoles.Where(r => r.RoleId == StudentRoleId).Count(),
                TotalRegistrations = _context.Registrations.Count(),
                TotalRevenue = _context.Payments.AsEnumerable().Sum(r => r.Amount),
                TopCourses = _context.Registrations
                                .GroupBy(r => r.Course.Title)
                                .Select(g => new { CourseTitle = g.Key, Count = g.Count() })
                                .OrderByDescending(g => g.Count)
                                .Take(5)
                                .ToList()
                                .Select(g => (g.CourseTitle, g.Count))
                                .ToList(),
                RecentRegistrations = _context.Registrations
                                            .Include(r => r.ApplicationUser)
                                            .Include(c=>c.Course)
                                            .ThenInclude(b => b.Branch)
                                            .Include(r => r.CourseSession)
                                            .ThenInclude(cs => cs.Course)
                                            .OrderByDescending(r => r.RegistrationDate)
                                            .Take(10)
                                            .ToList(),
                RecentRegistrants = _context.ApplicationUsers
                                            .OrderByDescending(r => r.RegistrationDate)
                                            .Take(10)
                                            .ToList(),
                RecentLogins = _context.ApplicationUsers
                                            .Where(u => u.LastLoginDate.HasValue)
                                            .OrderByDescending(u => u.LastLoginDate.Value)
                                            .Take(10)
                                            .ToList(),
                StudentSubscriptions = _context.StudentSubscriptions
                                            .Include(ss => ss.User)
                                            .Include(ss => ss.Registration)
                                            .OrderByDescending(ss => ss.CreatedAt)
                                            .Take(10)
                                            .ToList(),
                Payments = _context.Payments
                                            .Include(p => p.StudentSubscription)
                                            .ThenInclude(ss => ss.User)
                                            .OrderByDescending(p => p.PaymentDate)
                                            .Take(10)
                                            .ToList(),
            };


            return View(model);
        }
    }
}
