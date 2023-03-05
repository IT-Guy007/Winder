using System.Data.SqlClient;


namespace DataModel;

public class User {
    
    public static User CurrentUser { get; set; }
    
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDay { get; set; }
    public string Preference { get; set; }
    public string Email { get; set; }
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

    public User(string firstName, string middleName, string lastName, DateTime birthDay, string preference, string email, string gender, byte[] profilePicture, string bio, string school, string major, string[] interests, int minAge, int maxAge)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        BirthDay = birthDay;
        Preference = preference;
        Email = email;
        Gender = gender;
        ProfilePicture = profilePicture;
        Bio = bio;
        School = school;
        Major = major;
        Interests = interests;
        MinAge = minAge;
        MaxAge = maxAge;
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
            string query = "DELETE FROM winder.winder.Photos WHERE [user] = @Email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);

            command.ExecuteNonQuery();
        } catch (SqlException se) {
            Console.WriteLine("Error deleting pictures from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
    }

    ///// <summary>
    ///// Delete the user from database
    ///// </summary>
    ///// <param name="connection">The database connection</param>
    //public void DeleteUser(SqlConnection connection) {

    //    if (new UserModel().EmailIsUnique(Email, connection)) {

    //        Email = Email.ToLower();

    //        //querys maken
    //        SqlCommand queryDeleteUser = new SqlCommand(
    //            "DELETE FROM winder.winder.Liked WHERE person = @Email;" +
    //            "DELETE FROM winder.winder.Liked WHERE likedPerson = @Email;" +
    //            "DELETE FROM winder.winder.Match WHERE person1 = @Email;" +
    //            "DELETE FROM winder.winder.Match WHERE person2 = @Email;" +
    //            "DELETE FROM winder.winder.userHasInterest WHERE UID = @Email;" +
    //            "DELETE FROM winder.winder.[User] WHERE Email = @Email", connection);
    //        queryDeleteUser.Parameters.AddWithValue("@Email", Email);

    //        //Execute querys
    //        try {
    //            queryDeleteUser.ExecuteNonQuery();
    //        } catch (SqlException se) {
    //            Console.WriteLine("Error deleting user");
    //            Console.WriteLine(se.ToString());
    //            Console.WriteLine(se.StackTrace);
    //        }

    //    }

    //    SecureStorage.Default.Remove("Email");
    //    SecureStorage.Remove("Email");
    //    SecureStorage.RemoveAll();

    //}

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
    /// <param name="profilePicture">The first profile Picture of the user</param>
    /// <param name="active">Activation status of the account</param>
    /// <param name="school">The school of the user</param>
    /// <param name="major">The major of the user</param>
    /// <param name="connection">The database connection</param>
    /// <returns></returns>
    public User Registration(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major, SqlConnection connection) {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Email = email;
        Preference = preference;
        BirthDay = birthday;
        Gender = gender;
        Bio = bio;
        ProfilePicture = profilePicture;
        School = school;
        Major = major;


        SqlCommand command = new SqlCommand(
            "INSERT INTO Winder.Winder.[User](firstname, middlename, lastname, birthday, Preference, Email, password, Gender, ProfilePicture, bio, active, location, education)" +
            "VALUES('" + firstName + "', '" + middleName + "', '" + lastName + "', @birthday, '" + preference + "', '" +
            email + "', '" + password + "', '" + gender + "', @img, '" + bio +
            "', @active, '" + school + "', '" + major + "')", connection);
        command.Parameters.AddWithValue("@img", profilePicture);
        command.Parameters.AddWithValue("@active", active);
        command.Parameters.AddWithValue("@birthday", birthday);
        try {
            command.ExecuteNonQuery();
        } catch (SqlException se) {
            Console.WriteLine("Error registering user in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
        
        return this;
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
    
    
    /// <summary>
    /// Sets the interest in the database for the specific user
    /// </summary>
    /// <param name="interest">The interest</param>
    /// <param name="connection">The database connection</param>
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

