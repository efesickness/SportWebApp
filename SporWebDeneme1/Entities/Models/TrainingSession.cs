using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class TrainingSession
    {
        [Key]
        public int TrainingSessionId { get; set; }

        [ForeignKey("CourseSession")]
        public int CourseSessionId { get; set; } 

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } // Instructor's Id
        [ForeignKey("TrainingSessionSeries")]
        public int? SeriesId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public string? Note { get; set; }

        public virtual CourseSession CourseSession { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<TrainingAssistant> TrainingAssistants { get; set; } = new List<TrainingAssistant>();
        public virtual ICollection<TrainingStaff> TrainingStaffs { get; set; } = new List<TrainingStaff>();
        public virtual TrainingSessionSeries? TrainingSessionSeries { get; set; }
    }
}
