namespace SporWebDeneme1.Areas.Course.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public int BranchId { get; set; }
        public string UserId{ get; set; } // Instructor's UserId
        public string Title { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
