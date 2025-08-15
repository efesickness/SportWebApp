using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class District
    {
        [Key]
        public int DistrictId { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        [MaxLength(20)]
        public string DistrictName { get; set; }
        public City City { get; set; }
    }
}
