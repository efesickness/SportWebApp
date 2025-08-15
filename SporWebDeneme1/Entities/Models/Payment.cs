using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporWebDeneme1.Entities.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        [ForeignKey("StudentSubscription")]
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "TRY"; 
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public PaymentStatus Status{ get; set; }
        public PaymentMethod Method { get; set; }
        public string? TransactionId { get; set; }  // Comes from payment provider (e.g., Stripe, PayPal)
        public string? PaymentProvider { get; set; } // "Stripe", "PayPal", "Iyzico", "Manual"

        public StudentSubscription StudentSubscription { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }

    }
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }
    public enum PaymentMethod
    {
        CreditCard,
        BankTransfer,
        Cash,
        EFT,
        Stripe,
        MobilePayment,
        Iyzico,
        Other
    }
}
