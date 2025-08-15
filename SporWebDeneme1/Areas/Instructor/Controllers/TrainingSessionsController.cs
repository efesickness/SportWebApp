using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Areas.Instructor.Models;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = "CanAccessTrainingPanel")]
    public class TrainingSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingSessionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> Create()
        {
            var model = new TrainingSessionViewModel
            {
                CourseSessions = await _context.CourseSessions.ToListAsync(),
                Instructors = await _userManager.GetUsersInRoleAsync("Instructor"),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> Create(TrainingSessionViewModel model)
        {
            try
            {

                var entity = new TrainingSession
                {
                    CourseSessionId = model.CourseSessionId,
                    UserId = model.UserId,
                    Date = model.Date.Value,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Note = model.Note
                };
                _context.TrainingSessions.Add(entity);
                await _context.SaveChangesAsync();

                var students = await _context.Registrations
                    .Where(r => r.CourseSessionId == model.CourseSessionId)
                    .Select(r => r.UserId)
                    .ToListAsync();
                foreach (var student in students)
                {
                    var attendance = new TrainingAttendance
                    {
                        TrainingSessionId = entity.TrainingSessionId,
                        UserId = student,
                        IsPresent = false,
                        AttendanceDate = DateTime.Now,

                    };
                    _context.TrainingAttendances.Add(attendance);
                }

                if (model.SelectedTrainingAssistants != null && model.SelectedTrainingAssistants.Any())
                {

                    foreach (var assistantId in model.SelectedTrainingAssistants)
                    {
                        var trainingAssistant = new TrainingAssistant
                        {
                            TrainingSessionId = entity.TrainingSessionId,
                            UserId = assistantId
                        };
                        _context.TrainingAssistants.Add(trainingAssistant);
                    }
                }

                if (model.SelectedTrainingStaffs != null && model.SelectedTrainingStaffs.Any())
                {
                    foreach (var staffId in model.SelectedTrainingStaffs)
                    {
                        var trainingStaff = new TrainingStaff
                        {
                            TrainingSessionId = entity.TrainingSessionId,
                            UserId = staffId
                        };
                        _context.TrainingStaffs.Add(trainingStaff);
                    }
                }


                await _context.SaveChangesAsync();
                return RedirectToAction("Calendar");
            }
            catch
            {
                model.CourseSessions = await _context.CourseSessions.ToListAsync();
                model.Instructors = await _userManager.GetUsersInRoleAsync("Instructor");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> CreateSeries(TrainingSessionViewModel model, int[] SelectedDays)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Oluşturma başarısız oldu.";

                return View("Calendar");
            }
            var series = new TrainingSessionSeries
            {
                CourseSessionId = model.CourseSessionId,
                UserId = model.UserId,
                StartDate = model.StartDate.Value,
                EndDate = model.EndDate.Value,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Note = model.Note
            };
            _context.TrainingSessionSeries.Add(series);
            await _context.SaveChangesAsync();

            foreach (var day in SelectedDays)
            {
                _context.TrainingSessionSeriesDays.Add(new TrainingSessionSeriesDay
                {
                    TrainingSessionSeriesId = series.SeriesId,
                    DayOfWeek = (DayOfWeek)day
                });
            }

            await _context.SaveChangesAsync();


            DateTime date = series.StartDate;
            while (date <= series.EndDate)
            {
                if (SelectedDays.Contains((int)date.DayOfWeek))
                {
                    var session = new TrainingSession
                    {
                        SeriesId = series.SeriesId,
                        CourseSessionId = series.CourseSessionId,
                        UserId = series.UserId,
                        Date = date,
                        StartTime = series.StartTime,
                        EndTime = series.EndTime,
                        Note = series.Note,
                    };

                    if (model.SelectedTrainingAssistants != null && model.SelectedTrainingAssistants.All(id => !string.IsNullOrEmpty(id)))
                    {
                        session.TrainingAssistants = model.SelectedTrainingAssistants
                            .Select(assistantId => new TrainingAssistant
                            {
                                UserId = assistantId
                            })
                            .ToList();
                    }

                    if (model.SelectedTrainingStaffs != null && model.SelectedTrainingStaffs.All(id => !string.IsNullOrEmpty(id)))
                    {
                        session.TrainingStaffs = model.SelectedTrainingStaffs
                            .Select(staffId => new TrainingStaff
                            {
                                UserId = staffId
                            })
                            .ToList();
                    }

                    _context.TrainingSessions.Add(session);
                    await _context.SaveChangesAsync();

                    var students = await _context.Registrations
                        .Where(r => r.CourseSessionId == model.CourseSessionId)
                        .Select(r => r.UserId)
                        .ToListAsync();

                    foreach (var student in students)
                    {
                        var attendance = new TrainingAttendance
                        {
                            TrainingSessionId = session.TrainingSessionId,
                            UserId = student,
                            IsPresent = false,
                            AttendanceDate = DateTime.Now
                        };
                        _context.TrainingAttendances.Add(attendance);
                    }

                }
                date = date.AddDays(1);
            }
            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] = "Antrenmanlar başarıyla oluşturuldu.";
            return RedirectToAction("Calendar");
        }

        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> EditSeries(TrainingSessionViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> EditSeries(TrainingSessionViewModel model, int seriesId)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Güncelleme başarısız oldu.";
                return View("Calendar");
            }

            var series = await _context.TrainingSessionSeries
                .Include(s => s.Days)
                .FirstOrDefaultAsync(s => s.SeriesId == seriesId);

            if (series == null)
                return NotFound();

            // Seriyi güncelle
            series.CourseSessionId = model.CourseSessionId;
            series.UserId = model.UserId;
            series.StartTime = model.StartTime;
            series.EndTime = model.EndTime;
            series.Note = model.Note;


            var existingSessions = await _context.TrainingSessions
                .Include(ts => ts.TrainingAssistants)
                .Include(ts => ts.TrainingStaffs)
                .Where(ts => ts.SeriesId == seriesId)
                .ToListAsync();

            _context.TrainingAssistants.RemoveRange(existingSessions.SelectMany(s => s.TrainingAssistants));
            _context.TrainingStaffs.RemoveRange(existingSessions.SelectMany(s => s.TrainingStaffs));
            _context.TrainingSessions.RemoveRange(existingSessions);


            DateTime date = series.StartDate;
            while (date <= series.EndDate)
            {
                if (series.Days.Any(d => d.DayOfWeek == date.DayOfWeek))
                {
                    var session = new TrainingSession
                    {
                        SeriesId = series.SeriesId,
                        CourseSessionId = series.CourseSessionId,
                        UserId = series.UserId,
                        Date = date,
                        StartTime = series.StartTime,
                        EndTime = series.EndTime,
                        Note = series.Note
                    };

                    _context.TrainingSessions.Add(session);
                    await _context.SaveChangesAsync();

                    if (model.SelectedTrainingAssistants != null && model.SelectedTrainingAssistants.Any())
                    {
                        session.TrainingAssistants = model.SelectedTrainingAssistants
                            .Select(aid => new TrainingAssistant
                            {
                                TrainingSessionId = session.TrainingSessionId,
                                UserId = aid
                            })
                            .ToList();
                    }

                    if (model.SelectedTrainingStaffs != null && model.SelectedTrainingStaffs.Any())
                    {
                        session.TrainingStaffs = model.SelectedTrainingStaffs
                            .Select(sid => new TrainingStaff
                            {
                                TrainingSessionId = session.TrainingSessionId,
                                UserId = sid
                            })
                            .ToList();
                    }

                    await _context.SaveChangesAsync();
                }

                date = date.AddDays(1);
            }


            _context.TrainingSessionSeries.Update(series);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Antrenman serisi başarıyla güncellendi.";
            return RedirectToAction("Calendar");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> DeleteSeries(int seriesId)
        {
            var series = await _context.TrainingSessionSeries
                .Include(s => s.Days)
                .FirstOrDefaultAsync(s => s.SeriesId == seriesId);
            if (series == null)
            {
                return NotFound();
            }
            _context.TrainingSessionSeriesDays.RemoveRange(series.Days);
            var sessions = await _context.TrainingSessions
                .Where(t => t.SeriesId == seriesId)
                .ToListAsync();
            _context.TrainingSessions.RemoveRange(sessions);
            _context.TrainingSessionSeries.Remove(series);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Antrenman serisi başarıyla silindi.";
            return RedirectToAction("Calendar");
        }

        [HttpGet]
        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> Edit(int id)
        {
            var trainingSession = await _context.TrainingSessions
                .Include(t => t.CourseSession)
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(t => t.TrainingSessionId == id);
            if (trainingSession == null)
            {
                return NotFound();
            }
            var model = new TrainingSessionViewModel
            {
                TrainingSessionId = trainingSession.TrainingSessionId,
                CourseSessionId = trainingSession.CourseSessionId,
                UserId = trainingSession.UserId,
                Date = trainingSession.Date,
                StartTime = trainingSession.StartTime,
                EndTime = trainingSession.EndTime,
                Note = trainingSession.Note,
                CourseSessions = await _context.CourseSessions.ToListAsync(),
                Instructors = await _userManager.GetUsersInRoleAsync("Instructor"),
            };
            model.SelectedTrainingAssistants = await _context.TrainingAssistants
                .Where(ta => ta.TrainingSessionId == id)
                .Select(ta => ta.UserId)
                .ToListAsync();
            model.SelectedTrainingStaffs = await _context.TrainingStaffs
                .Where(ts => ts.TrainingSessionId == id)
                .Select(ts => ts.UserId)
                .ToListAsync();

            if (trainingSession.SeriesId != 0 && trainingSession.SeriesId != null)
            {
                ViewBag.SeriesId = trainingSession.SeriesId;
                return View("EditSeries", model);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> Edit(TrainingSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CourseSessions = await _context.CourseSessions.ToListAsync();
                model.Instructors = await _userManager.GetUsersInRoleAsync("Instructor");
                return View(model);
            }
            var trainingSession = await _context.TrainingSessions
                .Include(t => t.TrainingAssistants)
                .Include(t => t.TrainingStaffs)
                .FirstOrDefaultAsync(t => t.TrainingSessionId == model.TrainingSessionId);
            if (trainingSession == null)
            {
                return NotFound();
            }
            trainingSession.CourseSessionId = model.CourseSessionId;
            trainingSession.UserId = model.UserId;
            trainingSession.Date = model.Date.Value;
            trainingSession.StartTime = model.StartTime;
            trainingSession.EndTime = model.EndTime;
            trainingSession.Note = model.Note;
            _context.TrainingAssistants.RemoveRange(trainingSession.TrainingAssistants);
            _context.TrainingStaffs.RemoveRange(trainingSession.TrainingStaffs);
            foreach (var assistantId in model.SelectedTrainingAssistants)
            {
                var trainingAssistant = new TrainingAssistant
                {
                    TrainingSessionId = trainingSession.TrainingSessionId,
                    UserId = assistantId
                };
                _context.TrainingAssistants.Add(trainingAssistant);
            }
            foreach (var staffId in model.SelectedTrainingStaffs)
            {
                var trainingStaff = new TrainingStaff
                {
                    TrainingSessionId = trainingSession.TrainingSessionId,
                    UserId = staffId
                };
                _context.TrainingStaffs.Add(trainingStaff);
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Antrenman başarıyla güncellendi.";
            return RedirectToAction("Calendar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteTraining")]
        public async Task<IActionResult> Delete(int id)
        {
            var trainingSession = await _context.TrainingSessions
                .Include(t => t.TrainingAssistants)
                .Include(t => t.TrainingStaffs)
                .FirstOrDefaultAsync(t => t.TrainingSessionId == id);
            if (trainingSession == null)
            {
                return NotFound();
            }
            _context.TrainingAssistants.RemoveRange(trainingSession.TrainingAssistants);
            _context.TrainingStaffs.RemoveRange(trainingSession.TrainingStaffs);
            _context.TrainingSessions.Remove(trainingSession);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Antrenman başarıyla silindi.";
            return RedirectToAction("Calendar");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.TrainingSessions
                .Include(t => t.CourseSession)
                .Include(t => t.ApplicationUser)
                .Select(t => new
                {
                    id = t.TrainingSessionId,
                    title = $"{t.CourseSession.Title} - {t.ApplicationUser.Name} {t.ApplicationUser.Surname}",
                    start = t.Date.Date.Add(t.StartTime),
                    end = t.Date.Date.Add(t.EndTime),
                    extendedProps = new
                    {
                        description = t.Note,
                        date = t.Date.Date.ToString("yyyy-MM-dd"),
                        startTime = t.Date.Date.Add(t.StartTime).ToString(@"HH:mm"),
                        endTime = t.Date.Date.Add(t.EndTime).ToString(@"HH:mm"),
                    }
                })
                .ToListAsync();

            return Json(events);
        }

        public IActionResult Calendar()
        {
            return View();
        }

        [Authorize(Policy = "CanTakeTrainingAttendance")]
        public async Task<IActionResult> TakeAttendance(int trainingId)
        {
            var model = _context.TrainingAttendances
                .Include(x => x.ApplicationUser)
                .Where(x => x.TrainingSessionId == trainingId).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "CanTakeTrainingAttendance")]
        public async Task<IActionResult> TakeAttendance(List<TrainingAttendance> model)
        {
            foreach (var record in model)
            {
                var existingRecord = await _context.TrainingAttendances
                    .FirstOrDefaultAsync(x => x.TrainingSessionId == record.TrainingSessionId && x.UserId == record.UserId);
                if (existingRecord != null)
                {
                    existingRecord.IsPresent = record.IsPresent;
                    existingRecord.AttendanceDate = DateTime.Now;
                    _context.TrainingAttendances.Update(existingRecord);
                }
            }


            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Devamsızlık bilgileri başarıyla güncellendi.";
            return View("Calendar");
        }

    }

}
