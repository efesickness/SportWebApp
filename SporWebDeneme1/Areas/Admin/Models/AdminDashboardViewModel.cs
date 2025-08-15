using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Admin.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalCourses { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalStudents { get; set; }
        public int TotalRegistrations { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<StudentSubscription> StudentSubscriptions { get; set; } 
        public List<Payment> Payments { get; set; }
        public List<(string CourseTitle, int RegistrationCount)> TopCourses { get; set; }
        public List<Registration> RecentRegistrations { get; set; }
        public List<ApplicationUser> RecentRegistrants { get; set; }
        public List<ApplicationUser> RecentLogins { get; set; }
    }
}
