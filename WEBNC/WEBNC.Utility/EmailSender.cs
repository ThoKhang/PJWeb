using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace WEBNC.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var senderEmail = _config["EmailSettings:SenderEmail"];
                var password = _config["EmailSettings:Password"];
                var host = _config["EmailSettings:Host"];
                var port = int.Parse(_config["EmailSettings:Port"]);

                Console.WriteLine("SMTP DEBUG:");
                Console.WriteLine($"SenderEmail = {senderEmail}");
                Console.WriteLine($"Host = {host}, Port = {port}");

                var smtpClient = new SmtpClient(host, port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(senderEmail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, _config["EmailSettings:SenderName"]),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);

                Console.WriteLine("✅ SEND MAIL SUCCESS");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ SEND MAIL FAILED");
                Console.WriteLine(ex.ToString());
                throw; // QUAN TRỌNG
            }
        }

    }
}
