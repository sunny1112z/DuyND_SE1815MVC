using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DuyND_SE1815_Services.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            if (string.IsNullOrEmpty(smtpSettings["SenderEmail"]) || string.IsNullOrEmpty(smtpSettings["SenderPassword"]))
            {
                throw new Exception("SMTP settings are missing in appsettings.json");
            }

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("News System", smtpSettings["SenderEmail"]));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(smtpSettings["Server"], int.Parse(smtpSettings["Port"]), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(smtpSettings["SenderEmail"], smtpSettings["SenderPassword"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                Console.WriteLine(" Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Email send failed: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

    }
}
