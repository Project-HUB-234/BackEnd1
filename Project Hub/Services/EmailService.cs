using Project_Hub.DTOs;
using System.Net;
using System.Net.Mail;

namespace Project_Hub.Services
{
    public class EmailService
    {
        public string SmtpServer = "smtp.gmail.com";
        public int Port = 587;
        public string UserName = "notification.projecthub@gmail.com";
        public string Password = "ovymuaaxmnogpopq";
        public EmailService()
        {

        }

        public async Task SendEmailAsync(EmailDTO email)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(UserName);
                    mail.To.Add(email.Receiver!);
                    mail.Subject = email.Title;
                    mail.Body = email.Body;
                    mail.IsBodyHtml = true;

                    using SmtpClient smtpServer = new(SmtpServer);
                    smtpServer.Port = 587;
                    smtpServer.Credentials = new NetworkCredential(UserName, Password);
                    smtpServer.EnableSsl = true;
                    await smtpServer.SendMailAsync(mail);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
