using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Areas.Course.Models;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = "CanAccessCourseSession")]
    public class CourseSessionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseSessionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int courseId)
        {
            var returnUrl = Request.Path + Request.QueryString;

            ViewData["ReturnUrl"] = returnUrl;

            var sessions = await _context.CourseSessions
                .Where(x => x.CourseId == courseId)
                .Include(r => r.Course)
                .Include(a => a.ApplicationUser)
                .Include(cs => cs.Registrations)
                .OrderByDescending(s => s.CourseSessionId)
                .ToListAsync();
            if (sessions == null || !sessions.Any())
                return NotFound("No sessions found for the specified course.");

            return View(sessions);
        }
        [Authorize(Policy = "CanAddEditAndDeleteCourseSession")]
        public async Task<IActionResult> Create(int courseId)
        {

            ViewBag.CourseId = courseId;

            CourseSessionViewModel session = new CourseSessionViewModel
            {
                CourseId = courseId,
                Days = Enum.GetValues(typeof(DayOfWeek))
                    .Cast<DayOfWeek>()
                    .Select(day => new DaySelectionViewModel
                    {
                        Day = day,
                        IsSelected = false,
                        StartTime = null,
                        EndTime = null
                    })
                    .ToList()
            };

            ViewBag.Courses = await _context.Courses.Where(x => x.IsActive == true).ToListAsync();
            var allUsers = await _userManager.GetUsersInRoleAsync("Instructor");
            ViewBag.Instructors = allUsers;
            ViewBag.Assignments = await _context.BranchAssignments
                .Include(b => b.Branch)
                .Include(b => b.ApplicationUser)
                .ToListAsync();
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteCourseSession")]
        public async Task<IActionResult> Create(CourseSessionViewModel session, List<int> SelectedDays)
        {

            var course = await _context.Courses.FindAsync(session.CourseId);
            if (course != null)
                session.AvailableCapacity = course.Capacity;

            if (ModelState.IsValid)
            {
                var model = new CourseSession
                {
                    CourseId = session.CourseId,
                    UserId = session.UserId,
                    Title = session.Title,
                    Description = session.Description,
                    AvailableCapacity = session.AvailableCapacity,
                    IsActive = session.IsActive,
                };
                foreach (var day in session.Days.Where(d => d.IsSelected))
                {
                    model.CourseSessionDays.Add(new CourseSessionDay
                    {
                        DayOfWeek = day.Day,
                        StartTime = day.StartTime.Value,
                        EndTime = day.EndTime.Value
                    });
                }
                _context.CourseSessions.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "CourseSession", new { courseId = session.CourseId });
            }

            ViewBag.Courses = await _context.Courses.ToListAsync();
            ViewBag.Instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            return View(session);
        }


        [Authorize(Policy = "CanAddEditAndDeleteCourseSession")]
        public async Task<IActionResult> Delete(int id)
        {
            var session = await _context.CourseSessions.FindAsync(id);
            if (session == null) return NotFound();

            _context.CourseSessions.Remove(session);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "CourseSession", new { courseId = session.CourseId });
        }

        [Authorize(Policy = "CanAddEditAndDeleteCourseSession")]
        public async Task<IActionResult> Edit(int id, string returnUrl, int courseId)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.CourseId = courseId;

            var session = await _context.CourseSessions
                .Include(cs => cs.CourseSessionDays) // gün saat ilişkisi varsa buradan çek
                .FirstOrDefaultAsync(cs => cs.CourseSessionId == id);

            if (session == null) return NotFound();

            var viewModel = new CourseSessionViewModel
            {
                CourseSessionId = session.CourseSessionId,
                CourseId = session.CourseId,
                UserId = session.UserId,
                AvailableCapacity = session.AvailableCapacity,
                IsActive = session.IsActive,
                Title = session.Title,
                Description = session.Description,
                Days = Enum.GetValues(typeof(DayOfWeek))
                    .Cast<DayOfWeek>()
                    .Select(day => new DaySelectionViewModel
                    {
                        Day = day,
                        IsSelected = session.CourseSessionDays.Any(d => d.DayOfWeek == day),
                        StartTime = session.CourseSessionDays.FirstOrDefault(d => d.DayOfWeek == day)?.StartTime,
                        EndTime = session.CourseSessionDays.FirstOrDefault(d => d.DayOfWeek == day)?.EndTime
                    })
                    .ToList()
            };

            ViewBag.Courses = await _context.Courses.ToListAsync();
            ViewBag.Instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            ViewBag.Assignments = await _context.BranchAssignments
                .Include(b => b.Branch)
                .Include(b => b.ApplicationUser)
                .ToListAsync();
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteCourseSession")]
        public async Task<IActionResult> Edit(CourseSessionViewModel model, string returnUrl)
        {
            returnUrl ??= "Index";

            if (ModelState.IsValid)
            {
                var existingSession = await _context.CourseSessions
                    .Include(cs => cs.CourseSessionDays)
                    .FirstOrDefaultAsync(cs => cs.CourseSessionId == model.CourseSessionId);

                if (existingSession == null) return NotFound();

                // Ana bilgileri güncelle
                existingSession.CourseId = model.CourseId;
                existingSession.UserId = model.UserId;
                existingSession.AvailableCapacity = model.AvailableCapacity;
                existingSession.IsActive = model.IsActive;
                existingSession.Title = model.Title;
                existingSession.Description = model.Description;

                // Gün-saatleri güncelle
                existingSession.CourseSessionDays.Clear();
                foreach (var day in model.Days.Where(d => d.IsSelected))
                {
                    existingSession.CourseSessionDays.Add(new CourseSessionDay
                    {
                        DayOfWeek = day.Day,
                        StartTime = day.StartTime,
                        EndTime = day.EndTime
                    });
                }

                _context.CourseSessions.Update(existingSession);
                await _context.SaveChangesAsync();

                TempData["EditSuccessMessage"] = "Kurs grubu başarıyla güncellendi.";
            }
            else
            {
                TempData["EditErrorMessage"] = "Kurs grubu güncellemesi başarısız oldu.";
            }

            ViewBag.Courses = await _context.Courses.ToListAsync();
            ViewBag.Instructors = await _userManager.GetUsersInRoleAsync("Instructor");


            return Redirect(returnUrl);
        }

        [Authorize(Policy = "CanAddStudentToCourseSession")]
        public async Task<IActionResult> AddStudent(int id)
        {
            var session = await _context.CourseSessions
                .Include(s => s.Registrations)
                .FirstOrDefaultAsync(s => s.CourseSessionId == id);
            if (session == null) return NotFound();
            //var students = await _userManager.GetUsersInRoleAsync("Student");
            var approvedStudents = await (from r in _context.Registrations
                                          join ur in _context.UserRoles on r.UserId equals ur.UserId
                                          join role in _context.Roles on ur.RoleId equals role.Id
                                          join sub in _context.StudentSubscriptions on r.RegistrationId equals sub.RegistrationId
                                          join p in _context.Payments on sub.SubscriptionId equals p.SubscriptionId
                                          where r.IsApproved
                                                && !r.IsDeleted
                                                && role.Name == "Student"
                                                && r.CourseId == session.CourseId
                                                && sub.EndDate >= DateTime.Now // aboneliği devam eden öğrenciler
                                                && r.CourseSessionId == null // sadece bu kursa kayıtlı olmayan öğrenciler
                                          select r.ApplicationUser)
                        .Distinct()
                        .ToListAsync();
            AddStudentViewModel model = new AddStudentViewModel
            {
                CourseSession = session,
                Students = approvedStudents,
                SelectedCourseSessionId = id,
            };

            model.CourseSession.Course = _context.Courses.Find(session.CourseId);
            return View(model);
        }

        [Authorize(Policy = "CanAddStudentToCourseSession")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(AddStudentViewModel model)
        {
            var session = await _context.CourseSessions
            .Include(s => s.Registrations)
            .Include(c => c.Course)
            .FirstOrDefaultAsync(s => s.CourseSessionId == model.SelectedCourseSessionId);

            if (session == null) return NotFound();
            model.CourseSession = session;
            model.CourseSession.Course = _context.Courses.Find(session.CourseId);
            model.Students = await _userManager.GetUsersInRoleAsync("Student");

            var rdate = model.Registration?.RegistrationDate ?? DateTime.Now;
            model.Registration = new Registration
            {
                RegistrationDate = rdate
            };
            model.Payment = new Payment
            {
                Status = PaymentStatus.Paid
            };


            if (model.CourseSession.AvailableCapacity <= 0)
            {
                ModelState.AddModelError("", "Bu kayıtın kapasitesi dolmuştur, daha fazla öğrenci ekleyemezsiniz");
                return View(model);
            }

            if (_context.Registrations.Any(x => x.UserId == model.SelectedUserId && x.CourseSessionId == model.SelectedCourseSessionId))
            {
                ModelState.AddModelError("", "Bu öğrenci zaten bu kursa kayıtlı.");
                return View(model);
            }

            var registration = await _context.Registrations.Where(x => x.CourseId == model.CourseSession.CourseId && x.UserId == model.SelectedUserId).FirstOrDefaultAsync();
            registration.CourseSessionId = model.SelectedCourseSessionId;
            model.CourseSession.AvailableCapacity--;
            _context.Registrations.Update(registration);
            _context.CourseSessions.Update(model.CourseSession);
            await _context.SaveChangesAsync();

            var subscription = _context.StudentSubscriptions.FirstOrDefault(x => x.RegistrationId == registration.RegistrationId);

            await _context.SaveChangesAsync();

            var payment = _context.Payments
                .Where(x => x.UserId == model.SelectedUserId && x.SubscriptionId == subscription.SubscriptionId)
                .OrderByDescending(x => x.PaymentDate)
                .FirstOrDefault();


            payment.UserId = model.SelectedUserId;
            payment.Amount = 0;
            payment.Currency = "TRY";
            payment.PaymentDate = DateTime.Now;
            payment.Status = PaymentStatus.Paid;
            payment.Method = PaymentMethod.Other;
            payment.PaymentProvider = "Manual";
            payment.SubscriptionId = subscription.SubscriptionId;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { courseId = model.CourseSession.CourseId });
        }


        public async Task<IActionResult> Details(int id)
        {
            var session = await _context.CourseSessions
                .Include(s => s.Course)
                .Include(s => s.CourseSessionDays)
                .Include(s => s.Registrations)
                .ThenInclude(r => r.ApplicationUser)
                .FirstOrDefaultAsync(s => s.CourseSessionId == id);

            if (session == null) return NotFound();

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanDeleteStudentFromCourseSession")]
        public async Task<IActionResult> DeleteStudent(int registrationId, int courseSessionId)
        {
            var registration = await _context.Registrations
                .Include(r => r.CourseSession)
                .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);
            if (registration == null) return NotFound();

            registration.CourseSessionId = null;
            _context.Registrations.Update(registration);

            var courseSession = await _context.CourseSessions.FindAsync(courseSessionId);
            if (courseSession != null)
            {
                courseSession.AvailableCapacity++;
                _context.CourseSessions.Update(courseSession);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { courseId = courseSession?.CourseId });
        }
    }
}
