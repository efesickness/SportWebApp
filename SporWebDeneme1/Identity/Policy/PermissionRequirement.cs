using Microsoft.AspNetCore.Authorization;

namespace SporWebDeneme1.Identity.Policy
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
