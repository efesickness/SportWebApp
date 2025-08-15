using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(11)")]
        public string TC_Number { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Surname { get; set; }
        [Column(TypeName = "nvarchar(1)")] // E or K
        public string Gender { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Address { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        //public string? ProfilePictureUrl { get; set; }

        [MaxLength(3)] // A+, A-, B+, B-, AB+, AB-, O+, O-
        public string BloodType { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual ICollection<CourseSession> CourseSessions { get; set; } = new List<CourseSession>(); // For Instructors
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();    // For Students
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        //public ApplicationUser()
        //{
        //    // Initialize collections to avoid null reference exceptions
        //    CourseSessions = new List<CourseSession>();
        //    Registrations = new List<Registration>();
        //    AuditLogs = new List<AuditLog>();
        //}
    }
}
