
using DataModel;
using System.Data;
using System.Data.SqlClient;
using static System.Data.SqlClient.SqlDependency;

public static class DatabaseChangeListener {
    
    public static List<ChatMessage> ChatMessages;
    private static User fromUser;
    private static User toUser;

    //SQL
    private static string query;
    private static SqlConnection connection;
    private static SqlCommand SqlCommand;
    private static SqlDependency SqlDependency;

    public static void Initialize(User _fromUser, User _toUser)
    {
        ChatMessages = new List<ChatMessage>();
        fromUser = _fromUser;
        toUser = _toUser;

        GenerateConnection();
        GenerateQuery();
    }

    //Generate the connection
    private static void GenerateConnection()
    {

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "192.168.1.108,1433";
        builder.UserID = "sa";
        builder.Password = "Qwerty1@";
        builder.InitialCatalog = "winder";

        connection = new SqlConnection(builder.ConnectionString);
    }

    //Generate the query
    private static void GenerateQuery()
    {
        query = "SELECT * FROM Winder.winder.ChatMessage WHERE personFrom = " + fromUser.email +
            " AND personTo = " + toUser.email;

        GenerateCommand();
    }

    //Generate the command
    private static void GenerateCommand()
    {
        SqlCommand = new SqlCommand(query, connection);
    }


    static DataTable getDataWithSqlDependency() {

        var dataTable = new DataTable();

        // Create dependency for this command and add event handler
        SqlDependency = new SqlDependency(SqlCommand);
        SqlDependency.OnChange += new OnChangeEventHandler(onDependencyChange);

        // execute command to get data
        connection.Open();
        dataTable.Load(SqlCommand.ExecuteReader(CommandBehavior.CloseConnection));

        return dataTable;

    }

    // Handler method
    static void onDependencyChange(object sender, SqlNotificationEventArgs e) {

        Console.WriteLine($"OnChange Event fired. SqlNotificationEventArgs: Info={e.Info}, Source={e.Source}, Type={e.Type}.");

        if ((e.Info != SqlNotificationInfo.Invalid) && (e.Type != SqlNotificationType.Subscribe)){

            //Resubscribe
            var dataTable = getDataWithSqlDependency();

            Console.WriteLine($"Data changed. {dataTable.Rows.Count} rows returned.");
            //Do something with the dataTables
            DataTableToObject(dataTable);
        } else {
            Console.WriteLine("SqlDependency not restarted");
        }

    }

    private static void DataTableToObject(DataTable dataTable) {

    }

}