using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Student.Models
{
    public class CourseDetailsViewModel
    {
        public string branch { get; set; }
        public string courseName { get; set; }
        public string courseDescription { get; set; }
        public string instructor { get; set; }
        public DateTime lastPayment { get; set; }
        public DateTime nextPayment { get; set; }
        public string courseSession { get; set; }
        public decimal lastPaymentAmount { get; set; }
    }
}
