using System.Data;
using System.Data.SqlClient;

namespace DataModel;

using System;
using System.Collections.Generic;

public class Database {
    private static Authentication _authentication;
    private static SqlConnection connection;

    private const bool ageAlgorithm = true;
    private const bool preferenceAlgorithm = true;
    private const bool interestsAlgorithm = true;

    private static string dataSourceConnection = "192.168.1.106,1433";
    private static string userIDConnection = "sa";
    private static string passwordConnection = "Qwerty1@";
    private static string initialCatalogConnection = "winder";

    private string emailHasToStartWith = "s";
    private string emailHasToEndWith = "@student.windesheim.nl";

    private static int amountOfPictures = 6;

    private static int amountOfUsersToReturnForAlgorithm = 5;
    
    public static void Initialize() {
        _authentication = new Authentication();
       GenerateConnection();
    }
    private static void GenerateConnection() {
        
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder {
            DataSource = dataSourceConnection,
            UserID = userIDConnection,
            Password = passwordConnection,
            InitialCatalog = initialCatalogConnection
        };

        connection = new SqlConnection(builder.ConnectionString);
    }

    public static void OpenConnection() {
        if (connection == null) {
            GenerateConnection();
            OpenConnection();
        } else if (connection.State == ConnectionState.Closed) {
            connection.Open();
        }

    }

    public static void CloseConnection() {

        if (connection == null) {

            GenerateConnection();
        }
        connection.Close();
    }

