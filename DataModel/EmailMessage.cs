using System.Net;
using System.Net.Mail;

namespace DataModel;

public class EmailMessage {

    //Email credentials
    private const string winderEmail = "thewinderapp@gmail.com";
    private const string emailCredential = "xltbqbsyderpqsxp";
    private const string smtpClientGmail = "smtp.gmail.com";
    private const int portEmail = 587;
    
    private string Email {get; set;}
    private string Body {get; set;}
    private string Subject {get; set;}
    
    public EmailMessage(string email, string body, string subject) {
        Email = email;
        Body = body;
        Subject = subject;
    }

    
    
    //Sends the mail
    public void SendEmail() {
        //sets the client
        SmtpClient smtpClient = new SmtpClient(smtpClientGmail) {
            Port = portEmail,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(winderEmail, emailCredential),
            EnableSsl = true,
        };

        // Makes the email
        MailMessage mailMessage = new MailMessage {
            From = new MailAddress(winderEmail),
            Subject = Subject,
            Body = Body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(Email);
        // verstuurd de mail
        smtpClient.Send(mailMessage);
    }

}