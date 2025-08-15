using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class TrainingAssistant
    {
        [Key]
        public int TrainingAssistantId { get; set; }
        [ForeignKey("TrainingSession")]
        public int TrainingSessionId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } // Assistant's Id
        public virtual TrainingSession TrainingSession { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
