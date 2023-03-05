using System.Data.SqlClient;
using DataModel;
using Microsoft.Extensions.Configuration;
using Winder.Repositories.Interfaces;
namespace Winder.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        
        private const int MinAgePreference = 18;
        private const int MaxAgePreference = 99;
        private const int MaxAmountOfPictures = 6;
        public const int AmountOfProfilesInQueue = 5;
        private static DateTime MinDateTimeBirth = new DateTime(1925, 01, 01, 0, 0, 0, 0);
        
        // booleans that can be turned off by a developer if wanted
        private const bool AgeAlgorithm = true;
        private const bool PreferenceAlgorithm = true;
        private const bool InterestsAlgorithm = true;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// Checks if the given email and password are correct and returns a user object
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="password">The password</param>
        /// <returns>Returns user if login is valid</returns>

        public User CheckLogin(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //Create query
                SqlCommand query = new SqlCommand("SELECT * FROM winder.winder.[User] WHERE Email = @Email", connection);
                query.Parameters.AddWithValue("@Email", email);

                //Execute query
                SqlDataReader reader = null;
                try {
                    connection.Open();

                    reader = query.ExecuteReader();

                    Console.WriteLine("Checking login");
                    while (reader.Read()) {
                        if (password == reader["password"] as string) {
                            return GetUserFromDatabase(email);
                        }
                    }
                } catch (SqlException se) {
                    Console.WriteLine("Error logging in user");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);

                } finally {
                    if (reader != null) reader.Close();
                }
                return null;
            }

        }

        /// <summary>
        /// Deletes the user from the database
        /// </summary>
        /// <param name="email">The email of the user to delete in the database</param>
        /// <returns>Bool if succeeded  </returns>

        public bool DeleteUser(string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                if (IsEmailUnique(email)) {

                    email = email.ToLower();

                    //querys maken
                    SqlCommand queryDeleteUser = new SqlCommand(
                        "DELETE FROM Winder.Winder.Liked WHERE person = @Email;" +
                        "DELETE FROM Winder.Winder.Liked WHERE likedPerson = @Email;" +
                        "DELETE FROM Winder.Winder.Match WHERE person1 = @Email;" +
                        "DELETE FROM Winder.Winder.Match WHERE person2 = @Email;" +
                        "DELETE FROM Winder.Winder.userHasInterest WHERE UID = @Email;" +
                        "DELETE FROM Winder.Winder.[User] WHERE Email = @Email", connection);
                    queryDeleteUser.Parameters.AddWithValue("@Email", email);

                    //Execute querys
                    try {
                        queryDeleteUser.ExecuteNonQuery();
                        return true;
                    } catch (SqlException se) {
                        Console.WriteLine("Error deleting user");
                        Console.WriteLine(se.ToString());
                        Console.WriteLine(se.StackTrace);
                    }
            
                }
        
                
                return false;
            }
        }

        public void SetMinAge(int minAge, string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                if (minAge > MinAgePreference && minAge < MaxAgePreference)
                {

                    SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET min = @minAge WHERE Email = @Email", connection);
                    query.Parameters.AddWithValue("@Email", email);
                    query.Parameters.AddWithValue("@minAge", minAge);

                    try
                    {
                        query.ExecuteNonQuery();
                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine("Error inserting minAge in database");
                        Console.WriteLine(se.ToString());
                        Console.WriteLine(se.StackTrace);
                    }
                }
            }
        }

        public void SetSchool(string school, string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET location = @Location WHERE Email = @Email", connection);
                query.Parameters.AddWithValue("@Email", email);
                query.Parameters.AddWithValue("@Location", school);

                try
                {
                    query.ExecuteNonQuery();
                    
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error inserting location in database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                }
            }
        }

        public void SetMaxAge(int maxAge, string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                if (maxAge > MinAgePreference && maxAge < MaxAgePreference)
                {
                    
                    SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET max = @maxAge WHERE Email = @Email", connection);
                    query.Parameters.AddWithValue("@Email", email);
                    query.Parameters.AddWithValue("@maxAge", maxAge);

                    try
                    {
                        query.ExecuteNonQuery();
                        
                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine("Error inserting maxAge in database");
                        Console.WriteLine(se.ToString());
                        Console.WriteLine(se.StackTrace);
                    }
                }
            }
        }

        /// <summary>
        ///  Gets the list of users to get for the algorithm
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List of emails</returns>
        public List<string> GetConditionBasedUsers(User user)
        {
            using (SqlConnection connection =
                   new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                Queue<string> usersToSwipe = new Queue<string>();

                DateTime minDate = DateTime.Now.AddYears(0 - user.MinAge);
                DateTime maxDate = DateTime.Now.AddYears(0 - user.MaxAge);
                var formattedMin = minDate.ToString("yyyy-MM-dd HH:mm:ss");
                var formattedMax = maxDate.ToString("yyyy-MM-dd HH:mm:ss");

                string query = "SELECT TOP " + AmountOfProfilesInQueue * 2 + " Email " +
                               "FROM winder.[User] " +
                               "WHERE Email != @Email " + //Not themself
                               "AND active = 1 " + //Is active
                               "AND Email NOT IN (SELECT person FROM Winder.Winder.Liked WHERE likedPerson = @Email AND liked = 1) " + //Not disliked by other person
                               "AND Email NOT IN (SELECT likedPerson FROM winder.winder.Liked WHERE person = @Email) " + //Not already a person that you liked or disliked
                               "AND Email NOT IN (SELECT winder.winder.Match.person1 FROM Winder.Winder.Match WHERE person2 = @Email) " + //Not matched
                               "AND Email NOT IN (SELECT winder.winder.Match.person2 FROM Winder.Winder.Match WHERE person1 = @Email) " + //Not matched
                               "AND location = (SELECT location FROM winder.winder.[User] WHERE Email = @Email) "; // location check

                if (AgeAlgorithm)
                {
                    query = query + " AND birthday >= '" + formattedMax + "' AND birthday <= '" + formattedMin +
                            "'  "; //In age range
                }

                if (PreferenceAlgorithm)
                {
                    query = query +
                            " AND Gender = (SELECT Preference FROM winder.winder.[User] WHERE Email = @Email) "; //Gender check
                    query = query +
                            " AND Preference = (SELECT Gender FROM winder.winder.[User] WHERE Email = @Email) "; //Preference check
                }

                if (InterestsAlgorithm)
                {
                    if (user.Interests.Length > 0)
                    {
                        //Add interests
                        query = query +
                                " AND Email IN (SELECT UID FROM winder.winder.UserHasInterest WHERE interest = " +
                                "'" + user.Interests[0] + "' ";
                        for (int i = 1; i < user.Interests.Length; i++)
                        {
                            query = query + " OR interest =" + " '" + user.Interests[i] + "' ";
                        }

                        query = query + ")";
                    }
                }

                //Randomize the result
                query = query + " ORDER BY NEWID()";

                //Get the users emails
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    reader = command.ExecuteReader(); // execute het command
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string userEmail = reader["Email"] as string ?? "";
                            usersToSwipe.Enqueue(userEmail);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No new profiles found to swipe");
                    }

                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error retrieving the users for the algorithm");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    if (reader != null) reader.Close();
                }

                return usersToSwipe.ToList();
            }

        }


        /// <summary>
        /// Gets the user data from the database
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>User object</returns>
        public User GetUserFromDatabase(string email)
        {
            using (SqlConnection connection =
                   new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                string query = "SELECT * FROM Winder.Winder.[User] WHERE email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string firstName = reader["firstname"] as string ?? string.Empty;
                        string middleName = reader["middlename"] as string ?? string.Empty;
                        string lastName = reader["lastname"] as string ?? string.Empty;
                        string preference = reader["preference"] as string ?? string.Empty;
                        string gender = reader["gender"] as string ?? string.Empty;
                        DateTime birthDay = reader["birthday"] as DateTime? ?? MinDateTimeBirth;
                        string bio = reader["bio"] as string ?? string.Empty;
                        string school = reader["location"] as string ?? string.Empty;
                        string major = reader["education"] as string ?? string.Empty;
                        byte[] profilePicture = (byte[])reader["profilePicture"];
                        var minAgeTemp = reader["min"] as int?;
                        var maxAgeTemp = reader["max"] as int?;

                        int minAge = minAgeTemp ?? MinAgePreference;
                        int maxAge = maxAgeTemp ?? MaxAgePreference;

                        return new User(firstName, middleName, lastName, birthDay, preference, email, gender,
                            profilePicture, bio, school, major, GetInterestsFromUser(email).ToArray(), minAge, maxAge);

                    }

                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error retrieving user from database");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    if (reader != null) reader.Close();
                }

                return new User();
            }
        }

        /// <summary>
        /// Checks the database if the email is unique
        /// </summary>
        /// <param name="email">The email to check for</param>
        /// <returns>Boolean if email is unique</returns>
        public bool IsEmailUnique(string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                List<string> emails = new List<string>();
        
                string sql = "SELECT Email FROM Winder.Winder.[User];";
                SqlCommand command = new SqlCommand(sql, connection);
        
                SqlDataReader reader = null;
                try {
                    reader = command.ExecuteReader();
                    while (reader.Read()) {
                        var item = reader["Email"] as string;
                        emails.Add(item);
                    }

                } catch (SqlException e) {
                    Console.WriteLine("Error getting emails from database");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(e.StackTrace);

                } finally  {
                    if (reader != null) reader.Close();
                }

                if (emails.Contains(email)) {
                    return false;
                }
                return true;
            }
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
        /// <param name="profilePicture">The first profile Picture of the user</param>
        /// <param name="active">Activation status of the account</param>
        /// <param name="school">The school of the user</param>
        /// <param name="major">The major of the user</param>
        /// <returns>Returns true if succeeded</returns>
        public User Registration(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
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
        
                return new User(firstName,middleName,lastName,birthday,preference,email,gender,profilePicture,bio,school,major,new string[0],MinAgePreference,MaxAgePreference);
            }
        }

        /// <summary>
        /// Updates the users profile data in the database
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
        /// <returns>True if successful update to the database</returns>
        public bool UpdateUserData(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major)
        {
            if (IsEmailUnique(email))
            {
                return false;
            }
            else {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    try
                    {
                        //Create query
                        SqlCommand query = new SqlCommand("UPDATE winder.[User]" +
                                                          "SET firstname = @firstname, middlename = @middlename, lastname = @lastname, education = @Education,birthday = @birthday, bio = @bio, Gender = @Gender, Preference = @Preference,ProfilePicture = @profilepicture " +
                                                          "where Email = @Email", connection);
                        query.Parameters.AddWithValue("@firstname", firstName);
                        query.Parameters.AddWithValue("@middlename", middleName);
                        query.Parameters.AddWithValue("@lastname", lastName);
                        query.Parameters.AddWithValue("@birthday", birthday);
                        query.Parameters.AddWithValue("@Gender", gender);
                        query.Parameters.AddWithValue("@Preference", preference);
                        query.Parameters.AddWithValue("@Email", email);
                        query.Parameters.AddWithValue("@bio", bio);
                        query.Parameters.AddWithValue("@Education", major);
                        query.Parameters.AddWithValue("@ProfilePicture", profilePicture);

                        //Execute query
                        query.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine("Error updating user in database");
                        Console.WriteLine(se.ToString());
                        Console.WriteLine(se.StackTrace);
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Sets interest of the user in the database
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="interest">Interest to add</param>
        /// <returns>Returns true if succeeded</returns>
        public bool SetInterest(string email, string interest)
        {
            if (IsEmailUnique(email)) return false;
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                try
                {
                    string query = "INSERT INTO winder.winder.userHasInterest (winder.UID, winder.interest) VALUES(@Email, @Interest)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Interest", interest);
                    command.ExecuteNonQuery();
                    return true;

                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error registering interest in database");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(e.StackTrace);
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets all the interests an user has chosen, from the database
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>String list of the interests</returns>
        public List<string> GetInterestsFromUser(string email)
        {
            using (SqlConnection connection =
                   new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                string query = "SELECT * FROM Winder.Winder.[userHasInterest] WHERE UID = @Email;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                List<string> interestList = new List<string>();

                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = reader["interest"] as string;
                        interestList.Add(item);
                    }


                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error retrieving interests from database");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(e.StackTrace);

                }
                finally
                {
                    if (reader != null) reader.Close();
                }

                return interestList;
            }
        }

        /// <summary>
        /// Update password for current user
        /// </summary>
        /// <param name="password">New password for the user in plain</param>
        public void UpdatePassword(string email, string password)
        {
            // connectieopzetten en query maken
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET password = @password WHERE Email = @Email", connection);
                query.Parameters.AddWithValue("@Email", email);
                query.Parameters.AddWithValue("@password", password);

                //Execute query
                try
                {
                    connection.Open();
                    query.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error updating password");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                }
            }
        }


        public string GetSchool(string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                SqlCommand query = new SqlCommand("SELECT location FROM winder.winder.[User] WHERE Email = @Email", connection);
                query.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = null;
                try
                {
                    reader = query.ExecuteReader();
                    if (reader.Read())
                    {
                        var location = reader["location"] as string;
                        
                        reader.Close();
                        return location;
                    }
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error inserting location in database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);

                    return "";
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                return "";
            }
        }



    }
}