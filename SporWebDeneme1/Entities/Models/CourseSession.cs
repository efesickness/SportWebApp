using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class CourseSession
    {
        [Key]
        public int CourseSessionId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } // Instructor's UserId
        public string Title { get; set; }
        public string Description { get; set; }
        public int AvailableCapacity { get; set; }
        public bool IsActive { get; set; } = true;
        public bool isFull => (Course?.Capacity != null) && (Registrations?.Count ?? 0) >= Course.Capacity;
        public virtual Course Course { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    }
}
