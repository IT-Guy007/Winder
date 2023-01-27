

using System.Data.SqlClient;

namespace DataModel;

public class User {
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDay { get; set; }
    public string Preference { get; set; }
    public string Email { get; set; }
    private string Password { get; set; }
    public string Gender { get; set; }
    public byte[] ProfilePicture { get; set; }
    public string Bio { get; set; }
    public string School { get; set; }
    public string Major { get; set; }
    public string[] Interests { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    
        
    private const int MinAgePreference = 18;
    private const int MaxAgePreference = 99;
    private static DateTime MinDateTimeBirth = new DateTime(1925, 01, 01, 0, 0, 0, 0);
    

    public User(string firstName, string middleName, string lastName, DateTime birthDay,
        string preference, string email, string password, string gender, byte[] profilePicture, string bio, string school, string major, int minAge, int maxAge) {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        BirthDay = birthDay;
        Preference = preference;
        Email = email;
        Password = password;
        Gender = gender;
        ProfilePicture = profilePicture;
        Bio = bio;
        School = school;
        Major = major;
        MinAge = minAge;
        MaxAge = maxAge;

    }
    
    public User(){}
    
    
    /// <summary>
    /// Gets the user data from the database
    /// </summary>
    /// <param name="email">email of the user</param>
    /// <param name="connection">database connection of the user</param>
    /// <returns></returns>
    public User GetUserFromDatabase(string email, SqlConnection connection) {

        string query = "SELECT * FROM Winder.Winder.[User] where email = @Email";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);
        
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                Email = reader["email"] as string ?? string.Empty;
                FirstName = reader["firstname"] as string ?? string.Empty;
                MiddleName = reader["middlename"] as string ?? string.Empty;
                LastName = reader["lastname"] as string ?? string.Empty;
                Preference = reader["preference"] as string ?? string.Empty;
                Gender = reader["gender"] as string ?? string.Empty;
                BirthDay = reader["birthday"] as DateTime? ?? MinDateTimeBirth;
                Bio = reader["bio"] as string ?? string.Empty;
                School = reader["location"] as string ?? string.Empty;
                Major = reader["education"] as string ?? string.Empty;
                ProfilePicture = (byte[])(reader["profilePicture"]);
                var minAge = reader["min"] as int?;
                var maxAge = reader["max"] as int?;

                MinAge = minAge ?? MinAgePreference;
                MaxAge = maxAge ?? MaxAgePreference;
                
            }
            LoadInterestsFromDatabaseInForUser(connection);
        } catch (SqlException e) {
            Console.WriteLine("Error retrieving user from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);

        }

        return this;
    }
    
    /// <summary>
    /// Add's the interests of the user to the interests list
    /// </summary>
    /// <param name="connection"></param>
    private void LoadInterestsFromDatabaseInForUser(SqlConnection connection) {
        string query = "SELECT * FROM Winder.Winder.[userHasInterest] where UID = @Email;";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", Email);
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                var item = reader["interest"] as string;
                Interests.Append(item);
            }
        } catch (SqlException e) {
            Console.WriteLine("Error retrieving interests from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);

        }
        
    }
    
    /// <summary>
    /// Add's an picture to the database
    /// </summary>
    /// <param name="imageToUpload">The image</param>
    /// <param name="connection">The databaseconnection</param>
    public void InsertPictureInDatabase(byte[] imageToUpload, SqlConnection connection) {
        try {
            string query = "INSERT INTO winder.winder.Photos (winder.[user], winder.photo) VALUES(@Email, @profilepicture)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@profilepicture", imageToUpload);
            command.ExecuteNonQuery();
        }
        catch (SqlException se)
        {
            Console.WriteLine("Error inserting picture in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
    }
    
        
    /// <summary>
    /// Delete all the photos from the database of the given user
    /// </summary>
    /// <param name="connection">The database connection</param>
    public void DeleteAllPhotosFromDatabase(SqlConnection connection) {
        try {
            string query = "delete from winder.winder.Photos WHERE [user] = @Email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);
            command.ExecuteNonQuery();
        } catch (SqlException se) {
            Console.WriteLine("Error deleting pictures from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
    }
    
    /// <summary>
    /// setting the activation of the user
    /// </summary>
    /// <param name="activate">Bool ton set to active or not active</param>
    /// <param name="connection">The database connection</param>
    public void SetActivation(bool activate, SqlConnection connection) {

        SqlCommand query = new SqlCommand("update winder.winder.[User] set active = @Active where Email = @Email",
            connection);
        query.Parameters.AddWithValue("@Email", Email);
        query.Parameters.AddWithValue("@Active", activate);

        //Execute query
        try {
            query.ExecuteNonQuery();
        } catch (SqlException se) {
            Console.WriteLine("Error setting activation");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }

    }
    
    /// <summary>
    /// Update password for current user
    /// </summary>
    /// <param name="password">New password for the user in plain</param>
    /// <param name="connection">The database connection</param>
    public void UpdatePassword(string password, SqlConnection connection) {

        // checken of Email in de database staat
        if (new UserModel().EmailIsUnique(Email,connection) == false)  {

            // connectieopzetten en query maken

            string hashedPassword = new UserModel().HashPassword(password);

            SqlCommand query = new SqlCommand("update winder.winder.[User] set password = @password where Email = @Email", connection);
            query.Parameters.AddWithValue("@Email", Email);
            query.Parameters.AddWithValue("@password", hashedPassword);

            //Execute query
            try {
                query.ExecuteNonQuery();
                

            } catch (SqlException se) {
                Console.WriteLine("Error updating password");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);

            }
        }
    }
    
    /// <summary>
    /// Delete the user from database
    /// </summary>
    /// <param name="connection">The database connection</param>
    public void DeleteUser(SqlConnection connection) {

        if (new UserModel().EmailIsUnique(Email, connection)) {

            Email = Email.ToLower();

            //querys maken
            SqlCommand queryLikedPerson = new SqlCommand("delete from winder.winder.Liked where person = @Email", connection);
            queryLikedPerson.Parameters.AddWithValue("@Email", Email);
            SqlCommand queryLikedLikedPerson = new SqlCommand("delete from winder.winder.Liked where likedPerson = @Email", connection);
            queryLikedLikedPerson.Parameters.AddWithValue("@Email", Email);

            SqlCommand queryMatchPerson1 = new SqlCommand("delete from winder.winder.Match where person1 = @Email", connection);
            queryMatchPerson1.Parameters.AddWithValue("@Email", Email);
            SqlCommand queryMatchPerson2 = new SqlCommand("delete from winder.winder.Match where person2 = @Email", connection);
            queryMatchPerson2.Parameters.AddWithValue("@Email", Email);

            SqlCommand queryUserHasInterest = new SqlCommand("delete from winder.winder.userHasInterest where UID = @Email", connection);
            queryUserHasInterest.Parameters.AddWithValue("@Email", Email);

            SqlCommand queryUser = new SqlCommand("delete from winder.winder.[User] where Email = @Email", connection);
            queryUser.Parameters.AddWithValue("@Email", Email);

            //Execute querys
            try {
                queryLikedPerson.ExecuteNonQuery();
                queryLikedLikedPerson.ExecuteNonQuery();
                queryMatchPerson1.ExecuteNonQuery();
                queryMatchPerson2.ExecuteNonQuery();
                queryUserHasInterest.ExecuteNonQuery();

                queryUser.ExecuteNonQuery();

            } catch (SqlException se) {
                Console.WriteLine("Error deleting user");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);

            }
            
        }

    }
    
    public User CheckLogin(string email, string password, SqlConnection connection) {
        UserModel userModel = new UserModel();
        Console.WriteLine("Check login");
        string hashed = new UserModel().HashPassword(password);
        bool output = false;

        if (!email.EndsWith(userModel.EmailEndsWith)) {
            email = email + userModel.EmailEndsWith;

        }
        if (!email.StartsWith(userModel.EmailStartsWith)) {
            email = userModel.EmailStartsWith + email;
        }

        //Create query
        SqlCommand query = new SqlCommand("SELECT * FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);

        //Execute query
        SqlDataReader reader = query.ExecuteReader();

        Console.WriteLine("Checking login");
        while (reader.Read()) {
            if (hashed == reader["password"] as string) {
                Console.WriteLine("Getting password");
                try {
                    userModel.SetLoginEmail(email);
            
                } catch {
                    Console.WriteLine("Error setting login Email");
                }
                
            }
        }
        
        return this;

    }

}

