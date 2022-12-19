using DataModel;
using System.Data;
using System.Data.SqlClient;

public class DatabaseChangeListener
{
    public static ChatCollection _chatCollection;
    private static User fromUser;
    private static User toUser;


    //SQL
    private static string query;
    private static string connectionString;
    private static SqlCommand sqlCommand;
    private static SqlConnection connection;

    public void Initialize(User _fromUser, User _toUser)
    {
        fromUser = _fromUser;
        toUser = _toUser;
        _chatCollection = new ChatCollection();

        GenerateConnection();
        GenerateQuery();
        GenerateCommand();

        //Start listening
        GetOldMessages();
        SqlDependency.Start(connectionString);
        GetDataWithSqlDependency();

    }

    //Generate the connection
    private static void GenerateConnection()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "192.168.1.106,1433";
        builder.UserID = "sa";
        builder.Password = "Qwerty1@";
        builder.InitialCatalog = "winder";

        connectionString = builder.ConnectionString;
        connection = new SqlConnection(connectionString);
    }

    //Generate the query
    private static void GenerateQuery() {
        query = "SELECT [winder].[ChatMessage].[personFrom], [winder].[ChatMessage].[personTo], [winder].[ChatMessage].[sendDate], [winder].[ChatMessage].[chatMessage], [winder].[ChatMessage].[read] " +
                  "FROM [winder].[ChatMessage] " +
                  "WHERE ([winder].[ChatMessage].[personFrom] = '" + fromUser.email + "' AND [winder].[ChatMessage].[personTo] = '" + toUser.email + "') " +
                  "OR ([winder].[ChatMessage].[personFrom] = '" + toUser.email + "' AND [winder].[ChatMessage].[personTo] = '" + fromUser.email + "')";
    }

    //Generate the command
    private static void GenerateCommand() {
        sqlCommand = new SqlCommand(query, connection);
    }


    //Listener
    private DataTable GetDataWithSqlDependency() {

        try  {
            Console.WriteLine("Waiting for new Data");

            //Create dependency for this command and add event handler
            SqlDependency dependency = new SqlDependency(sqlCommand);
            dependency.OnChange += OnDependencyChange;

            var dataTable = new DataTable();
            dataTable.Load(sqlCommand.ExecuteReader(CommandBehavior.CloseConnection));

            return dataTable;
        } catch (Exception e)
        {
            Console.WriteLine("Error starting listener: " + e.Message);
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
        }

        return new DataTable();

    }

    // Handler method
    private void OnDependencyChange(object sender, SqlNotificationEventArgs e){

        Console.WriteLine("New chatmessage found by listener, Data:");
        Console.WriteLine($"Info={e.Info}, Source={e.Source}, Type={e.Type}.");

        SqlDependency sqlDependency = (SqlDependency)sender;
        sqlDependency.OnChange -= OnDependencyChange;
        sqlCommand.Notification = null;
        sqlDependency = new SqlDependency(sqlCommand);
        sqlDependency.OnChange += OnDependencyChange;
        
        //Convert the data
        var dataTable = GetDataWithSqlDependency();
        DataTableToObject(dataTable);

    }

    //Transfer data to collection as ChatMessage objects
    private static void DataTableToObject(DataTable dataTable)
    {
        Console.WriteLine("Found new info, inserting rows: " + dataTable.Rows.Count);
        foreach (DataRow row in dataTable.Rows)
        {
            string fromUser = row["personFrom"].ToString() ?? "";
            string toUser = row["personTo"].ToString() ?? "";
            DateTime date = DateTime.Parse(row["sendDate"].ToString() ?? "");
            string message = row["chatMessage"].ToString() ?? "";
            bool read = row["read"] as bool? ?? false;
            if (fromUser != "" && toUser != "" && message != "")
            {
                _chatCollection.Add(new ChatMessage(fromUser, toUser, date, message, read));
            }
        }
    }

    private void GetOldMessages() {
        //Open connection
        connection.Open();
        
        //Create query
        string query = "SELECT [winder].[ChatMessage].[personFrom], [winder].[ChatMessage].[personTo], [winder].[ChatMessage].[sendDate], [winder].[ChatMessage].[chatMessage], [winder].[ChatMessage].[read] " +
                  "FROM [winder].[ChatMessage] " +
                  "WHERE ([winder].[ChatMessage].[personFrom] = '" + fromUser.email + "' AND [winder].[ChatMessage].[personTo] = '" + toUser.email + "') " +
                  "OR ([winder].[ChatMessage].[personFrom] = '" + toUser.email + "' AND [winder].[ChatMessage].[personTo] = '" + fromUser.email + "')";
        
        //Create command
        sqlCommand = new SqlCommand(query, connection);
        
        //Create result table
        var dataTable = new DataTable();
        
        //Fill table
        dataTable.Load(sqlCommand.ExecuteReader(CommandBehavior.CloseConnection));
        
        //Convert data
        DataTableToObject(dataTable);
        
        //Close connection
        connection.Close();
    }

}