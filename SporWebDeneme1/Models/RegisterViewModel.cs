using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Eposta boş bırakılamaz.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Tekrar şifre boş bırakılamaz.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "İsim boş bırakılamaz.")]
        [MinLength(3, ErrorMessage = "İsim en az 3 karakter olmalıdır.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyisim boş bırakılamaz.")]
        [MinLength(3, ErrorMessage = "Soyisim en az 3 karakter olmalıdır.")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [RegularExpression(@"^05\d{9}$", ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Doğum tarihi boş bırakılamaz.")]
        public DateOnly BirthDate { get; set; }
        [Required(ErrorMessage = "Cinsiyet boş bırakılamaz.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Şehir boş bırakılamaz.")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "İlçe boş bırakılamaz.")]
        public int DistrictId { get; set; }
        [Required(ErrorMessage = "Adres boş bırakılamaz.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "TC Kimlik Numarası boş bırakılamaz.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik Numarası 11 haneli olmalıdır.")]
        public string TCNO { get; set; }
        [Required(ErrorMessage = "Kan grubu boş bırakılamaz.")]
        public string BloodType { get; set; }
        [Required(ErrorMessage = "Rh boş bırakılamaz.")]
        public string Rh { get; set; }
    }

}
