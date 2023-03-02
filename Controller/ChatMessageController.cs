using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;


namespace Controller
{
    public class ChatMessageController
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatMessageController(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public void SendMessage(string message, string senderEmail, string receiverEmail)
        {
            _chatMessageRepository.SendMessage(message, senderEmail, receiverEmail);
        }
    }
}
