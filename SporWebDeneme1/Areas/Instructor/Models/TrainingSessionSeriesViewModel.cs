using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Instructor.Models
{
    public class TrainingSessionSeriesViewModel
    {
        public int SeriesId { get; set; }
        public int CourseSessionId { get; set; }
        public string UserId { get; set; } // Instructor's Id
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Note { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; internal set; }
        public List<int> SelectedDays { get; internal set; }
        public List<CourseSession> CourseSessions { get; internal set; }
        public IList<ApplicationUser> Instructors { get; internal set; }
        public List<string> SelectedTrainingAssistants { get; internal set; }
        public List<string> SelectedTrainingStaffs { get; internal set; }
    }
}
