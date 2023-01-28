using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace DataModel;

public class ChatModel {
    public ObservableCollection<ChatMessage> Messages { get; set; }
    public User FromUser { get; set; }
    public User ToUser { get; set; }
    
    public ChatModel(User fromUser, User toUser, SqlConnection connection) {
        FromUser = fromUser;
        ToUser = toUser;
        Messages = new ObservableCollection<ChatMessage>();
        GetChatMessages(connection);
        SetRead(connection);
    }
    
    /// <summary>
    /// Gets all chat messages from a conversation
    /// </summary>
    /// <param name="fromUser">The first person, the one that is loggedin</param>
    /// <param name="toUser">The second person, the other person</param>
    /// <param name="connection">The database connection</param>
    private void GetChatMessages(SqlConnection connection) {
        
        string query = "SELECT [winder].[ChatMessage].[personFrom], [winder].[ChatMessage].[personTo], [winder].[ChatMessage].[sendDate], [winder].[ChatMessage].[chatMessage], [winder].[ChatMessage].[readMessage] " +
                       "FROM [winder].[ChatMessage] " +
                       "WHERE ([winder].[ChatMessage].[personFrom] = '" + FromUser.Email + "' AND [winder].[ChatMessage].[personTo] = '" + ToUser.Email + "') " +
                       "OR ([winder].[ChatMessage].[personFrom] = '" + ToUser.Email + "' AND [winder].[ChatMessage].[personTo] = '" + FromUser.Email + "') order by sendDate";
        
        //Create command
        SqlCommand sqlCommand = new SqlCommand(query, connection);
        
        //Create result table
        var dataTable = new DataTable();
        
        //Fill table
        dataTable.Load(sqlCommand.ExecuteReader(CommandBehavior.CloseConnection));
        
        foreach (DataRow row in dataTable.Rows) {
            string fromUserData = row["personFrom"].ToString() ?? "";
            string toUserData = row["personTo"].ToString() ?? "";
            DateTime date = DateTime.Parse(row["sendDate"].ToString() ?? "");
            string message = row["chatMessage"].ToString() ?? "";
            int read = int.Parse(row["readMessage"].ToString() ?? "0");
            if (fromUserData != "" && toUserData != "" && message != "") {
                Messages.Add(new ChatMessage(fromUserData, toUserData, date, message, read != 0));
            }
        }
    }
    
    
    /// <summary>
    /// Sets all messages to read in a conversation
    /// </summary>
    /// <param name="personFrom">Person who sees the messages</param>
    /// <param name="personTo">Person who send the messages</param>
    /// <param name="connection">The database connection</param>
    private void SetRead(SqlConnection connection){
        try {
            SqlCommand query = new SqlCommand("UPDATE winder.winder.[ChatMessage] SET [readMessage] = 1 WHERE personTo = '" + FromUser.Email + "' AND personFrom = '" + ToUser.Email + "'", connection);
            query.ExecuteNonQuery();

        } catch (SqlException se) {
            Console.WriteLine("Error updating read message");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);

        }

    }

}