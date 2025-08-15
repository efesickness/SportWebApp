using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class TrainingAttendance
    {
        public int Id { get; set; }

        [ForeignKey("TrainingSession")]
        public int TrainingSessionId { get; set; }
        public TrainingSession TrainingSession { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsPresent { get; set; } 
        public DateTime AttendanceDate { get; set; } 
    }

}
