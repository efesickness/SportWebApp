using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "CanAddAndDeletePermission")]
    public class PermissionsManageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PermissionsManageController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Permissions.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(permission);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return NotFound();
            return View(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Permission permission)
        {
            if (id != permission.PermissionId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(permission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(permission);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return NotFound();

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
