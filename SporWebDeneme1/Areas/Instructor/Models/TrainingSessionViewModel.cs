using SporWebDeneme1.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Areas.Instructor.Models
{
    public class TrainingSessionViewModel
    {
        [Required]
        public int TrainingSessionId { get; set; }
        [Required]
        public int CourseSessionId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public List<string> SelectedTrainingAssistants { get; set; } = new List<string>();
        [Required]
        public List<string> SelectedTrainingStaffs { get; set; } = new List<string>();
        public DateTime? Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        public string? Note { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<TrainingSessionSeriesDay>? TrainingSessionSeriesDay { get; set; }

        public List<CourseSession>? CourseSessions { get; set; }
        public IList<ApplicationUser>? Instructors { get; set; }
    }

}
