/*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Areas.Course.Models;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
namespace SporWebDeneme1.Areas.Admin.Controllers.temp
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {

            var returnUrl = Request.Path + Request.QueryString;
            ViewBag.ReturnUrl = returnUrl;
            var courses = await _context.Courses
                .Include(c => c.CourseSessions)
                .Include(c => c.Branch)
                .OrderByDescending(x=>x.CourseId)
                .ToListAsync();
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var allUsers = await _userManager.GetUsersInRoleAsync("Instructor");
            var branches = await _context.Branches.ToListAsync();
            
            ViewBag.Instructors = allUsers;
            ViewBag.Branches = branches;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel course)
        {

            if (course.Capacity <= 0)
            {
                ModelState.AddModelError("Capacity", "Kapasite 0'dan yüksek olmalı");
            }
            if (ModelState.IsValid)
            {
                _context.Courses.Add(new Entities.Models.Course
                {
                    UserId = course.UserId,
                    Title = course.Title,
                    Description = course.Description,
                    Capacity = course.Capacity,
                    IsActive = course.IsActive,
                    Price = course.Price,
                    BranchId = course.BranchId,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? "Index";

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            var allUsers = await _userManager.GetUsersInRoleAsync("Instructor");
            ViewBag.Instructors = allUsers;
            var courseViewModel = new CourseViewModel
            {
                CourseId = course.CourseId,
                UserId = course.UserId,
                Title = course.Title,
                Description = course.Description,
                Capacity = course.Capacity,
                IsActive = course.IsActive,
                Price = course.Price
            };
            return View(courseViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseViewModel course, string returnUrl)
        {
            returnUrl ??= "Index";

            if (ModelState.IsValid)
            {
                var existingCourse = await _context.Courses.FindAsync(course.CourseId);
                if (existingCourse == null) return NotFound();
                existingCourse.UserId = course.UserId;
                existingCourse.Title = course.Title;
                existingCourse.Description = course.Description;
                existingCourse.Capacity = course.Capacity;
                existingCourse.IsActive = course.IsActive;
                existingCourse.Price = course.Price;
                _context.Courses.Update(existingCourse);
                await _context.SaveChangesAsync();
                TempData["EditSuccessMessage"] = "Kurs başarıyla güncellendi.";
                return Redirect(returnUrl);
            }
            TempData["EditErrorMessage"] = "Kurs güncellemesi başarısız oldu.";
            return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(int id)
        {
            var course = _context.Courses.Find(id);
            if (course is not null)
            {
                course.IsActive = true;
                _context.Courses.Update(course);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Deactivate(int id)
        {
            var course = _context.Courses.Find(id);
            if (course is not null)
            {
                course.IsActive = false;
                _context.Courses.Update(course);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
*/