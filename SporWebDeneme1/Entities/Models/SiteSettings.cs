using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Entities.Models
{
    public class SiteSettings
    {
        [Key]
        public int Id { get; set; }

        public bool RegistrationEnabled { get; set; } = true;

        public bool RequireEmailConfirmation { get; set; } = true;

        public int MaxCoursesPerUser { get; set; } = 3;

        public decimal DefaultCoursePrice { get; set; } = 0;

        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

}
