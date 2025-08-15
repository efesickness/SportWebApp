    namespace SporWebDeneme1.Areas.Course.Models
{
    public class DaySelectionViewModel
    {
        public DayOfWeek Day { get; set; }
        public bool IsSelected { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
