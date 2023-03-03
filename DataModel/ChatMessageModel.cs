using System.Data.SqlClient;

namespace DataModel;

public class ChatMessage {
    public string FromUser { get; set; }
    public string ToUser { get; set; }
    public DateTime SendTime { get; set; }
    public string Message { get; set; }
    public bool Read { get; set; }
    
    public ChatMessage(string fromUser, string toUser, DateTime sendTime, string message, bool read) {
        FromUser = fromUser;
        ToUser = toUser;
        SendTime = sendTime;
        Message = message;
        Read = read;
    }
    

}