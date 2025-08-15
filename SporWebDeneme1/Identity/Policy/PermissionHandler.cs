using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Identity.Policy
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null) return;

            var roles = await _userManager.GetRolesAsync(user);

            var hasPermission = await _context.RolePermissions
                .Include(rp => rp.Permission)
                .AnyAsync(rp => roles.Contains(rp.IdentityRole.Name) && rp.Permission.Name == requirement.PermissionName);

            if (hasPermission)
                context.Succeed(requirement);
        }
    }
}
