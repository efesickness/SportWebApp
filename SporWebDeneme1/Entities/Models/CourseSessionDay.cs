using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class CourseSessionDay
    {
        public int Id { get; set; }

        [ForeignKey("CourseSession")]
        public int CourseSessionId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public virtual CourseSession CourseSession { get; set; }
    }
}
