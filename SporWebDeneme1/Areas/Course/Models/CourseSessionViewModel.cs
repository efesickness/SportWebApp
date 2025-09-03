using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Course.Models
{
    public class CourseSessionViewModel
    {
        public int CourseSessionId { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; } // Instructor's UserId
        public string Title { get; set; }
        public string Description { get; set; }
        public int AvailableCapacity { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFull => AvailableCapacity <= 0;
    }
}
