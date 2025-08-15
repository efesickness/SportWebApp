using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Areas.Instructor.Models;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = "CanAccessSubscriptionsPanel")]
    public class SubscriptionTrackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionTrackController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subList = await _context.StudentSubscriptions
                .Include(r => r.Registration)
                .ThenInclude(c => c.Course)
                .OrderByDescending(s => s.IsActive)  // Önce aktif olanlar (IsActive = true) gelir.
                .ThenBy(s => s.EndDate)
                .ToListAsync();
            foreach (var item in subList)
            {
                item.User = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == item.UserId);
            }
            foreach (var item in subList)
            {
                item.Registration = await _context.Registrations.FirstOrDefaultAsync(r => r.RegistrationId == item.RegistrationId);
            }
            SubscriptionViewModel subscriptionViewModel = new SubscriptionViewModel
            {
                StudentSubscription = subList
            };
            return View(subscriptionViewModel);
        }

        [Authorize(Policy = "CanActivateDeactivateSubscription")]
        public IActionResult Activate(int id)
        {
            var sub = _context.StudentSubscriptions.Find(id);
            if (sub is not null)
            {
                sub.IsActive = true;
                _context.StudentSubscriptions.Update(sub);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanActivateDeactivateSubscription")]
        public IActionResult Deactivate(int id)
        {
            var sub = _context.StudentSubscriptions.Find(id);
            if (sub is not null)
            {
                sub.IsActive = false;
                _context.StudentSubscriptions.Update(sub);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanRenewSubscription")]
        public async Task<IActionResult> Renew(int id)
        {
            var sub = await _context.StudentSubscriptions
                .Include(r => r.Registration)
                .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(s => s.SubscriptionId == id);
            if (sub is not null)
            {
                sub.User = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == sub.UserId);
                sub.Registration = await _context.Registrations.FirstOrDefaultAsync(r => r.RegistrationId == sub.RegistrationId);
            }
            if (sub is null)
            {
                return NotFound();
            }
            RenewSubViewModel renewSubViewModel = new RenewSubViewModel
            {
                StudentSubscription = sub,
                Payment = new Payment(),
                Course = sub.Registration?.Course
            };
            return View(renewSubViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanRenewSubscription")]
        public async Task<IActionResult> Renew(RenewSubViewModel model)
        {
            var sub = await _context.StudentSubscriptions.FindAsync(model.StudentSubscription.SubscriptionId);
            if (sub is not null)
            {
                var payment = new Payment
                {
                    PaymentDate = DateTime.Now,
                    UserId = sub.UserId,
                    SubscriptionId = sub.SubscriptionId,
                    Amount = model.Payment.Amount,
                    Method = PaymentMethod.BankTransfer,
                    Status = PaymentStatus.Paid,
                    PaymentProvider = "Manual",
                    Currency = "TRY"
                };

                sub.EndDate = DateTime.Now.AddMonths(1); // 1 ay uzatma
                _context.StudentSubscriptions.Update(sub);
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanRemoveSubscription")]
        public async Task<IActionResult> Delete(int id)
        {
            var sub = await _context.StudentSubscriptions
                .Include(r => r.Registration)
                .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(s => s.SubscriptionId == id);
            if (sub is null)
            {
                return NotFound();
            }
            
            _context.StudentSubscriptions.Remove(sub);
            _context.Registrations.Remove(sub.Registration);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