    public List<string> GetInterestsFromDataBase() {
        List<string> interests = new List<string>();
        OpenConnection();
        string sql = "SELECT * FROM Winder.Winder.[Interests];";
        SqlCommand command = new SqlCommand(sql, connection);
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                var item = reader["name"] as string;
                interests.Add(item);
            }
        } catch (SqlException e) {
            Console.WriteLine("Error retrieving interests from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
            CloseConnection();
        }
        CloseConnection();
        return interests;
    }

    public bool RegisterInterestInDatabase(string userName, string interest) {

        try {
            OpenConnection();
            string query = "INSERT INTO winder.winder.userHasInterest (winder.UID, winder.interest) VALUES(@Email, @Interest)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", userName);
            command.Parameters.AddWithValue("@Interest", interest);
            command.ExecuteNonQuery();
            CloseConnection();
            return true;
        } catch (SqlException e) {
            Console.WriteLine("Error registering interest in database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
            CloseConnection();
            return false;
        }
    }

    public bool RemoveInterestOfUser(string userName, string interest) {
        try {
            OpenConnection();
            string query = "Delete From winder.userHasInterest Where UID = @Email and interest = @Interest";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", userName);
            command.Parameters.AddWithValue("@Interest", interest);
            command.ExecuteNonQuery();
            CloseConnection();
            return true;
        } catch (SqlException e) {
            Console.WriteLine("Error removing interest from user in database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
            CloseConnection();
            return false;
        }
    }
    public bool UpdateUserInDatabaseWithNewUserData(User user) {
        try {
            //Start connection
            OpenConnection();

            //Create query
            SqlCommand query = new SqlCommand("UPDATE winder.[User]" +
            "SET firstname = @firstname, middlename = @middlename, lastname = @lastname, education = @Education,birthday = @birthday, bio = @bio, Gender = @Gender, Preference = @Preference,ProfilePicture = @profilepicture " +
            "where Email = @Email", connection);
            query.Parameters.AddWithValue("@firstname", user.FirstName);
            query.Parameters.AddWithValue("@middlename", user.MiddleName);
            query.Parameters.AddWithValue("@lastname", user.LastName);
            query.Parameters.AddWithValue("@birthday", user.BirthDay);
            query.Parameters.AddWithValue("@Gender", user.Gender);
            query.Parameters.AddWithValue("@Preference", user.Preference);
            query.Parameters.AddWithValue("@Email", user.Email);
            query.Parameters.AddWithValue("@bio", user.Bio);
            query.Parameters.AddWithValue("@Education", user.Major);
            query.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture);
            //Execute query
            query.ExecuteNonQuery();
            //Close connection
            CloseConnection();
            return true;
        } catch (SqlException se) {
            Console.WriteLine("Error updating user in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();
            return false;
        }
    }

    public bool SaveProfilePictures(string email, byte[] profilePicture) {
        OpenConnection();
        string query = "INSERT INTO winder.winder.Photos (winder.[user], winder.photo) VALUES(@Email, @profilepicture)";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@profilepicture", profilePicture);
        try {
            command.ExecuteNonQuery();
            CloseConnection();
            return true;
        } catch (SqlException se) {
            Console.WriteLine("Error saving profile picture");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();
            return false;
        }
    }

    public void RegistrationFunction(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender,
                                 string bio, string password, byte[] proficePicture, bool active, string locatie, string opleiding) {
        OpenConnection();
        SqlCommand command = new SqlCommand("INSERT INTO Winder.Winder.[User](firstname, middlename, lastname, birthday, Preference, Email, password, Gender, ProfilePicture, bio, active, location, education)" +
                       "VALUES('" + firstName + "', '" + middleName + "', '" + lastName + "', @birthday, '" + preference + "', '" + email + "', '" + password + "', '" + gender + "', @img, '" + bio +
                       "', @active, '" + locatie + "', '" + opleiding + "')", connection);
        command.Parameters.AddWithValue("@img", proficePicture);
        command.Parameters.AddWithValue("@active", active);
        command.Parameters.AddWithValue("@birthday", birthday);
        try {
            command.ExecuteReader();
            //Close connection
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error registering user in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();
        }

    }


    public void InsertPreference(string email, string preference) {
        // open connection

        OpenConnection();
        string selectPreference = "SELECT Preference FROM winder.[User] WHERE Email = @Email";
        SqlCommand query = new SqlCommand(selectPreference, connection);
        query.Parameters.AddWithValue("@Email", email);

        try {
            query.ExecuteReader();
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting Preference in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }


        if (selectPreference.Equals(null)) {
            OpenConnection();
            SqlCommand query1 = new SqlCommand("INSERT INTO winder.[User] (Preference) VALUES (@Preference) WHERE Email = @Email", connection);
            query1.Parameters.AddWithValue("@Email", email);
            query1.Parameters.AddWithValue("@Preference", preference);
            try {
                query1.ExecuteReader();
                CloseConnection();
            } catch (SqlException se) {
                Console.WriteLine("Error inserting Preference in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                CloseConnection();
            }
        } else {
            OpenConnection();
            SqlCommand query2 = new SqlCommand("UPDATE winder.winder.[User] SET Preference = @Preference WHERE Email = @Email", connection);
            query2.Parameters.AddWithValue("@Email", email);
            query2.Parameters.AddWithValue("@Preference", preference);
            try {
                query2.ExecuteReader();
                CloseConnection();
            } catch (SqlException se) {
                Console.WriteLine("Error inserting Preference in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                CloseConnection();
            }
        }

        CloseConnection();
    }

    public void InsertLocation(string email, string location) {
        // open connection

        OpenConnection();

        SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET location = @Location WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);
        query.Parameters.AddWithValue("@Location", location);

        try {
            query.ExecuteReader();
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting location in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }


    }

    public string GetPreference(string email) {
        // open connection

        OpenConnection();

        SqlCommand query = new SqlCommand("SELECT Preference FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);


        try {
            SqlDataReader reader = query.ExecuteReader();
            if (reader.Read())
            {
                var preference = reader["Preference"] as string;
                CloseConnection();
                return preference;

            }
        } catch (SqlException se) {
            Console.WriteLine("Error inserting Preference in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
            return "";
        }
        return "";
    }

    public string GetLocation(string email) {
        // open connection

        OpenConnection();

        SqlCommand query = new SqlCommand("SELECT location FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);


        try {
            SqlDataReader reader = query.ExecuteReader();
            if (reader.Read()) {
                var location = reader["location"] as string;
                CloseConnection();
                return location;

            }
        } catch (SqlException se) {
            Console.WriteLine("Error inserting location in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
            return "";
        }
        return "";
    }

    public void SetMinAge(string email, int minAge) {
        
        // open connection
        OpenConnection();

        if (minAge > 17 && minAge < 101) {


            SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET min = @minAge WHERE Email = @Email",
                connection);
            query.Parameters.AddWithValue("@Email", email);
            query.Parameters.AddWithValue("@minAge", minAge);

            try {
                query.ExecuteReader();
                CloseConnection();
            } catch (SqlException se) {
                Console.WriteLine("Error inserting minAge in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                CloseConnection();
            }
        }
    }

    public void SetMaxAge(string email, int maxAge) {

        // open connection
        OpenConnection();

        SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET max = @maxAge WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);
        query.Parameters.AddWithValue("@maxAge", maxAge);

        try {
            query.ExecuteReader();
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting maxAge in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }

    }

    public int GetMinAge(string email) {
        // open connection

        OpenConnection();

        SqlCommand query = new SqlCommand("SELECT min FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);


        try {
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read()) {
                int? minAge = reader["min"] as int?;
                CloseConnection();
                int minimalAge = minAge ?? 18;
                return minimalAge;

            }
        } catch (SqlException se) {
            Console.WriteLine("Error inserting minAge in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
            return 0;
        }
        return 0;
    }

    public int GetMaxAge(string email) {
        // open connection

        OpenConnection();

        SqlCommand query = new SqlCommand("SELECT max FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);


        try {
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read()) {
                int? maxAge = reader["max"] as int?;
                CloseConnection();
                int maximalAge = maxAge ?? 99;
                return maximalAge;

            }
        }
        catch (SqlException se) {
            Console.WriteLine("Error inserting maxAge in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
            return 0;
        }
        return 0;
    }


    //<summary>Checks if there is a match between users.</summary>
    public bool CheckMatch(string emailCurrentUser, string emailLikedPerson) {
        bool match;
        OpenConnection();

        SqlCommand command = new SqlCommand("SELECT * FROM Winder.Winder.[Liked] WHERE person = @emailLikedPerson AND likedPerson = @emailCurrentUser AND liked = 1", connection);
        command.Parameters.AddWithValue("@emailLikedPerson", emailLikedPerson);
        command.Parameters.AddWithValue("@emailCurrentUser", emailCurrentUser);

        try {
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            match = reader.HasRows;
            //Close connection
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error checking match in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            match = false;
            //Close connection
            CloseConnection();
        }

        return match;
    }

    public void NewLike(string emailCurrentUser, string emailLikedPerson) {
        //There is no match yet
        OpenConnection();
        SqlCommand command = new SqlCommand("INSERT INTO Winder.Winder.[Liked] (person, likedPerson, liked) " +
                                            "VALUES (@currentUser, @likedUser, 1)", connection);
        command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
        command.Parameters.AddWithValue("@likedUser", emailLikedPerson);

        try {
            command.ExecuteReader();
            //Close connection
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting like in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();
        }
    }

    //als iemand jou gedisliked heeft krijg jij hem niet meer te zien want een match is dan niet meer mogelijk
    public void NewDislike(string emailCurrentUser, string emailLikedPerson) {
        //There is no match yet
        OpenConnection();
        SqlCommand command = new SqlCommand("INSERT INTO Winder.Winder.[Liked] (person, likedPerson, liked) " +
                                            "VALUES (@currentUser, @likedUser, 0)", connection);
        command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
        command.Parameters.AddWithValue("@likedUser", emailLikedPerson);

        try {
            command.ExecuteReader();
            //Close connection
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting dislike in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();
        }
    }

    public void NewMatch(string emailCurrentUser, string emailLikedPerson) {
        OpenConnection();

        SqlCommand command = new SqlCommand("INSERT INTO winder.winder.[Match] (person1, person2) " +
                                            "VALUES (@currentUser, @likedUser)", connection);
        command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
        command.Parameters.AddWithValue("@likedUser", emailLikedPerson);

        try {
            command.ExecuteReader();
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting match in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }
    }
    
    public void deleteLikeOnMatch(string emailCurrentUser, string emailLikedUser) {
        OpenConnection();

        SqlCommand command = new SqlCommand("DELETE FROM winder.winder.[Liked] " +
                                            "WHERE person = @emailLikedUser AND likedPerson = @emailCurrentUser ", connection);
        command.Parameters.AddWithValue("@emailLikedUser", emailLikedUser);
        command.Parameters.AddWithValue("@emailCurrentUser", emailCurrentUser);

        try {
            command.ExecuteReader();
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error deleting like on match in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }
    }

    public static byte[][] GetPicturesFromDatabase(string email) {

        byte[][] result = new byte[amountOfPictures][];

        //TO-DO: Get pictures from database
        //Start connection
        OpenConnection();

        //Create query
        SqlCommand query = new SqlCommand("select * from winder.Photos where [user] = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);

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
            CloseConnection();
        }
        CloseConnection();
        return result;
    }

    public static string[] GetUsersWhoLikedYou(string email) {

        List<string> users = new List<string>();
        OpenConnection();

        SqlCommand command = new SqlCommand("SELECT top 5 person FROM Winder.Winder.Liked " +
                                            "WHERE likedPerson = @Email AND liked = 1 " + // selects the users that have liked the given user
                                            "AND person not in (select likedPerson from Winder.Winder.Liked where person = @Email) " + // except the ones that the given user has already disliked or liked
                                            "order by NEWID()", connection); 
        command.Parameters.AddWithValue("@Email", email);

        try {
            SqlDataReader reader = command.ExecuteReader(); // execute het command

            while (reader.Read()) {
                string person = reader["person"] as string ?? "Unknown";   
                users.Add(person);   // zet elk persoon in de users 
            }
            //Close connection

        } catch (SqlException se) {
            Console.WriteLine("Error retrieving users who liked you from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();

        }
        CloseConnection();
        return users.ToArray();
    }

    // Enque users to swipe
    private static void EnqueueUsersToSwipe(Queue<string> usersToSwipe, string email, string query) {
        //Random people
        query = query + " ORDER BY NEWID()";
        Console.WriteLine(query);
        
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);

        OpenConnection();

        try
        {
            SqlDataReader reader = command.ExecuteReader(); // execute het command
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string user = reader["Email"] as string ?? "";
                    usersToSwipe.Enqueue(user);
                }
            }
            else
            {
                Console.WriteLine("No new profiles found to swipe");
            }
            CloseConnection();

        }
        catch (SqlException se)
        {
            Console.WriteLine("Error retrieving emails for algorithm");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);

            //Close connection
            CloseConnection();
        }
        CloseConnection();
    }

    // Enque who liked you
    private static void EnqueWhoLikedYou(Queue<string> likedQueue, string[] usersWhoLikedYou)
    {
        foreach (string user in usersWhoLikedYou)
        {
            likedQueue.Enqueue(user);
        }
    }

    // Loop though users and check if in currentQueue
    private static void CheckCurrentQueue(List<string> result, Queue<string> usersToSwipe, Queue<string> likedQueue, int place)
    {
        while (result.Count != amountOfUsersToReturnForAlgorithm && (usersToSwipe.Count > 0 || likedQueue.Count > 0))
        {

            var userToAdd = "";

            try
            {
                if (place == result.Count)
                {
                    //For the liked user place else random user
                    if (likedQueue.Count > 0)
                    {
                        userToAdd = likedQueue.Dequeue();
                    }
                    else
                    {
                        userToAdd = usersToSwipe.Dequeue();
                    }
                }
                else
                {
                    if (usersToSwipe.Count > 0)
                    {
                        userToAdd = usersToSwipe.Dequeue();
                    }
                    else
                    {
                        userToAdd = likedQueue.Dequeue();
                    }
                }

                //Check if already exists in current Queue
                IEnumerable<Profile> alreadyExists = Authentication.ProfileQueue.Where(i => i.user.Email == userToAdd);
                if (!alreadyExists.Any() && !usersToSwipe.Contains(userToAdd) && !string.IsNullOrEmpty(userToAdd))
                {
                    Console.WriteLine("Adding user");
                    result.Add(userToAdd);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding user to queue");
                Console.WriteLine(e.ToString());
                Console.WriteLine(e.StackTrace);
            }

        }
    }

    
    public static List<string> AlgorithmForSwiping() {

        //Get 10 users from the database within the criteria
        Queue<string> usersToSwipe = new Queue<string>();

        DateTime minDate = DateTime.Now.AddYears(0 - Authentication.CurrentUser.MinAge);
        DateTime maxDate = DateTime.Now.AddYears(0 - Authentication.CurrentUser.MaxAge);
        var formattedMin = minDate.ToString("yyyy-MM-dd HH:mm:ss");
        var formattedMax = maxDate.ToString("yyyy-MM-dd HH:mm:ss");

        string query = "select top 10 Email " +
                       "from winder.[User] " +
                       "where Email != @Email " + //Not themself
                       "and Email not in (select person from Winder.Winder.Liked where likedPerson = @Email and liked = 1) " + //Not disliked by other person
                       "and Email not in (select likedPerson from winder.winder.Liked where person = @Email) " + //Not already a person that you liked or disliked
                       "and Email not in (select winder.winder.Match.person1 from Winder.Winder.Match where person2 = @Email) " + //Not matched
                       "and Email not in (select winder.winder.Match.person2 from Winder.Winder.Match where person1 = @Email) " + //Not matched
                       "And location = (select location from winder.winder.[User] where Email = @Email) "; // location check

        if (ageAlgorithm)
        {
            query = query + " and birthday >= '" + formattedMax + "' and birthday <= '" + formattedMin + "'  "; //In age range
        }

        if (preferenceAlgorithm)
        {
            query = query + " and Gender = (select Preference from winder.winder.[User] where Email = @Email) "; //Gender check
            query = query + " and Preference = (select Gender from winder.winder.[User] where Email = @Email) "; //Preference check
        }

        if (interestsAlgorithm) {
            if (interestsGivenUser.Count > 0) {
                //Add interests
                query = query + " AND Email in (select UID from winder.winder.UserHasInterest where interest = " + "'" + interestsGivenUser[0] + "' ";
                for (int i = 1; i < interestsGivenUser.Count; i++)
                {
                    query = query + " or interest =" + " '" + interestsGivenUser[i] + "' ";
                }
                query = query + ")";
            }
        }
        
        // Enque users to swipe
        EnqueueUsersToSwipe(usersToSwipe, email, query);

        //Get 5 users who likes you
        string[] usersWhoLikedYou = GetUsersWhoLikedYou(email);
        Queue<string> likedQueue = new Queue<string>();
        
        //Liked into queue
        EnqueWhoLikedYou(likedQueue, usersWhoLikedYou);

        //Place for the users who liked you
        Random random = new Random();
        int place = random.Next(0, amountOfUsersToReturnForAlgorithm);


        //Check if users contains empty strings
        List<string> result = new List<string>();


        //Loop though users and check if in currentQueue
        CheckCurrentQueue(result,usersToSwipe,likedQueue,place);


        return result;
    }


    public List<User> GetMatchedStudentsFromUser(string email) {
        List<User> users = new List<User>();
        List<string> emails = new List<string>();
        try
        {
            OpenConnection();
            string query = "SELECT person1, person2 FROM Winder.Winder.Match WHERE person1 = @Email OR person2 = @Email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string person1 = reader["person1"] as string ?? "Unknown";
                string person2 = reader["person2"] as string ?? "Unknown";
                if (person1 == email)
                {
                    emails.Add(person2);
                }
                else
                {
                    emails.Add(person1);
                }
            }
            CloseConnection();
            emails.ForEach(x => users.Add(new User().GetUserFromDatabase(x, Database2.ReleaseConnection)));
        }
        catch (SqlException se)
        {
            Console.WriteLine("Error retrieving matches from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }
        CloseConnection();
        return users;
    }

}

