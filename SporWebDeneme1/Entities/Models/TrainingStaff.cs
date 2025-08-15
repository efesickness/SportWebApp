using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class TrainingStaff
    {
        [Key]
        public int TrainingStuffId { get; set; }
        [ForeignKey("TrainingSession")]
        public int TrainingSessionId { get; set; } 
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } // Staff's Id
        public virtual TrainingSession TrainingSession { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
