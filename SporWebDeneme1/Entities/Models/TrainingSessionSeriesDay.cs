using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class TrainingSessionSeriesDay
    {
        [Key]
        public int SeriesDayId { get; set; }
        [ForeignKey("TrainingSessionSeries")]
        public int TrainingSessionSeriesId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public virtual TrainingSessionSeries TrainingSessionSeries { get; set; }
    }
}
