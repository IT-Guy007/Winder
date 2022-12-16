
using DataModel;
using System.Data;
using System.Data.SqlClient;
using static System.Data.SqlClient.SqlDependency;

public class DatabaseChangeListener {
    
    public static List<string> ChatMessages;
    private User fromUser;
    private User toUser;

    //SQL
    private string query;
    private SqlConnection connection;
    private SqlCommand SqlCommand;
    private SqlDependency SqlDependency;

    public DatabaseChangeListener(User _fromUser, User _toUser) {
        ChatMessages = new List<string>();
        fromUser = _fromUser;
        toUser = _toUser;

        GenerateConnection();
        GenerateQuery();
        GenerateDependency();
    }

    //Generate the connection
    private void GenerateConnection() {

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "192.168.1.108,1433";
        builder.UserID = "sa";
        builder.Password = "Qwerty1@";
        builder.InitialCatalog = "winder";

        connection = new SqlConnection(builder.ConnectionString);
    }

    //Generate the query
    private void GenerateQuery() {
        this.query = "SELECT * FROM Winder.winder.ChatMessage WHERE personFrom = " + fromUser.email +
            " AND personTo = " + toUser.email;

        GenerateCommand();
    }

    //Generate the query
    private void GenerateCommand() {
        this.SqlCommand = new SqlCommand(query, connection);
    }

    //Generate the dependency
    private void GenerateDependency() {
        this.SqlDependency = new SqlDependency(SqlCommand);
        this.SqlDependency.OnChange += new OnChangeEventHandler(OnDependencyChange);
    }

    //Activate the dependency
    public void Start() {
        if( SqlDependency != null) {
            try {


            } catch(Exception e) {
                Console.WriteLine("Error getting the chatmessages from: " + fromUser.email + " And " + toUser.email);
                Console.WriteLine(e.ToString);
                Console.WriteLine(e.StackTrace);
            }
        } else {
            Console.WriteLine("No listener exists");
        }

    }

        // Handler method
    private void OnDependencyChange(object sender,SqlNotificationEventArgs e ) {
        SqlDependency dependency = sender as SqlDependency;

         // A notification will only work once, so remove the existing subscription to allow a new one to be added:
        dependency.OnChange -= OnDependencyChange;
    }


}