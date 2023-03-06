namespace DataModel;

public class EmailMessage
{

    public string Email { get; set; }
    public string Body { get; set; }
    public string Subject { get; set; }

    public EmailMessage(string email, string body, string subject)
    {
        Email = email;
        Body = body;
        Subject = subject;
    }

}