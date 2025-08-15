namespace SporWebDeneme1.Areas.Admin.Models
{
    public class SiteSettingsViewModel
    {
        public bool RegistrationEnabled { get; set; }

        public bool RequireEmailConfirmation { get; set; }

        public int MaxCoursesPerUser { get; set; }

        public decimal DefaultCoursePrice { get; set; }
    }

}
