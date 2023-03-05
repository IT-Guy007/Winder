using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;
using EmailMessage = DataModel.EmailMessage;

namespace Controller
{
    public class ResetPasswordController
    {
        //Email credentials
        private const string winderEmail = "thewinderapp@gmail.com";
        private const string emailCredential = "xltbqbsyderpqsxp";
        private const string smtpClientGmail = "smtp.gmail.com";
        private const int portEmail = 587;

        //Sends the mail
        public void SendEmail(EmailMessage message)
        {
            //sets the client
            SmtpClient smtpClient = new SmtpClient(smtpClientGmail)
            {
                Port = portEmail,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(winderEmail, emailCredential),
                EnableSsl = true,
            };

            // Makes the email
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(winderEmail),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(message.Email);
            // verstuurd de mail
            smtpClient.Send(mailMessage);

        }
    }
}
