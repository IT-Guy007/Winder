namespace DataModel;

public class ChatModel
{
    public List<ChatMessage> Messages { get; set; }
    public string EmailFrom { get; set; }
    public string EmailTo { get; set; }

    public ChatModel(string emailFrom, string emailTo)
    {
        EmailFrom = emailFrom;
        EmailTo = emailTo;
        Messages = new List<ChatMessage>();
    }
}
