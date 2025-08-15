using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = "CanAccessBranches")]
    public class ManageBranchesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageBranchesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _context.Branches.ToListAsync();
            var branchAssignments = await _context.BranchAssignments
                .Include(ba => ba.Branch)
                .Include(ba => ba.ApplicationUser)
                .ToListAsync();
            ViewBag.Courses = await _context.Courses.ToListAsync();
            ViewBag.Branches = branches;
            ViewBag.BranchAssignments = branchAssignments;
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanAddEditAndDeleteBranchAssignments")]
        public async Task<IActionResult> AssignBranchToInstructor()
        {
            var branches = await _context.Branches.ToListAsync();
            var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            var courses = await _context.Courses.FirstOrDefaultAsync();

            instructors = instructors
                .Where(i => !_context.BranchAssignments.Any(b => b.UserId == i.Id))
                .ToList();
            ViewBag.Branches = branches;
            ViewBag.Instructors = instructors;
            ViewBag.Courses = courses;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteBranchAssignments")]
        public async Task<IActionResult> AssignBranchToInstructor(int branchId, string instructorId)
        {
            if (branchId == 0 || string.IsNullOrEmpty(instructorId))
            {
                return BadRequest("Branch ID and Instructor ID cannot be null or empty.");
            }
            var branchAssignment = new Entities.Models.BranchAssignment
            {
                BranchId = branchId,
                UserId = instructorId,
                AssignedAt = DateTime.Now
            };
            _context.BranchAssignments.Add(branchAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteBranchAssignments")]
        public async Task<IActionResult> RemoveBranchAssignment(int assignmentId)
        {
            var assignment = await _context.BranchAssignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound("Branch assignment not found.");
            }
            _context.BranchAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Policy = "CanAddEditAndDeleteBranchAssignments")]
        public async Task<IActionResult> UpdateBranchAssignment(int assignmentId)
        {
            if (assignmentId == 0)
            {
                return BadRequest("Assignment ID cannot be null or empty.");
            }
            var assignment = await _context.BranchAssignments
                .Include(ba => ba.Branch)
                .Include(ba => ba.ApplicationUser)
                .FirstOrDefaultAsync(ba => ba.BranchAssignmentId == assignmentId);
            if (assignment == null)
            {
                return NotFound("Branch assignment not found.");
            }
            ViewBag.Branches = await _context.Branches.ToListAsync();
            ViewBag.Instructors = await _userManager.GetUsersInRoleAsync("Instructor");

            return View(assignment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteBranchAssignments")]
        public async Task<IActionResult> UpdateBranchAssignment(Entities.Models.BranchAssignment branchAssignmentModel)
        {
            if (branchAssignmentModel.BranchAssignmentId == 0 || string.IsNullOrEmpty(branchAssignmentModel.UserId) || branchAssignmentModel.BranchId == 0)
            {
                TempData["ErrorMessage"] = "Branş ataması güncellenemedi, boş bıraktığınız alanlar var.";
                return RedirectToAction(nameof(Index));
            }
            var assignment = await _context.BranchAssignments.FindAsync(branchAssignmentModel.BranchAssignmentId);
            if (assignment == null)
            {
                TempData["ErrorMessage"] = "Branş ataması güncellenemedi, boş bıraktığınız alanlar var.";
                return RedirectToAction(nameof(Index));
            }
            assignment.BranchId = branchAssignmentModel.BranchId;
            assignment.UserId = branchAssignmentModel.UserId;
            _context.BranchAssignments.Update(assignment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Branş ataması başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Policy = "CanAddEditAndDeleteBranches")]
        public async Task<IActionResult> CreateBranch()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteBranches")]
        public async Task<IActionResult> CreateBranch(string branchName, string description)
        {
            if (string.IsNullOrEmpty(branchName))
            {
                return BadRequest("Branch name cannot be empty.");
            }
            var branch = new Branch
            {
                Name = branchName,
                Description = description,
                CreatedAt = DateTime.Now
            };
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteBranches")]
        public async Task<IActionResult> DeleteBranch(int branchId)
        {
            if (branchId == 0)
            {
                return BadRequest("Branch ID cannot be null or empty.");
            }
            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null)
            {
                return NotFound("Branch not found.");
            }
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Policy = "CanAddEditAndDeleteBranches")]
        public async Task<IActionResult> UpdateBranch(int branchId)
        {
            if (branchId == 0)
            {
                return BadRequest("Branch ID cannot be null or empty.");
            }
            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null)
            {
                return NotFound("Branch not found.");
            }
            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanAddEditAndDeleteBranches")]
        public async Task<IActionResult> UpdateBranch(Branch branchModel)
        {
            if (branchModel.BranchId == 0 || string.IsNullOrEmpty(branchModel.Name) || string.IsNullOrEmpty(branchModel.Description))
            {
                TempData["ErrorMessage"] = "Branş güncellenemedi, boş bıraktığınız alanlar var.";
                return RedirectToAction(nameof(Index));
            }
            var branch = await _context.Branches.FindAsync(branchModel.BranchId);
            if (branch == null)
            {
                TempData["ErrorMessage"] = "Branş güncellenemedi, boş bıraktığınız alanlar var.";
                return RedirectToAction(nameof(Index));
            }
            branch.Name = branchModel.Name;
            branch.Description = branchModel.Description;
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Branş başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Policy = "CanAccessBranchAssignments")]
        public async Task<IActionResult> BranchAssignments()
        {
            var branchAssignments = await _context.BranchAssignments
                .Include(ba => ba.Branch)
                .Include(ba => ba.ApplicationUser)
                .ToListAsync();
            ViewBag.BranchAssignments = branchAssignments;
            return View();
        }
    }
}
