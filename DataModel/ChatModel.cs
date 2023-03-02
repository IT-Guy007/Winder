using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace DataModel;

public class ChatModel {

    List<ChatMessage> messages = new List<ChatMessage>();    
    public ChatModel(ChatMessage chatMessage) {
        messages.Add(chatMessage);
    }

}