using FutureDocteur.API.DataBase.Repository.Contract;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace FutureDocteur.API.Utils
{
    public class EmailSender:IEmail
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }


        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var from = _config["EmailSettings:From"];
                var smtpServer = _config["EmailSettings:SmtpServer"];
                var port = _config["EmailSettings:Port"];
                var username = _config["EmailSettings:Username"];
                var password = _config["EmailSettings:Password"];

                if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(smtpServer) ||
                    string.IsNullOrEmpty(port) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new InvalidOperationException("Configuration email incomplète.");
                }

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(smtpServer, int.Parse(port), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(username, password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                // Tu peux logger ici
                Console.WriteLine($"Erreur d'envoi d'e-mail : {ex.Message}");
                return false;
            }
        }
    }
}
