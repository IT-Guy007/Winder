using System.Drawing.Text;
using System.Linq.Expressions;

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

    public void updateLocalUserFromDatabase(int UID) {
        //Start connection
        openConnection();
        
        //Create query
        SqlCommand query =  new SqlCommand("select * from winder.winder.[User] where UID = @UID", connection);
        query.Parameters.AddWithValue("@UID", UID);
        
        //Execute query
        try {
            SqlDataReader reader = query.ExecuteReader();

            while (reader.Read()) {
                var tUID = int.Parse(reader["UID"] as string);
                var username = reader["username"] as string;
                var firstName = reader["firstName"] as string;
                var middleName = reader["middleName"] as string;
                var lastName = reader["lastName"] as string;
                var email = reader["email"] as string;
                var preferences = reader["preference"] as string;
                var birthday = DateTime.Parse(reader["birthday"] as string);
                var gender = reader["gender"] as string;
                var profilePicture = reader["profilePicture"] as string;
                var bio = reader["bio"] as string;

                Authentication._currentUser = new User(tUID, username, firstName, middleName, lastName, birthday,
                    preferences, email, "", gender, StrToByteArray(profilePicture),bio);
            }

        }
        catch (SqlException sql) {
            Console.WriteLine("Sql error: " + sql);
            throw;
        }
    }

    public bool checkLogin(string email, string password) {
        
        //Start connection
        openConnection();
        
        //Create query
        SqlCommand query = new SqlCommand("SELECT * FROM winder.winder.[User] WHERE Email = @Email AND Password = @Password", connection);
        query.Parameters.AddWithValue("@Email", email);
        query.Parameters.AddWithValue("@Password", password);
        
        //Execute query
        SqlDataReader reader = query.ExecuteReader();
        
        while (reader.Read()) {
            //If the query returns a result, the login is correct
            return true;
        }
        
        return false;
        
        //Close connection
        
    }
    
    static System.Drawing.Image StrToByteArray(string str)
    {
        // Convert Base64 String to byte[]
        byte[] imageBytes = Convert.FromBase64String(str);
        MemoryStream ms = new MemoryStream(imageBytes, 0,
            imageBytes.Length);

        // Convert byte[] to Image
        ms.Write(imageBytes, 0, imageBytes.Length);
        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        return image;
    }
}