using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporWebDeneme1.Email;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
using SporWebDeneme1.Models;
using System.Net;
using System.Web;

namespace SporWebDeneme1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }


        public IActionResult signIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> signIn(LoginViewModel model)
        {
            /*if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Geçersiz giriş denemesi.");
            return View(model);*/

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "E-posta doğrulanmamış.");
                    return View(model);
                }

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains(model.Role))
                {
                    ModelState.AddModelError(string.Empty, $"Bu hesap {model.Role} rolüne ait değil.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Giriş başarısız.");
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public JsonResult GetDistrictsByCityId(int cityId)
        {
            var districts = _context.Districts
                .Where(d => d.CityId == cityId)
                .Select(d => new { d.DistrictId, d.DistrictName })
                .ToList();

            return Json(districts);
        }

        public IActionResult signUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Districts = _context.Districts.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> signUp([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["FailMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
                return View(model);
            }
            string name = CapitalizeFirstLetter(model.Name.Trim());
            string lastname = CapitalizeFirstLetter(model.Lastname.Trim());
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Address = model.Address,
                BirthDate = model.BirthDate,
                CityId = model.CityId,
                DistrictId = model.DistrictId,
                PhoneNumber = model.PhoneNumber,
                TC_Number = model.TCNO,
                Name = name,
                Surname = lastname,
                LastLoginDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
                Gender = model.Gender,
                BloodType = model.BloodType + model.Rh
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = HttpUtility.UrlEncode(token)
                }, Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "E-posta Doğrulama",
                    $"Hesabınızı doğrulamak için <a href='{confirmationLink}'>buraya tıklayın</a>");

                return RedirectToAction("RegisterConfirmation1");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            TempData["FailMessage"] = "Kayıt işlemi başarısız. Lütfen tekrar deneyin.";
            return View(model);
        }

        public IActionResult RegisterConfirmation1()
        {
            return View("RegisterConfirmation1");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Geçersiz e-posta doğrulama bağlantısı.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Kullanıcı bulunamadı: {userId}");

            token = HttpUtility.UrlDecode(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return View("EmailConfirmed");

            return BadRequest("E-posta doğrulaması başarısız.");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // E-posta bulunamadı veya doğrulanmamış, hata mesajı gösterme
                ModelState.AddModelError("", "E-posta adresi bulunamadı veya doğrulanmamış.");
                return View(model);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new
            {
                email = user.Email,
                token = HttpUtility.UrlEncode(token)
            }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Şifre Sıfırlama",
                $"Şifrenizi sıfırlamak için <a href='{resetLink}'>buraya tıklayın</a>");
            return RedirectToAction("ForgotPasswordConfirmation");
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            token = HttpUtility.UrlDecode(token);
            if (token == null || email == null)
                return BadRequest("Geçersiz token");

            return View(new ResetPasswordViewModel { Token = token, Email = email });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            model.Token = HttpUtility.UrlDecode(model.Token);
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //çalışmayabilir tekrar kontrol et ve tamamla
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "E-posta adresi boş olamaz.");
                return View("ForgotPassword");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "E-posta adresi bulunamadı veya zaten doğrulanmış.");
                return View("ForgotPassword");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new
            {
                userId = user.Id,
                token = HttpUtility.UrlEncode(token)
            }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "E-posta Doğrulama",
                $"Hesabınızı doğrulamak için <a href='{confirmationLink}'>buraya tıklayın</a>");
            return RedirectToAction("RegisterConfirmation1");
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var model = new ProfileViewModel
            {
                Id = user.Id,
                FirstName = user.Name,
                LastName = user.Surname,
                Email = user.Email
            };

            ViewBag.changePassword = false;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || string.IsNullOrEmpty(model.Email)
                || string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.Id))
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            // Mevcut şifreyi doğrula
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isPasswordCorrect)
            {
                ModelState.AddModelError("CurrentPassword", "Şifreyi yanlış girdiniz.");
                TempData["Failed"] = "Şifre doğrulaması başarısız.";
                return View(model);
            }

            // E-posta veya ad soyad güncellemesi
            user.Name = model.FirstName;
            user.Surname = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);
                TempData["Failed"] = "Profil bilgileri güncellenemedi.";
                return View(model);
            }

            if (model.ConfirmNewPassword != model.NewPassword)
            {
                ModelState.AddModelError("ConfirmNewPassword", "Yeni şifreler eşleşmiyor.");
                TempData["Failed"] = "Yeni şifreler eşleşmiyor.";
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var error in passwordChangeResult.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }


            TempData["Success"] = "Profil bilgileri güncellendi.";
            return RedirectToAction("Profile");
        }

        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
