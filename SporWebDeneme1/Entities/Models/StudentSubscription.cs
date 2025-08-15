using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class StudentSubscription
    {
        [Key]
        public int SubscriptionId { get; set; }

        // ForeignKey sadece UserId için belirtiliyor
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [ForeignKey("Registration")]
        public int RegistrationId { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual Registration Registration { get; set; }

        // Diğer alanlar
        public DateTime StartDate { get; set; }  // Ödeme başladığı tarih
        public DateTime EndDate { get; set; }    // Erişim hakkının bittiği tarih
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

    }
}
