namespace DataModel;
using System.Data.SqlClient;
public class Database {
    private SqlConnection connection;

    public void generateConnection()
    {
        string connectionString = "Data Source=192.168.1.106,1433;Initial Catalog=winder;User ID=winderUser;Password=Qwerty1@";
        connection = new SqlConnection(connectionString);
    }
    
    public void openConnection() {
        connection.Open();
    }
    
    public void closeConnection() {
        connection.Close();
    }
    
    
}