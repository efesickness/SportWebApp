using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SporWebDeneme1.Entities.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } // Instructor
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual Branch Branch { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<CourseSession> CourseSessions { get; set; }
    }
}
