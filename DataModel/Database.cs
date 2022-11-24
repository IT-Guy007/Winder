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

    public void updateLocalUserFromDatabase(string email) {
        
        //Start connection
        openConnection();
        
        //Create query
        SqlCommand query =  new SqlCommand("select * from winder.winder.[User] where email = @email", connection);
        query.Parameters.AddWithValue("@email", email);
        
        //Execute query
        try {
            SqlDataReader reader = query.ExecuteReader();

            while (reader.Read()) {
                var username = reader["username"] as string;
                var firstName = reader["firstName"] as string;
                var middleName = reader["middleName"] as string;
                var lastName = reader["lastName"] as string;
                var preferences = reader["preference"] as string;
                var birthday = DateTime.Parse(reader["birthday"] as string);
                var gender = reader["gender"] as string;
                var profilePicture = reader["profilePicture"] as string;
                var bio = reader["bio"] as string;

                Authentication._currentUser = new User(username, firstName, middleName, lastName, birthday,
                    preferences, email, "", gender, Base64StringToBitmap(profilePicture),bio);
            }
        
            //Close connection
            closeConnection();

        }
        catch (SqlException sql) {
            Console.WriteLine("Sql error: " + sql);
            
            //Close connection
            closeConnection();
        }

        //Close connection
        closeConnection();
    }

    public bool checkLogin(string email, string password) {
        bool output = false;
        //Start connection
        openConnection();
        
        //Create query
        SqlCommand query = new SqlCommand("SELECT * FROM winder.winder.[User] WHERE Email = @Email AND Password = @Password", connection);
        query.Parameters.AddWithValue("@Email", email);
        query.Parameters.AddWithValue("@Password", password);
        
        //Execute query
        SqlDataReader reader = query.ExecuteReader();

        if (reader.HasRows) {
            output = true;
        }

        //Close connection
        closeConnection();
        return output;
    }

    public bool register(string firstname, string middlename, string lastname, string username, string email,
        string preference, DateTime birthday, string gender, string bio, string password, string proficePicture, bool active) {
        
        
        //Start connection
        openConnection();
        
        //Create query
        SqlCommand query = new SqlCommand("insert into winder.winder.[User] " +
                                                    "Values (@firstname, @middlename, @lastname, @birthday, @preference, @email, @password, @gender, convert(varbinary(max),@profilePicture), @username, @bio,@active)", connection);
        query.Parameters.AddWithValue("@firstname", firstname);
        query.Parameters.AddWithValue("@middlename", middlename);
        query.Parameters.AddWithValue("@lastname", lastname);
        query.Parameters.AddWithValue("@birthday", birthday);
        query.Parameters.AddWithValue("@preference", preference);
        query.Parameters.AddWithValue("@email", email);
        query.Parameters.AddWithValue("@password", password);
        query.Parameters.AddWithValue("@gender", gender);
        query.Parameters.AddWithValue("@username", username);
        query.Parameters.AddWithValue("@bio", bio);
        query.Parameters.AddWithValue("@profilePicture", proficePicture);
        query.Parameters.AddWithValue("@active", active);
        
        //Execute query
        try {
            SqlDataReader reader = query.ExecuteReader();
            
            //Close connection
            closeConnection();
            return true;
        }
        catch(SqlException se) {
            
            //Close connection
            closeConnection();
            return false;
        }

    }
    
    public static Bitmap Base64StringToBitmap(string base64String)
    {
        Bitmap bmpReturn = null;

  
        byte[] byteBuffer = Convert.FromBase64String(base64String);
        MemoryStream memoryStream = new MemoryStream(byteBuffer);

  
        memoryStream.Position = 0;

  
        bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);

  
        memoryStream.Close();
        memoryStream = null;
        byteBuffer = null;

  
        return bmpReturn;
    }
    
    
}