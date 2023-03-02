using System.Data.SqlClient;

namespace Controller;

public static  class Database {
    
    //Constants
    private const string DataSourceReleaseConnection = "192.168.1.106,1433";
    private const string DataSourceDebugConnection = "192.168.1.106,1434";
    private const string UserIdConnection = "sa";
    private const string PasswordConnection = "Qwerty1@";
    private const string InitialCatalogConnection = "winder";

    //The connection
    public static SqlConnection ReleaseConnection { get; private set; }
    public static SqlConnection DebugConnection { get; private set; }
    
    
    /// <summary>
    /// Creates the release connection
    /// </summary>
    public static void InitializeReleaseConnection() {
        GenerateReleaseConnection();
        ReleaseConnection.Open();
    }
    
    /// <summary>
    /// Creates the debug connection
    /// </summary>
    public static void InitializeDebugConnection() {
        GenerateDebugConnection();
        DebugConnection.Open();
    }


    /// <summary>
    /// Generates the releaseconnection
    /// </summary>
    private static void GenerateReleaseConnection() {
        
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder {
            DataSource = DataSourceReleaseConnection,
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
            DataSource = DataSourceDebugConnection,
            UserID = UserIdConnection,
            Password = PasswordConnection,
            InitialCatalog = InitialCatalogConnection
        };

        DebugConnection = new SqlConnection(builder.ConnectionString);
    }
    
}