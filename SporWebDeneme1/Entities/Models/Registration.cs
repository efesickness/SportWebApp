using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }
        [ForeignKey("CourseSession")]
        public int? CourseSessionId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public virtual CourseSession? CourseSession { get; set; }
        public virtual Course Course { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
