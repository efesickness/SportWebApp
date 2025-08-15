using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Areas.Admin.Models
{
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
