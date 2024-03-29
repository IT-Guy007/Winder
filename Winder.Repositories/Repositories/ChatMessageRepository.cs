﻿using DataModel;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        public List<ChatMessage> GetChatMessages(string emailFrom, string emailTo)
        {
            List<ChatMessage> chatMessages = new List<ChatMessage>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    connection.Open();
                    string query =
                        "SELECT [winder].[ChatMessage].[personFrom], [winder].[ChatMessage].[personTo], [winder].[ChatMessage].[sendDate], [winder].[ChatMessage].[chatMessage], [winder].[ChatMessage].[readMessage] " +
                        "FROM [winder].[ChatMessage] " +
                        "WHERE " +
                            "([winder].[ChatMessage].[personFrom] = '" + emailFrom + "' " +
                            "AND [winder].[ChatMessage].[personTo] = '" + emailTo + "') " +
                            "OR " +
                            "([winder].[ChatMessage].[personFrom] = '" + emailTo + "' " +
                            "AND [winder].[ChatMessage].[personTo] = '" + emailFrom + "') " +
                        "ORDER BY sendDate";

                    //Create command
                    SqlCommand sqlCommand = new SqlCommand(query, connection);

                    //Create result table
                    var dataTable = new DataTable();

                    //Fill table
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    dataTable.Load(reader);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string fromUserData = row["personFrom"].ToString() ?? "";
                        string toUserData = row["personTo"].ToString() ?? "";
                        DateTime date = DateTime.Parse(row["sendDate"].ToString() ?? "");
                        string message = row["chatMessage"].ToString() ?? "";
                        int read = int.Parse(row["readMessage"].ToString() ?? "0");
                        if (fromUserData != "" && toUserData != "" && message != "")
                        {
                            chatMessages.Add(new ChatMessage(fromUserData, toUserData, date, message, read != 0));
                        }
                    }
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error retrieving chat messages from database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                }
            }
            return chatMessages;
        }

        public bool SendMessage(string message, string EmailFrom, string EmailTo)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    DateTime timeSend = DateTime.Now;
                    connection.Open();
                    SqlCommand command =
                        new SqlCommand(
                            "INSERT INTO winder.ChatMessage VALUES (@senderEmail, @receiverEmail, @sendDate, @message, @read)",
                            connection);
                    command.Parameters.AddWithValue("@senderEmail", EmailFrom);
                    command.Parameters.AddWithValue("@receiverEmail", EmailTo);
                    command.Parameters.AddWithValue("@message", message);
                    command.Parameters.AddWithValue("@sendDate", timeSend);
                    command.Parameters.AddWithValue("@read", 0);
                    command.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error sending message");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                    return false;
                }
            }
            return true;
        }

        public bool SetRead(string EmailFrom, string EmailTo)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    connection.Open();
                    SqlCommand query = new SqlCommand("UPDATE winder.winder.[ChatMessage] SET [readMessage] = 1 WHERE personTo = '" + EmailTo + "' AND personFrom = '" + EmailFrom + "'", connection);
                    query.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error updating read message");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                    return false;
                }
            }
            return true;
        }


    }
}