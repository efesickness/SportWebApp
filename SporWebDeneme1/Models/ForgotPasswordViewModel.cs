using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
