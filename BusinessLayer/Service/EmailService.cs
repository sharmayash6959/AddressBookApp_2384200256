using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using BusinessLayer.Interface;

namespace BusinessLayer.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("your-email@gmail.com", "your-password");
                smtp.EnableSsl = true;

                var mail = new MailMessage
                {
                    From = new MailAddress("sharmayash6959@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(toEmail);
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
