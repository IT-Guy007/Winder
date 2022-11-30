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
                var firstName = reader["firstName"] as string;
                var middleName = reader["middleName"] as string;
                var lastName = reader["lastName"] as string;
                var preferences = reader["preference"] as string;
                var birthday = (DateTime)reader["birthday"];
                var gender = reader["gender"] as string;
                //var profilePicture = reader["profilePicture"] as string;
                var bio = reader["bio"] as string;

                Authentication._currentUser = new User(firstName, middleName, lastName, birthday,
                    preferences, email, "", gender, bio);
                //Authentication._currentUser = new User(username, firstName, middleName, lastName, birthday,
                //    preferences, email, "", gender, Base64StringToBitmap(profilePicture),bio);
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
        
        Authentication authentication = new Authentication();
        string hashed = authentication.HashPassword(password);
        bool output = false;
        
        //Start connection
        openConnection();
        
        //Create query
        SqlCommand query = new SqlCommand("SELECT * FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);

        
        //Execute query
        SqlDataReader reader = query.ExecuteReader();

        while (reader.Read()) {
            if(hashed == reader["password"] as string) {
                output = true;
            }
        }

        //Close connection
        closeConnection();
        updateLocalUserFromDatabase(email);
        return output;
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

    public bool register(string firstname, string middlename, string lastname, string email,
        string preference, DateTime birthday, string gender, string bio, string password, string proficePicture, bool active, string locatie, string opleiding) {

        Authentication authentication = new Authentication();
        string hashedpassword = authentication.HashPassword(password);
        //Start connection
        openConnection();
        
        //Create query
        SqlCommand query = new SqlCommand("insert into winder.winder.[User] " +
                                                    "Values (@firstname, @middlename, @lastname, @birthday, @preference, @email, @password, @gender, convert(varbinary(max),@profilePicture), @bio,@active, @locatie, @opleiding)", connection);
        query.Parameters.AddWithValue("@firstname", firstname);
        query.Parameters.AddWithValue("@middlename", middlename);
        query.Parameters.AddWithValue("@lastname", lastname);
        query.Parameters.AddWithValue("@birthday", birthday);
        query.Parameters.AddWithValue("@preference", preference);
        query.Parameters.AddWithValue("@email", email);
        query.Parameters.AddWithValue("@password", hashedpassword);
        query.Parameters.AddWithValue("@gender", gender);
        query.Parameters.AddWithValue("@bio", bio);
        query.Parameters.AddWithValue("@profilePicture", proficePicture);
        query.Parameters.AddWithValue("@active", active);
        query.Parameters.AddWithValue("@locatie", locatie);
        query.Parameters.AddWithValue("@opleiding", opleiding);

        //Execute query
        try {
            query.ExecuteReader();
            
            //Close connection
            closeConnection();
            return true;
        }
        catch(SqlException se) {

            Console.WriteLine(se.ToString());
            //Close connection
            closeConnection();
            return false;
        }

    }

    public bool toggleActivation(string email, bool activate) {
        
        //Open connectionn
        openConnection();
        
        SqlCommand query = new SqlCommand("update winder.winder.[User] set active = @Active where email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);
        query.Parameters.AddWithValue("@Active", activate);
        
        //Execute query
        try {
            int rows = query.ExecuteNonQuery();
            
            //Close connection
            closeConnection();
            if (rows != 0) {
                return true;
            }
            return false;
            
        } catch(SqlException se) {
            
            //Close connection
            closeConnection();
            return false;
        }

    }
    
    public static Bitmap Base64StringToBitmap(string? base64String)
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
    public List<string> GetInterestsFromDataBase()
    {
        List<string> interests = new List<string>();
        openConnection();
        string sql = "SELECT * FROM Winder.Winder.[Interests];";
        SqlCommand command = new SqlCommand(sql, connection);
        try
        {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var iets1 = reader["name"] as string;
                interests.Add(iets1);
            }
        }
        catch (SqlException e)
        {
            closeConnection();
        }
        closeConnection();
        return interests;
    }
}