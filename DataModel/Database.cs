namespace DataModel;
using System.Data.SqlClient;
using System.Drawing;
public class Database {
    public SqlConnection connection;

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


    public List<string> GetEmailFromDataBase() {
        List<string> emails = new List<string>();
        openConnection();
        string sql = "USE winder;" +
                     "SELECT email FROM Winder.Winder.[User];";
        SqlCommand command = new SqlCommand(sql, connection);
        try
        {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var iets1 = reader["email"] as string;
                emails.Add(iets1);
            }
        }
        catch (SqlException e)
        {
            
        }
        closeConnection();
        return emails;
    }
}