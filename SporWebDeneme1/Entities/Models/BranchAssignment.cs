using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class BranchAssignment
    {
        [Key]
        public int BranchAssignmentId { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public virtual Branch Branch { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
