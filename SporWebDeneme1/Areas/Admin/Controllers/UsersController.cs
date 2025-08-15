using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
using SporWebDeneme1.Models;
using System.Reflection.Metadata.Ecma335;
using System.Web;

namespace SporWebDeneme1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "CanAccessUsers")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize(Policy = "CanAccessUsers")]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [Authorize(Policy = "CanAccessUsers")]
        [HttpGet]
        public IActionResult Index(string search)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(u => u.Email.Contains(search));

            return View(users.ToList());
        }

        [Authorize(Policy = "CanAddUser")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Districts = _context.Districts.ToList();
            return View();
        }

        [Authorize(Policy = "CanAddUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] RegisterViewModel model, string roleName)
        {
            if (!ModelState.IsValid)
            {
                TempData["FailMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
                return View(model);
            }
            string name = CapitalizeFirstLetter(model.Name.Trim());
            string lastname = CapitalizeFirstLetter(model.Lastname.Trim());
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Address = model.Address,
                BirthDate = model.BirthDate,
                CityId = model.CityId,
                DistrictId = model.DistrictId,
                PhoneNumber = model.PhoneNumber,
                TC_Number = model.TCNO,
                Name = name,
                Surname = lastname,
                LastLoginDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
                Gender = model.Gender,
                BloodType = model.BloodType + model.Rh,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, roleName);

                TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }


            TempData["FailMessage"] = "Kayıt işlemi başarısız. Lütfen tekrar deneyin.";
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetDistrictsByCityId(int cityId)
        {
            var districts = _context.Districts
                .Where(d => d.CityId == cityId)
                .Select(d => new { d.DistrictId, d.DistrictName })
                .ToList();

            return Json(districts);
        }

        [Authorize(Policy = "CanDeleteUser")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            if (user.UserName == User.Identity.Name)
            {
                TempData["FailMessage"] = "Kendi hesabınızı silemezsiniz.";
                return RedirectToAction("Index");
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "CanAccessUsers")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _context.ApplicationUsers
                .Include(x => x.City)
                .Include(x => x.District)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            ViewBag.AllRoles = allRoles;
            ViewBag.UserRoles = roles;

            return View(user);
        }

        [Authorize(Policy = "CanAssignRoleToUser")]
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (string.IsNullOrEmpty(role))
                return RedirectToAction("Details", new { id = userId });

            if (!await _userManager.IsInRoleAsync(user, role))
                await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction("Details", new { id = userId });
        }

        [Authorize(Policy = "CanAssignRoleToUser")]
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            if (user.UserName == User.Identity.Name && role == "Admin")
            {
                TempData["FailMessage"] = "Kendi hesabınızdan Admin rolünü kaldıramazsınız.";
                return RedirectToAction("Index");
            }

            if (await _userManager.IsInRoleAsync(user, role))
                await _userManager.RemoveFromRoleAsync(user, role);

            return RedirectToAction("Details", new { id = userId });
        }








        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
