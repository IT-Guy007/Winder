using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace DataModel;

public class ChatModel {
    

    private List<ChatMessage> Messages;
    public User FromUser{ get; set; }
    public User ToUser{ get; set; }

    public static ChatModel? chat = new ChatModel();

    public ChatModel() 
    {
        Messages = new List<ChatMessage>();
    }

    public ChatModel(User fromUser, User toUser) {
        Messages = new List<ChatMessage>();
        FromUser = fromUser;
        ToUser = toUser;
    }

    public List<ChatMessage> GetChatMessages()
    {
        return Messages;
    }

    public void AddChatMessagesToList(ChatMessage message)
    {
        Messages.Add(message);
    }
    
}