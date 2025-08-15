using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Course.Models
{
    public class AddStudentViewModel
    {
        public Payment Payment { get; set; }
        public Registration Registration { get; set; }

        public IList<ApplicationUser> Students { get; set; }
        public CourseSession CourseSession { get; set; }

        public string SelectedUserId { get; set; }
        public int SelectedCourseSessionId { get; set; }
    }
}
