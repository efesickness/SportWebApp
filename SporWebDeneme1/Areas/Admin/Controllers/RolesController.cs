using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "CanAccessRole")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize(Policy = "CanAccessRole")]
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        [Authorize(Policy = "CanAddAndDeleteRole")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Permissions = await _context.Permissions.ToListAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddAndDeleteRole")]
        public async Task<IActionResult> Create(string roleName, int[] selectedPermissions)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                TempData["ErrorMessage"] = "Rol adı boş olamaz.";
                ViewBag.Permissions = await _context.Permissions.ToListAsync();
                return View();
            }

            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                foreach (var permId in selectedPermissions)
                {
                    _context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permId
                    });
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rol başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.Permissions = await _context.Permissions.ToListAsync();
            return View();
        }

        [Authorize(Policy = "CanAddAndDeleteRole")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var allPermissions = await _context.Permissions.ToListAsync();

            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            ViewBag.Permissions = allPermissions;
            ViewBag.SelectedPermissions = rolePermissions;

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddAndDeleteRole")]
        public async Task<IActionResult> Edit(string id, string roleName, int[] selectedPermissions)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                // Eski yetkileri sil
                var existingRolePermissions = _context.RolePermissions.Where(rp => rp.RoleId == role.Id);
                _context.RolePermissions.RemoveRange(existingRolePermissions);

                // Yeni yetkileri ekle
                foreach (var permId in selectedPermissions)
                {
                    _context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permId
                    });
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rol başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Rol güncellenirken bir hata oluştu.";

            ViewBag.Permissions = await _context.Permissions.ToListAsync();
            ViewBag.SelectedPermissions = selectedPermissions;
            return View(role);
        }


        [Authorize(Policy = "CanAddAndDeleteRole")]
        public async Task<IActionResult> Delete(string id)
        {


            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            if (role.Name == "Admin" || role.Name == "Instructor" || role.Name == "Student" || role.Name == "Developer")
            {
                TempData["ErrorMessage"] = "Bu rolü silemezsiniz.";
                return View("Index", _roleManager.Roles.ToList());
            }
            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .ToListAsync();
            _context.RolePermissions.RemoveRange(rolePermissions);
            await _context.SaveChangesAsync();
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Rol başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Rol silinirken bir hata oluştu.";
            return View("Index", _roleManager.Roles.ToList());
        }
    }

}
