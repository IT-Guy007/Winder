using System.Data.SqlClient;

namespace DataModel;

public class Database2 {
    
    //Constants
    private const string DataSourceConnection = "192.168.1.106,1433";
    private const string UserIdConnection = "sa";
    private const string PasswordConnection = "Qwerty1@";
    private const string InitialCatalogConnection = "winder";

    //The connection
    public static SqlConnection ReleaseConnection { get; private set; }

    public Database2() {
        GenerateConnection();
        ReleaseConnection.Open();
    }
    
    private static void GenerateConnection() {
        
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder {
            DataSource = DataSourceConnection,
            UserID = UserIdConnection,
            Password = PasswordConnection,
            InitialCatalog = InitialCatalogConnection
        };

        ReleaseConnection = new SqlConnection(builder.ConnectionString);
    }
    
    public static SqlDataReader ExecuteReader(string query) {
        try {
            SqlCommand command = new SqlCommand(query, ReleaseConnection);
            return command.ExecuteReader();
        } catch(SqlException e) {
            Console.WriteLine("Error in ExecuteReader:");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            SqlDataReader reader = null;
            return reader;
        }
    }
    
    public static void ExecuteUpdate(string query) {
        try {
            SqlCommand command = new SqlCommand(query, ReleaseConnection);
            command.ExecuteNonQuery();
        } catch (SqlException e) {
            Console.WriteLine("Error in Executing update:");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
    
}