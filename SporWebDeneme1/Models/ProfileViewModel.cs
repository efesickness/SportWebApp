using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Models
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mevcut şifrenizi girmeniz gerekiyor.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre (Tekrar)")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmNewPassword { get; set; }
    }

}
