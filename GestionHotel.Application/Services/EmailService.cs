using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GestionHotel.Application.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _from;

        public EmailService()
        {
            _smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("8d8760d305b476", "e771a6262d20d2"), 
                EnableSsl = true
            };

            _from = "no-reply@hotel-epsi.com"; 
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MailMessage(_from, to, subject, body);
            await _smtpClient.SendMailAsync(message);
        }
    }
}
