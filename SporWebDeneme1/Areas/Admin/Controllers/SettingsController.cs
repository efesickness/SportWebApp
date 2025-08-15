using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporWebDeneme1.Areas.Admin.Models;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
using SporWebDeneme1.Models;

namespace SporWebDeneme1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "CanAccessAndChangeSettings")]
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SettingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var settings = _context.SiteSettings.FirstOrDefault();
            if (settings == null)
            {
                settings = new SiteSettings();
                _context.SiteSettings.Add(settings);
                _context.SaveChanges();
            }

            var vm = new SiteSettingsViewModel
            {
                RegistrationEnabled = settings.RegistrationEnabled,
                RequireEmailConfirmation = settings.RequireEmailConfirmation,
                MaxCoursesPerUser = settings.MaxCoursesPerUser,
                DefaultCoursePrice = settings.DefaultCoursePrice
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(SiteSettingsViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Failed"] = "Ayarlar güncellenemedi. Lütfen gerekli alanları doldurun.";
                return View(vm);
            }

            var settings = _context.SiteSettings.FirstOrDefault();
            if (settings == null)
            {
                settings = new SiteSettings();
                _context.SiteSettings.Add(settings);
            }

            settings.RegistrationEnabled = vm.RegistrationEnabled;
            settings.RequireEmailConfirmation = vm.RequireEmailConfirmation;
            settings.MaxCoursesPerUser = vm.MaxCoursesPerUser;
            settings.DefaultCoursePrice = vm.DefaultCoursePrice;
            settings.LastUpdated = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Ayarlar başarıyla güncellendi!";
            return View(vm);
        }

        public IActionResult EmailEdit()
        {
            var config = _context.EmailConfigs.FirstOrDefault();
            if (config == null)
            {
                config = new EmailConfig();
            }
            return View(config);
        }

        [HttpPost]
        public IActionResult EmailEdit(EmailConfig model)
        {
            if (!ModelState.IsValid)
            {
                TempData["FailMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
                return View(model);
            }

            var existingConfig = _context.EmailConfigs.FirstOrDefault();

            if (existingConfig == null)
            {
                TempData["FailMessage"] = "E-posta ayarları bulunamadı.";
                _context.EmailConfigs.Add(model);
            }
            else
            {
                existingConfig.Host = model.Host;
                existingConfig.Port = model.Port;
                existingConfig.EnableSSL = model.EnableSSL;
                existingConfig.UserName = model.UserName;
                existingConfig.Password = model.Password;
                existingConfig.SenderName = model.SenderName;
                existingConfig.SenderEmail = model.SenderEmail;
                _context.EmailConfigs.Update(existingConfig);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "E-posta ayarları başarıyla güncellendi.";
            return RedirectToAction("EmailEdit");
        }

    }

}
