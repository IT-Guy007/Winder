namespace DataModel;
using System.Data.SqlClient;
using System.Drawing;
public class Database {
    private SqlConnection connection;

    public void generateConnection() {
        
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "192.168.1.106,1433";
        builder.UserID = "sa";
        builder.Password ="Qwerty1@";
        builder.InitialCatalog = "winder";
        
        
       
        connection = new SqlConnection(builder.ConnectionString);
    }
    
    public void openConnection() {
        if (connection == null) {
            generateConnection();
        }
        connection.Open();
    }
    
    public void closeConnection() {
        
        if (connection == null) {
            generateConnection();
        }
        connection.Close();
    }

    public static User getUserFromDatabase(int UID) {
        return new User(0,"Padawan","Jeroen","den","Otter",DateTime.Now,"Female","s1165707@student.windesheim.nl","MysecretPassword1@","Male", new Bitmap(0,0));
    }
}