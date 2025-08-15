using SporWebDeneme1.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TrainingSessionSeries
{
    [Key]
    public int SeriesId { get; set; }
    [ForeignKey("CourseSession")]
    public int CourseSessionId { get; set; }
    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Note { get; set; }

    public virtual CourseSession CourseSession { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual ICollection<TrainingSessionSeriesDay> Days { get; set; } = new List<TrainingSessionSeriesDay>();
}
