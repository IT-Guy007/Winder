using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly IConfiguration _configuration;
        
        public ChatMessageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<string> GetMessages(string senderEmail, string receiverEmail)
        {
            throw new NotImplementedException();
        }

        public bool SendMessage(string message, string senderEmail, string receiverEmail)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO winder.ChatMessage (chatMessage, personTo, personFrom) VALUES (@message, @senderEmail, @receiverEmail)", connection);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@senderEmail", senderEmail);
                command.Parameters.AddWithValue("@receiverEmail", receiverEmail);
                command.ExecuteNonQuery();
                return true;
            }
        }

        public bool SetRead(string senderEmail, string receiverEmail)
        {
            return true;
        }
    }
}