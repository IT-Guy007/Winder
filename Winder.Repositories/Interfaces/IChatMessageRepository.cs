using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winder.Repositories.Interfaces
{
    public interface IChatMessageRepository
    {
        public bool SendMessage(string message, string emailTo, string emailFrom);
        public List<ChatMessage> GetChatMessages(string emailTo, string emailFrom);
        public bool SetRead(string emailTo, string emailFrom);
    }
}
