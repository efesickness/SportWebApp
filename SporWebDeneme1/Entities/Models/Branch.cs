using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Entities.Models
{
    public class Branch
    {
        [Key]
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Navigation property for related teams
    }
}
