using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
using SporWebDeneme1.Models;
using System.Diagnostics;

namespace SporWebDeneme1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            if (User.IsInRole("Admin")) { return RedirectToAction("Index", "Home", new { Area = "Admin" }); }
            else if (User.IsInRole("Instructor")) { return RedirectToAction("Index", "Home", new { Area = "Instructor" }); }
            else if (User.IsInRole("Student")) { return RedirectToAction("Index", "Home", new { Area = "Student" }); }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature != null)
            {
                var ex = exceptionHandlerPathFeature.Error;
                var path = exceptionHandlerPathFeature.Path;

                _context.Logs.Add(new Log
                {
                    Date = DateTime.Now,
                    Level = "Error",
                    Message = ex.Message,
                    Exception = ex.ToString(),
                    Path = path
                });
                _context.SaveChanges();
            }

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

    }
}
