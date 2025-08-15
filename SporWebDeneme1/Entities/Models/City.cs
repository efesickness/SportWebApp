using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Entities.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        [MaxLength(20)]
        public string CityName { get; set; }
        public ICollection<District> Districts { get; set; } = new List<District>();
    }
}
