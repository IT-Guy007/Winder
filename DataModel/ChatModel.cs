using DataModel;
using System.Data;
using System.Data.SqlClient;

public class DatabaseChangeListener  {
    public static ChatCollection _chatCollection;
    private static User fromUser;
    private static User toUser;


    //SQL
    private static string query;
    private static string connectionString;
    private static SqlCommand sqlCommand;
    private static SqlConnection connection;
    private static SqlDependency SqlDependency;

    public static void Initialize(User _fromUser, User _toUser) {
        fromUser = _fromUser;
        toUser = _toUser;
        _chatCollection = new ChatCollection();

        GenerateConnection();
        GenerateQuery();
        GenerateCommand();
        
        SqlDependency.Start(connectionString);
        getDataWithSqlDependency();
    }

    //Generate the connection
    private static void GenerateConnection() {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "192.168.1.106,1433";
        builder.UserID = "sa";
        builder.Password = "Qwerty1@";
        builder.InitialCatalog = "winder";

        connectionString = builder.ConnectionString;
    }

    //Generate the query
    private static void GenerateQuery() {
        query = "SELECT * FROM Winder.winder.ChatMessage WHERE personFrom = " + fromUser.email +
            " AND personTo = " + toUser.email;
    }
    
    //Generate the command
    private static void GenerateCommand() {
        sqlCommand = new SqlCommand(query, connection);
    }


    //Listener
    private static DataTable getDataWithSqlDependency() {
        Console.WriteLine("Waiting for new Data");

        var dataTable = new DataTable();

        // Create dependency for this command and add event handler
        SqlDependency = new SqlDependency(sqlCommand);
        SqlDependency.OnChange += onDependencyChange;

        // execute command to get data
        connection.Open();
        dataTable.Load(sqlCommand.ExecuteReader(CommandBehavior.CloseConnection));

        return dataTable;

    }

    // Handler method
    private static void onDependencyChange(object sender, SqlNotificationEventArgs e) {

        Console.WriteLine("New chatmessage found by listener, Data:");
        Console.WriteLine($"Info={e.Info}, Source={e.Source}, Type={e.Type}.");

        if ((e.Info != SqlNotificationInfo.Invalid) && (e.Type != SqlNotificationType.Subscribe)){

            //Resubscribe
            var dataTable = getDataWithSqlDependency();

            Console.WriteLine($"Data changed. {dataTable.Rows.Count} rows returned.");
            
            //Convert to ChatMessage and add to list
            DataTableToObject(dataTable);
        } else {
            Console.WriteLine("SqlDependency not restarted");
        }

    }

    //Transfer data to collection as ChatMessage objects
    private static void DataTableToObject(DataTable dataTable) {
        foreach (DataRow row in dataTable.Rows) {
            string fromUser = row["personFrom"].ToString() ?? "";
            string toUser = row["personTo"].ToString() ?? "";
            DateTime date = DateTime.Parse(row["sendDate"].ToString()?? "") ;
            string message = row["chatMessage"].ToString() ?? "";
            bool read = row["read"] as bool? ?? false;
            if(fromUser != "" && toUser != "" && message != "") {
                _chatCollection.Add(new ChatMessage(fromUser, toUser, date, message, read));
            }
        }
    }

}