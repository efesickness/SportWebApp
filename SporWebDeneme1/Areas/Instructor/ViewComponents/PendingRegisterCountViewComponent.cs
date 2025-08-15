using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities;

namespace SporWebDeneme1.Areas.Instructor.ViewComponents
{
    [Area("Instructor")]
    public class PendingRegisterCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public PendingRegisterCountViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pendingCount = await _context.Registrations
                .CountAsync(r => !r.IsApproved && !r.IsDeleted);
            return View(pendingCount);
        }
    }
}
