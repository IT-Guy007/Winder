using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winder.Repositories.Interfaces
{
    public interface IChatMessageRepository
    {
        public bool SendMessage(string message, string senderEmail, string receiverEmail);
    }
}
