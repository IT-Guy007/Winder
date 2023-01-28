

using System.Data.SqlClient;
using Microsoft.Maui.Storage;

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
    private const int MaxAmountOfPictures = 6;
    private static DateTime MinDateTimeBirth = new DateTime(1925, 01, 01, 0, 0, 0, 0);

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
        
        SecureStorage.Default.Remove("Email");
        SecureStorage.Remove("Email");
        SecureStorage.RemoveAll();

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

    public List<User> GetMatchedStudentsFromUser(SqlConnection connection) {
        List<User> users = new List<User>();
        List<string> emails = new List<string>();
        try {
            string query = "SELECT person1, person2 FROM Winder.Winder.Match WHERE person1 = @Email OR person2 = @Email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string person1 = reader["person1"] as string ?? "Unknown";
                string person2 = reader["person2"] as string ?? "Unknown";
                if (person1 == Email) {
                    emails.Add(person2);
                } else {
                    emails.Add(person1);
                }
            }
            emails.ForEach(x => users.Add(new User().GetUserFromDatabase(x, connection)));
        } catch (SqlException se) {
            Console.WriteLine("Error retrieving matches from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
        return users;
    }

    /// <summary>
    /// Sets the minimum preferred age in the database and local
    /// </summary>
    /// <param name="minAge">The preferred age</param>
    /// <param name="connection">The database connection</param>
    public void SetMinAge(int minAge, SqlConnection connection) {
        if (minAge > MinAgePreference && minAge < MaxAgePreference) {

            SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET min = @minAge WHERE Email = @Email", connection);
            query.Parameters.AddWithValue("@Email", Email);
            query.Parameters.AddWithValue("@minAge", minAge);

            try {
                query.ExecuteReader();

            } catch (SqlException se) {
                Console.WriteLine("Error inserting minAge in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);

            }
        }
    }

    /// <summary>
      /// Sets the max age in the database and local
      /// </summary>
      /// <param name="maxAge">The max age in integer</param>
      /// <param name="connection">The database connection</param>
    public void SetMaxAge(int maxAge, SqlConnection connection) {
          if (maxAge > MinAgePreference && maxAge < MaxAgePreference) {

              SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET max = @maxAge WHERE Email = @Email",
                  connection);
              query.Parameters.AddWithValue("@Email", Email);
              query.Parameters.AddWithValue("@maxAge", maxAge);

              try
              {
                  query.ExecuteReader();
                  MaxAge = maxAge;
              }
              catch (SqlException se)
              {
                  Console.WriteLine("Error inserting maxAge in database");
                  Console.WriteLine(se.ToString());
                  Console.WriteLine(se.StackTrace);
              }
          }
    }

    /// <summary>
    /// Gets the max age from the database and sets it local and returns it
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public int GetMinAge(SqlConnection connection) {

        SqlCommand query = new SqlCommand("SELECT min FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", Email);


        try {
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read()) {
                int? minAge = reader["min"] as int?;
                int minimalAge = minAge ?? MinAgePreference;
                
                MinAge = minimalAge;
                return MinAge;

            }
        } catch (SqlException se) {
            Console.WriteLine("Error inserting minAge in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            return MinAgePreference;
        }

        return MinAgePreference;
    }
    
    
    /// <summary>
    /// Gets the max preferred age from the database and sets it local and returns it
    /// </summary>
    /// <param name="connection">The database connection</param>
    /// <returns>Integer of the max preffered age</returns>
    public int GetMaxAge(SqlConnection connection) {

        SqlCommand query = new SqlCommand("SELECT max FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", Email);

        try {
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read()) {
                int? maxAge = reader["max"] as int?;
                int maximalAge = maxAge ?? MaxAgePreference;
                
                MaxAge = maximalAge;
                return MaxAge;

            }
        }
        catch (SqlException se) {
            Console.WriteLine("Error inserting maxAge in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            return 0;
        }
        return 0;
    }
    
    /// <summary>
    /// Gets the profile pictures from the database
    /// </summary>
    /// <param name="connection">The database connection</param>
    /// <returns>2 dimension byte array</returns>
    public byte[][] GetPicturesFromDatabase(SqlConnection connection) {

        byte[][] result = new byte[MaxAmountOfPictures][];

        //Create query
        SqlCommand query = new SqlCommand("select * from winder.Photos where [user] = @Email", connection);
        query.Parameters.AddWithValue("@Email", Email);

        //Execute query
        try {
            SqlDataReader reader = query.ExecuteReader();
            int i = 0;
            while (reader.Read()) {
                var profilePicture = reader["photo"] as byte[];
                result[i] = profilePicture;
                i++;
            }
        } catch (SqlException se) {
            Console.WriteLine("Error retrieving pictures from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);

        }

        return result;
    }
    
    /// <summary>
    /// Sets the school in the database and local
    /// </summary>
    /// <param name="school">The new school</param>
    /// <param name="connection">The database connection</param>
    public void SetSchool(string school, SqlConnection connection) {
        SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET location = @Location WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", Email);
        query.Parameters.AddWithValue("@Location", school);

        try {
            query.ExecuteReader();
            School = school;
        } catch (SqlException se) {
            Console.WriteLine("Error inserting location in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }

    }

    /// <summary>
    /// Gets the school from the database and sets it local and returns it
    /// </summary>
    /// <param name="connection"></param>
    /// <returns>The school from the database</returns>
    public string GetSchool(SqlConnection connection) {
        SqlCommand query = new SqlCommand("SELECT location FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", Email);

        try {
            SqlDataReader reader = query.ExecuteReader();
            if (reader.Read()) {
                var location = reader["location"] as string;
                School = location;
                return location;

            }
        } catch (SqlException se) {
            Console.WriteLine("Error inserting location in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            return "";
        }
        return "";
    }


    /// <summary>
    /// Registers the user in the database with all its data
    /// </summary>
    /// <param name="firstName">The first name of the user</param>
    /// <param name="middleName">The middle name of the user</param>
    /// <param name="lastName">The last name of the user</param>
    /// <param name="email">The email of the user</param>
    /// <param name="preference">The preference of the user</param>
    /// <param name="birthday">The birthday of the user</param>
    /// <param name="gender">The gender of the user</param>
    /// <param name="bio">The bio of the user</param>
    /// <param name="password">The password of the user</param>
    /// <param name="proficePicture">The first profile Picture of the user</param>
    /// <param name="active">Activation status of the account</param>
    /// <param name="school">The school of the user</param>
    /// <param name="major">The major of the user</param>
    /// <param name="connection">The database connection</param>
    /// <returns></returns>
    public User Registration(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] proficePicture, bool active, string school, string major, SqlConnection connection) {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Email = email;
        Preference = preference;
        BirthDay = birthday;
        Gender = gender;
        Bio = bio;
        ProfilePicture = proficePicture;
        School = school;
        Major = major;


        SqlCommand command = new SqlCommand(
            "INSERT INTO Winder.Winder.[User](firstname, middlename, lastname, birthday, Preference, Email, password, Gender, ProfilePicture, bio, active, location, education)" +
            "VALUES('" + firstName + "', '" + middleName + "', '" + lastName + "', @birthday, '" + preference + "', '" +
            email + "', '" + password + "', '" + gender + "', @img, '" + bio +
            "', @active, '" + school + "', '" + major + "')", connection);
        command.Parameters.AddWithValue("@img", proficePicture);
        command.Parameters.AddWithValue("@active", active);
        command.Parameters.AddWithValue("@birthday", birthday);
        try {
            command.ExecuteReader();
            //Close connection

        } catch (SqlException se) {
            Console.WriteLine("Error registering user in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }

        return this;
    }

    /// <summary>
    /// Update's the current user data to the database
    /// </summary>
    /// <param name="connection">The database connection</param>
    public void UpdateUserDataToDatabase(SqlConnection connection) {
        try { 
            //Create query
            SqlCommand query = new SqlCommand("UPDATE winder.[User]" +
                                              "SET firstname = @firstname, middlename = @middlename, lastname = @lastname, education = @Education,birthday = @birthday, bio = @bio, Gender = @Gender, Preference = @Preference,ProfilePicture = @profilepicture " +
                                              "where Email = @Email", connection);
            query.Parameters.AddWithValue("@firstname", FirstName);
            query.Parameters.AddWithValue("@middlename", MiddleName);
            query.Parameters.AddWithValue("@lastname", LastName);
            query.Parameters.AddWithValue("@birthday", BirthDay);
            query.Parameters.AddWithValue("@Gender", Gender);
            query.Parameters.AddWithValue("@Preference", Preference);
            query.Parameters.AddWithValue("@Email", Email);
            query.Parameters.AddWithValue("@bio", Bio);
            query.Parameters.AddWithValue("@Education", Major);
            query.Parameters.AddWithValue("@ProfilePicture", ProfilePicture);
           
            //Execute query
            query.ExecuteNonQuery();
        }
        catch (SqlException se)
        {
            Console.WriteLine("Error updating user in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
    }
    
    /// <summary>
    /// Removes an interest of a user from the database
    /// </summary>
    /// <param name="interest">The interest to be removed</param>
    /// <param name="connection">The database connection</param>
    public void DeleteInterestInDatabase(string interest, SqlConnection connection) {
        try {
            string query = "Delete From winder.userHasInterest Where UID = @Email and interest = @Interest";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Interest", interest);
            command.ExecuteNonQuery();
            Interests.Select(x => x == interest).ToList().RemoveAll(x => x);

        } catch (SqlException e) {
            Console.WriteLine("Error removing interest from user in database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);

        }
    }
    
    
    public void SetInterestInDatabase(string interest, SqlConnection connection) {

        try {
            string query = "INSERT INTO winder.winder.userHasInterest (winder.UID, winder.interest) VALUES(@Email, @Interest)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Interest", interest);
            command.ExecuteNonQuery();
            Interests.Append(interest);

        } catch (SqlException e) {
            Console.WriteLine("Error registering interest in database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);

        }
    }
    
}

