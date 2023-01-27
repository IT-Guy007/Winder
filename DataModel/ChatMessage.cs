using System.Data;
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
    
    /// <summary>
    /// Send chat message to database
    /// </summary>
    /// <param name="personFrom">Person who send the message</param>
    /// <param name="personTo">Person who receives the message</param>
    /// <param name="message">The message</param>
    /// <param name="connection">The database connection</param>
    public void SendMessage(SqlConnection connection) {
        try {
            SqlCommand query = new SqlCommand("INSERT INTO winder.winder.[ChatMessage] (personFrom, personTo,chatMessage,sendDate,[readMessage]) VALUES ('" + FromUser + "' , '" + ToUser + "','" + Message + "', @sendDate, 0)", connection);
            query.Parameters.AddWithValue("@sendDate", SendTime);
            query.ExecuteNonQuery();

        } catch (SqlException se) {
            Console.WriteLine("Error sending the message");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
    }

}