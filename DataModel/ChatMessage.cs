namespace DataModel;

public class ChatMessage {
    public string fromUser { get; set; }
    public string toUser { get; set; }
    public DateTime sendTime { get; set; }
    public string message { get; set; }
    public bool read { get; set; }
    
    public ChatMessage(string fromUser, string toUser, DateTime sendTime, string message, bool read) {
        this.fromUser = fromUser;
        this.toUser = toUser;
        this.sendTime = sendTime;
        this.message = message;
        this.read = read;
    }
}