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
    public static SqlConnection DebugConnection { get; private set; }

    public Database2() {
        GenerateReleaseConnection();
        ReleaseConnection.Open();
    }
    
    
    /// <summary>
    /// Generates the releaseconnection
    /// </summary>
    private static void GenerateReleaseConnection() {
        
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder {
            DataSource = DataSourceConnection,
            UserID = UserIdConnection,
            Password = PasswordConnection,
            InitialCatalog = InitialCatalogConnection
        };

        ReleaseConnection = new SqlConnection(builder.ConnectionString);
    }
    
    
    /// <summary>
    /// Generates the debugConnection
    /// </summary>
    private static void GenerateDebugConnection() {
        
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder {
            DataSource = DataSourceConnection,
            UserID = UserIdConnection,
            Password = PasswordConnection,
            InitialCatalog = InitialCatalogConnection
        };

        DebugConnection = new SqlConnection(builder.ConnectionString);
    }
    
    public List<string> GetInterestsFromDataBase(SqlConnection connection) {
        List<string> interests = new List<string>();

        string sql = "SELECT * FROM Winder.Winder.[Interests];";
        SqlCommand command = new SqlCommand(sql, connection);
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                var item = reader["name"] as string;
                interests.Add(item);
            }
        } catch (SqlException e) {
            Console.WriteLine("Error retrieving interests from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);

        }

        return interests;
    }
    

}