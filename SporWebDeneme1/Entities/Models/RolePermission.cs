using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class RolePermission
    {
        [ForeignKey("IdentityRole")]
        public string RoleId { get; set; }
        public virtual IdentityRole IdentityRole { get; set; }
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
