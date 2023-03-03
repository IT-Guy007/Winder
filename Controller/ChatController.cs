using DataModel;
using Winder.Repositories.Interfaces;


namespace Controller
{
    public class ChatController
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatController(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public void SendMessage(string message, string senderEmail, string receiverEmail)
        {
            _chatMessageRepository.SendMessage(message, senderEmail, receiverEmail);
        }
        public List<ChatMessage> GetChatMessages(string emailTo, string emailFrom)
        {
            foreach(ChatMessage message in _chatMessageRepository.GetChatMessages(emailTo, emailFrom))
            {
                ChatModel.chat.AddChatMessagesToList(message);
            }
            
            return ChatModel.chat.GetChatMessages();
        }
        public void SetRead(string emailTo, string emailFrom) {
            _chatMessageRepository.SetRead(emailTo, emailFrom);
        }
    }
}
