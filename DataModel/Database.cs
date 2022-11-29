namespace DataModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection.PortableExecutable;

public class Database
{

    public SqlConnection connection;


    public void generateConnection()
    {

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "192.168.1.106,1433";
        builder.UserID = "sa";
        builder.Password = "Qwerty1@";
        builder.InitialCatalog = "winder";

        connection = new SqlConnection(builder.ConnectionString);
    }

    public void openConnection()
    {
        if (connection == null)
        {
            generateConnection();
        }
        connection.Open();
    }

    public void closeConnection()
    {

        if (connection == null)
        {

            generateConnection();
        }
        connection.Close();
    }

    public void updateLocalUserFromDatabase(string email)
    {

        //Start connection
        openConnection();

        //Create query
        SqlCommand query = new SqlCommand("select * from winder.winder.[User] where email = @email", connection);
        query.Parameters.AddWithValue("@email", email);

        //Execute query
        try
        {
            SqlDataReader reader = query.ExecuteReader();

            while (reader.Read())
            {
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
                    preferences, email, "", gender, Base64StringToBitmap(profilePicture), bio);
            }

            //Close connection
            closeConnection();

        }
        catch (SqlException sql)
        {
            Console.WriteLine("Sql error: " + sql);

            //Close connection
            closeConnection();
        }

        //Close connection
        closeConnection();
    }

    public bool checkLogin(string email, string password)
    {

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

        while (reader.Read())
        {
            if (hashed == reader["password"] as string)
            {
                output = true;
            }
        }

        //Close connection
        closeConnection();
        return output;
    }



    public List<string> GetEmailFromDataBase()
    {
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

    public bool register(string firstname, string middlename, string lastname, string username, string email,
        string preference, DateTime birthday, string gender, string bio, string password, string proficePicture, bool active)
    {

        Authentication authentication = new Authentication();
        string hashedpassword = authentication.HashPassword(password);
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
        query.Parameters.AddWithValue("@password", hashedpassword);
        query.Parameters.AddWithValue("@gender", gender);
        query.Parameters.AddWithValue("@username", username);
        query.Parameters.AddWithValue("@bio", bio);
        query.Parameters.AddWithValue("@profilePicture", proficePicture);
        query.Parameters.AddWithValue("@active", active);

        //Execute query
        try
        {
            query.ExecuteReader();

            //Close connection
            closeConnection();
            return true;
        }
        catch (SqlException se)
        {

            //Close connection
            closeConnection();
            return false;
        }

    }

    public bool toggleActivation(string email, bool activate)
    {

        //Open connectionn
        openConnection();

        SqlCommand query = new SqlCommand("update winder.winder.[User] set active = @Active where email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);
        query.Parameters.AddWithValue("@Active", activate);

        //Execute query
        try
        {
            int rows = query.ExecuteNonQuery();

            //Close connection
            closeConnection();
            if (rows != 0)
            {
                return true;
            }
            return false;

        }
        catch (SqlException se)
        {

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

    public User GetUserFromDatabase(string email)
    {
        User user = null;
        openConnection();
        string sql = "SELECT * FROM Winder.Winder.[User] where email = @Email";
        SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
        try
        {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string? username = reader["email"] as string;
                var firstName = reader["firstname"] as string;
                var middleName = reader["middlename"] as string;
                var lastName = reader["lastname"] as string;
                var preferences = reader["preference"] as string;
                string? gender = reader["gender"] as string;
                DateTime? bday = getDateTimefromReader(reader);
                var bio = reader["bio"] as string;

                user = new User(username,firstName, middleName,lastName,bday, preferences, null,null, gender, null,bio);
            }
        }
        catch (SqlException e)
        {
            closeConnection();
        }
        closeConnection();
        return user;
    }

    private DateTime? getDateTimefromReader(SqlDataReader reader)
    {
        return reader.IsDBNull(3) ?
                           (DateTime?)new DateTime(1925, 01, 01, 0, 0, 0, 0) :
                           (DateTime?)reader.GetDateTime(3);
    }

    public void RegisterInterestInDatabase(string username, string interest)
    {
        openConnection();
        string sql = "INSERT INTO winder.winder.userHasInterest (UID, interest) VALUES(@Email, @Interest)";
        SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Email", username);
        command.Parameters.AddWithValue("@Interest", interest);
        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqlException e)
        {
            closeConnection();
        }
        closeConnection();
    }
}