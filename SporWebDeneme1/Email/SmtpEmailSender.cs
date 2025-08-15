using Microsoft.Extensions.Options;
using SporWebDeneme1.Entities;
using System.Net;
using System.Net.Mail;

namespace SporWebDeneme1.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly ApplicationDbContext _context;

        public SmtpEmailSender(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var settings = _context.EmailConfigs.FirstOrDefault();

            if (settings == null)
                throw new Exception("Email ayarları bulunamadı.");

            var client = new SmtpClient(settings.Host, settings.Port)
            {
                Credentials = new NetworkCredential(settings.UserName, settings.Password),
                EnableSsl = settings.EnableSSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(settings.SenderEmail, settings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            await client.SendMailAsync(mailMessage);
        }
    }


}
