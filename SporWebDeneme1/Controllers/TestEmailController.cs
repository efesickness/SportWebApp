using Microsoft.AspNetCore.Mvc;
using SporWebDeneme1.Email;

namespace SporWebDeneme1.Controllers
{
    public class TestEmailController : Controller
    {
        private readonly IEmailSender _emailSender;

        public TestEmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<IActionResult> SendTestEmail()
        {
            await _emailSender.SendEmailAsync(
                "iefeucar@hotmail.com",      // Buraya test için kendi adresini yaz
                "Test Mail",
                "<strong>Bu bir test mailidir.</strong> ASP.NET Core MVC projesinden gönderildi.");

            return Content("Mail gönderildi!");
        }
    }

}
