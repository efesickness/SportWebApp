using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Areas.Instructor.Models
{
    public class RenewSubViewModel
    {
        public StudentSubscription StudentSubscription { get; set; }
        public Payment Payment { get; set; }
        public Entities.Models.Course Course { get; set; }
    }
}
