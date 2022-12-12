using System.Data.SqlClient;

namespace DataModel;


using System;
using System.Collections.Generic;

public class Database {
    private Authentication _authentication = new Authentication();
    public SqlConnection connection;
    public void GenerateConnection() {

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "192.168.1.106,1433";
        builder.UserID = "sa";
        builder.Password = "Qwerty1@";
        builder.InitialCatalog = "winder";

        connection = new SqlConnection(builder.ConnectionString);
    }

    public void OpenConnection() {
        if (connection == null) {
            GenerateConnection();
        }
        connection.Open();
    }

    public void CloseConnection() {

        if (connection == null) {

            GenerateConnection();
        }
        connection.Close();
    }

    public void UpdateLocalUserFromDatabase(string email) {

        if (!string.IsNullOrWhiteSpace(email)) {

            //Start connection
            OpenConnection();


            //Create query
            SqlCommand query = new SqlCommand("select * from winder.winder.[User] where email = @email", connection);
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
                var profilePicture = reader["profilePicture"] as byte[];
                var bio = reader["bio"] as string;
                var school = reader["location"] as string;
                var major = reader["education"] as string;
                Authentication._currentUser = new User(firstName, middleName, lastName, birthday,
                    preferences, email, "", gender ,profilePicture, bio,school,major);
                }

                //Close connection
                CloseConnection();

            } catch (SqlException sql) {
                Console.WriteLine("Error updating local User from Database");
                Console.WriteLine(sql.ToString());
                Console.WriteLine(sql.StackTrace);

                //Close connection
                CloseConnection();
            }

            //Close connection
            CloseConnection();
        }
    }

    public bool CheckLogin(string email, string password) {
        
        string hashed = _authentication.HashPassword(password);
        bool output = false;

        //Start connection
        OpenConnection();

        //Create query
        SqlCommand query = new SqlCommand("SELECT * FROM winder.winder.[User] WHERE Email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);

        //Execute query
        SqlDataReader reader = query.ExecuteReader();

        while (reader.Read()) {
            if (hashed == reader["password"] as string) {
                output = true;
            }
        }

        //Close connection
        CloseConnection();
        UpdateLocalUserFromDatabase(email);
        return output;
        
    }

    public List<string> GetEmailFromDataBase() {

        List<string> emails = new List<string>();
        OpenConnection();
        string sql = "SELECT email FROM Winder.Winder.[User];";
        SqlCommand command = new SqlCommand(sql, connection);
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                var iets1 = reader["email"] as string;
                emails.Add(iets1);
            }
        } catch (SqlException e) {
            Console.WriteLine("Error getting emails from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
        }
        CloseConnection();
        return emails;
    }

    public bool ToggleActivation(string email, bool activate) {

        //Open connection
        OpenConnection();

        SqlCommand query = new SqlCommand("update winder.winder.[User] set active = @Active where email = @Email",
            connection);
        query.Parameters.AddWithValue("@Email", email);
        query.Parameters.AddWithValue("@Active", activate);

        //Execute query
        try {
            int rows = query.ExecuteNonQuery();

            //Close connection
            CloseConnection();
            if (rows != 0) {
                return true;
            }

            return false;
        } catch (SqlException se) {
            Console.WriteLine("Error toggling activation");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();
            return false;
        }

    }

    public void UpdatePassword(string email, string password) {
        Authentication authentication = new Authentication();
        if (authentication.EmailIsUnique(email) == false) { // checken of email in de database staat
            // connectieopzetten en query maken
            
            string hashedpassword = authentication.HashPassword(password); // eerst het password hashen voor het updaten
            OpenConnection();
            SqlCommand query = new SqlCommand("update winder.winder.[User] set password = @password where email = @Email", connection);
            query.Parameters.AddWithValue("@Email", email);
            query.Parameters.AddWithValue("@password", hashedpassword);

            //Execute query
            try {
                query.ExecuteNonQuery();

                //Close connection
                CloseConnection();


            } catch (SqlException se) {
                Console.WriteLine("Error updating password");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);

                //Close connection
                CloseConnection();

            }
        }
    }

    public void DeleteUser(string email) {
        Authentication authentication = new Authentication();

        if (authentication.EmailIsUnique(email) == false) {


            OpenConnection(); // connectie opzetten

            email = email.ToLower();

            //querys maken
            SqlCommand queryLikedPerson = new SqlCommand("delete from winder.winder.Liked where person = @Email", connection);
            queryLikedPerson.Parameters.AddWithValue("@Email", email);
            SqlCommand queryLikedLikedPerson = new SqlCommand("delete from winder.winder.Liked where likedPerson = @Email", connection);
            queryLikedLikedPerson.Parameters.AddWithValue("@Email", email);

            SqlCommand queryMatchPerson1 = new SqlCommand("delete from winder.winder.Match where person1 = @Email", connection);
            queryMatchPerson1.Parameters.AddWithValue("@Email", email);
            SqlCommand queryMatchPerson2 = new SqlCommand("delete from winder.winder.Match where person2 = @Email", connection);
            queryMatchPerson2.Parameters.AddWithValue("@Email", email);

            SqlCommand queryuserHasInterest = new SqlCommand("delete from winder.winder.userHasInterest where UID = @Email", connection);
            queryuserHasInterest.Parameters.AddWithValue("@Email", email);

            SqlCommand queryUser = new SqlCommand("delete from winder.winder.[User] where email = @Email", connection);
            queryUser.Parameters.AddWithValue("@Email", email);

            //Execute querys
            try {
                queryLikedPerson.ExecuteNonQuery();
                queryLikedLikedPerson.ExecuteNonQuery();
                queryMatchPerson1.ExecuteNonQuery();
                queryMatchPerson2.ExecuteNonQuery();
                queryuserHasInterest.ExecuteNonQuery();

                queryUser.ExecuteNonQuery();

                //Close connection
                CloseConnection();
            } catch (SqlException se) {
                Console.WriteLine("Error deleting user");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);

                //Close connection
                CloseConnection();

            }
        }

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

    public User GetUserFromDatabase(string email) {
        User user = null;
        OpenConnection();
        string sql = "SELECT * FROM Winder.Winder.[User] where email = @Email";
        SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                string? username = reader["email"] as string;
                var firstName = reader["firstname"] as string;
                var middleName = reader["middlename"] as string;
                var lastName = reader["lastname"] as string;
                var preferences = reader["preference"] as string;
                string? gender = reader["gender"] as string;
                DateTime? bday = reader["birthday"] as DateTime?;
                var bio = reader["bio"] as string;
                var school = reader["location"] as string;
                var major = reader["education"] as string;
                byte[] img = (byte[])(reader["profilePicture"]);
                
                DateTime birthday = bday ?? new DateTime(1925, 01, 01, 0, 0, 0, 0);
                user = new User(firstName, middleName,lastName,birthday,preferences,email,"",gender, img, bio, school, major);
            }
        } catch (SqlException e) {
            Console.WriteLine("Error retrieving user from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
            CloseConnection();
        }
        CloseConnection();
        return user;
    }

    public bool RegisterInterestInDatabase(string username, string interest) {

        try {
            OpenConnection();
            string sql = "INSERT INTO winder.winder.userHasInterest (winder.UID, winder.interest) VALUES(@Email, @Interest)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", username);
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

    public bool RemoveInterestOfUser(string username, string interest) {
        try {
            OpenConnection();
            string sql = "Delete From winder.userHasInterest Where UID = @Email and interest = @Interest";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", username);
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
    public List<string> LoadInterestsFromDatabaseInListInteresses(string email) {
        List<string> interests = new List<string>();
        OpenConnection();
        string sql = "SELECT * FROM Winder.Winder.[userHasInterest] where UID = @Email;";
        SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                var iets1 = reader["interest"] as string;
                interests.Add(iets1);
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
    public bool UpdateUserInDatabaseWithNewUserData(User user) {
        try {
            //Start connection
            OpenConnection();
            
            //Create query
            SqlCommand query = new SqlCommand("UPDATE winder.[User]" +
            "SET firstname = @firstname, middlename = @middlename, lastname = @lastname, education = @Education,birthday = @birthday, bio = @bio, profilePicture = @profilepicture " +
            "where email = @Email", connection);
            query.Parameters.AddWithValue("@firstname", user.firstName);
            query.Parameters.AddWithValue("@middlename", user.middleName);
            query.Parameters.AddWithValue("@lastname", user.lastName);
            query.Parameters.AddWithValue("@birthday", user.birthDay);
            query.Parameters.AddWithValue("@preference", user.preference);
            query.Parameters.AddWithValue("@Email", user.email);
            query.Parameters.AddWithValue("@bio", user.bio);
            query.Parameters.AddWithValue("@Education", user.major);
            query.Parameters.AddWithValue("@profilePicture", user.profilePicture);
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

    public bool SaveProfilePictures(string email, byte[] profilepicture) {
        OpenConnection();
        string sql = "INSERT INTO winder.winder.Photos (winder.[user], winder.photo) VALUES(@Email, @profilepicture)";
        SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@profilepicture", profilepicture);
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

    public void RegistrationFunction(string firstname, string middlename, string lastname, string email, string preference, DateTime birthday, string gender,
                                 string bio, string password, byte[] proficePicture, bool active, string locatie, string opleiding) {
        OpenConnection();
        SqlCommand command = new SqlCommand("INSERT INTO Winder.Winder.[User](firstname, middlename, lastname, birthday, preference, email, password, gender, profilePicture, bio, active, location, education)" +
                       "VALUES('" + firstname + "', '" + middlename + "', '" + lastname + "', @birthday, '" + preference + "', '" + email + "', '" + password + "', '" + gender + "', @img, '" + bio +
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
        string selectPreference = "SELECT preference FROM winder.[User] WHERE email = @Email";
        SqlCommand query = new SqlCommand(selectPreference, connection);
        query.Parameters.AddWithValue("@Email", email);

        try {
            query.ExecuteReader();
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting preference in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }


        if (selectPreference.Equals(null)) {
            OpenConnection();
            SqlCommand query1 = new SqlCommand("INSERT INTO winder.[User] (preference) VALUES (@Preference) WHERE email = @Email", connection);
            query1.Parameters.AddWithValue("@Email", email);
            query1.Parameters.AddWithValue("@Preference", preference);
            try {
                query1.ExecuteReader();
                CloseConnection();
            } catch (SqlException se) {
                Console.WriteLine("Error inserting preference in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                CloseConnection();
            }
        } else {
            OpenConnection();
            SqlCommand query2 = new SqlCommand("UPDATE winder.winder.[User] SET preference = @Preference WHERE email = @Email", connection);
            query2.Parameters.AddWithValue("@Email", email);
            query2.Parameters.AddWithValue("@Preference", preference);
            try {
                query2.ExecuteReader();
                CloseConnection();
            } catch (SqlException se) {
                Console.WriteLine("Error inserting preference in database");
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
       
        SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET location = @Location WHERE email = @Email", connection);
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

        SqlCommand query = new SqlCommand("SELECT preference FROM winder.winder.[User] WHERE email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);


        try {
            SqlDataReader reader = query.ExecuteReader();
            if (reader.Read())
            {
                var preference = reader["preference"] as string;
                CloseConnection();
                return preference;

            }
        } catch (SqlException se) {
            Console.WriteLine("Error inserting preference in database");
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

        SqlCommand query = new SqlCommand("SELECT location FROM winder.winder.[User] WHERE email = @Email", connection);
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

        SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET min = @minAge WHERE email = @Email", connection);
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

    public void SetMaxAge(string email, int maxAge) {
        
        // open connection
        OpenConnection();

        SqlCommand query = new SqlCommand("UPDATE winder.winder.[User] SET max = @maxAge WHERE email = @Email", connection);
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

        SqlCommand query = new SqlCommand("SELECT min FROM winder.winder.[User] WHERE email = @Email", connection);
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

        SqlCommand query = new SqlCommand("SELECT max FROM winder.winder.[User] WHERE email = @Email", connection);
        query.Parameters.AddWithValue("@Email", email);


        try {
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read()) {
                int? minAge = reader["max"] as int?;
                CloseConnection();
                int minimalAge = minAge ?? 18;
                return minimalAge;

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

    public byte[][] GetPicturesFromDatabase(string email) {
        
        byte[][] result = new byte[10][];

        //TO-DO: Get pictures from database
        //Start connection
        OpenConnection();

        //Create query
        SqlCommand query = new SqlCommand("select * from winder.Photos where [user] = @email", connection);
        query.Parameters.AddWithValue("@email", email);

        //Execute query
        try {
            SqlDataReader reader = query.ExecuteReader();
            int i = 0;
            while(reader.Read()) {
                var profilePicture = reader["photo"] as byte[];
                result[i] = profilePicture;
                i++;
            }
        } catch(SqlException se) {
            Console.WriteLine("Error retrieving pictures from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }
        CloseConnection();
        return result;
    }
    
    public string[] GetUsersWhoLikedYou(string email) {
        
        List<string> users = new List<string>();
        OpenConnection();

    
        SqlCommand command = new SqlCommand("SELECT person FROM Winder.Winder.Liked WHERE likedPerson = @email AND liked = 1 " + // selects the users that have liked the given user
            "AND person not in (select likedPerson from Winder.Winder.Liked where person = @email) ", connection); // except the ones that the given user has already disliked or liked
        command.Parameters.AddWithValue("@email", email);
        
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

    public Queue<string> GetRestOfUsers(string email) {
        List<string> userslist = new List<string>();

        string[] userswholikedyou = GetUsersWhoLikedYou(email);
        string[] userswithcommoninterests = GetUsersWithCommonInterest(email);
        
        OpenConnection();

        SqlCommand command = new SqlCommand("SELECT TOP 10 email FROM Winder.Winder.[User] WHERE email != @email " +
        "AND email not in (select person from Winder.Winder.Liked where likedPerson = @email and liked = 0) " + // selects the users which have not disliked the given user
        "AND email not in (select likedPerson from Winder.Winder.Liked where person = @email) " +               // selects the users that the given user had not disliked or already liked
        "AND email not in (select person1 from Winder.Winder.Match where person2 = @email) " +                  // selects the users that the given user has not already matched with 
        "AND email not in (select person2 from Winder.Winder.Match where person1 = @email) ", connection);                                                                                 //Limit to 5
        command.Parameters.AddWithValue("@email", email);

        try {
            SqlDataReader reader = command.ExecuteReader(); // execute het command

            while (reader.Read()) {
                string person = reader["email"] as string ?? "Unknown";
                userslist.Add(person);   // zet elk persoon in de users 
            }
             
        } catch (SqlException se) {
            Console.WriteLine("Error retrieving rest of users from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            CloseConnection();
        }
        
        string[] users = userslist.ToArray();
        users = users.Except(userswholikedyou).ToArray();
        users = users.Except(userswithcommoninterests).ToArray();  // makes it so the rest of users does not contain the users that liked you or have common interests because we have different methods for them
        
        Queue<string> queue = new Queue<string>();                 //make a queue from the users
        foreach (string user in users) {
            queue.Enqueue(user);
        }

        CloseConnection();
        return queue;
    }

    public string[] GetUsersWithCommonInterest(string email) {
        List<string> users = new List<string>();

        List<string> interestsgivenuser = LoadInterestsFromDatabaseInListInteresses(email).ToList();


        string query = "SELECT UID FROM Winder.Winder.UserHasInterest WHERE UID != @email AND (interest = @interest"; // selects the users which have a common interest

        for (int i = 1; i < interestsgivenuser.Count; i++) {
            query = query + " or interest =" + " '" + interestsgivenuser[i] + "' ";
        }
        query = query + ") AND UID not in (select person from Winder.Winder.Liked where likedPerson = @email and liked = 0) " + // selects the users which have not disliked the given user
                        "and UID not in (select likedPerson from Winder.Winder.Liked where person = @email) " +// selects the users that the given user has not already disliked or liked
                        "and UID not in (select person1 from Winder.Winder.Match where person2 = @email) " +
                        "and UID not in (select person2 from Winder.Winder.Match where person1 = @email) ";          // selects the users that the given user has not matched with

        OpenConnection();

        // selects every user which has common interests except the ones which have disliked the given user or the given user has disliked
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@interest", interestsgivenuser[0]);


        try {
            SqlDataReader reader = command.ExecuteReader(); // execute het command
            while (reader.Read())
            {
                string person = reader["UID"] as string ?? "Unknown";
                users.Add(person);   // zet elk persoon in de users tot het maxamount is bereikt of er niks meer te readen valt
            }
            //Close connection
            
        } catch (SqlException se) {
            Console.WriteLine("Error retrieving users with common interests from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();


        }
        CloseConnection();
        return users.ToArray();
    }
    

    public List<string> AlgorithmForSwiping(string email) {
        Random rnd = new Random();

        string[] usersWhoLikedYou = GetUsersWhoLikedYou(email); //Users that liked you
        string[] usersWithCommonInterests = GetUsersWithCommonInterest(email);

        usersWithCommonInterests = usersWithCommonInterests.Where(x => !usersWhoLikedYou.Contains(x)).ToArray(); // is needed because they could have duplicates between the lists and is done this way because users who likedyou has a higher priority in the algorithm

        Queue<string> restOfUsers = GetRestOfUsers(email); //Random Users
        usersWhoLikedYou = usersWhoLikedYou.OrderBy(x => rnd.Next()).ToArray(); 
        usersWithCommonInterests = usersWithCommonInterests.OrderBy(x => rnd.Next()).ToArray();     // shuffle the arrays randomly instead of how they are sorted in the database

        usersWithCommonInterests = usersWithCommonInterests.GroupBy(x => x).ToList().OrderByDescending(g => g.Count()).Select(g => g.Key).ToArray(); // sorts so that the values with the most duplicates are in front and removes the duplicates

        List<string> users = new List<string>(); //Result
        for (int i = 0; i < 5; i++) {
            var userToAdd = "";
            if (i < 3 && usersWithCommonInterests.Length >= 3) {    // preferebaly we want 3 people with common interests{
                userToAdd = usersWithCommonInterests[i];

            } else if (i == 3 & usersWhoLikedYou.Length > 0) {      //1 person who has liked
                userToAdd = usersWhoLikedYou[i];

            } else {          //Fill up with random people
                if(restOfUsers.Count > 0) {
                    userToAdd = restOfUsers.Dequeue();
                }
            }

            //Check if already exists in current Queue, otherwise add random
            var alreadyExists = Authentication._profileQueue.Where(i => i.user.email == userToAdd);
            if (alreadyExists.Count() <= 0 && alreadyExists != null) {
                users.Add(userToAdd);
            } else { 
                while (string.IsNullOrWhiteSpace(userToAdd) && restOfUsers.Count != 0) {
                    var nextUsers = restOfUsers.Dequeue();
                    var alreadyExistsLoop = Authentication._profileQueue.Where(i => i.user.email == nextUsers);
                    if(alreadyExistsLoop.Count() == 0) {
                        userToAdd = nextUsers;
                    }
                }
                if (!string.IsNullOrWhiteSpace(userToAdd) && userToAdd != "") {
                users.Add(userToAdd);
                }
            }
        }

        //Check if users contains empty strings
        List<string> result = new List<string>();
        foreach (var user in users) {
            if(user != "") {
                result.Add(user);
            }
        }
        return result;
    }

    //User to get the profiles for the match(run async)
    public Profile[] Get5Profiles(string email) {
        //The algorithm that determines who to get

        //The users(email) to get
        List<string> usersToRetrief = new List<string>();

        usersToRetrief = AlgorithmForSwiping(email); 

        //Results
        Profile[] profiles = new Profile[usersToRetrief.Count()];
        
        //Retrieving
        for (int i = 0; i < usersToRetrief.Count(); i++) {

            //Get the user
            User user = GetUserFromDatabase(usersToRetrief[i]);

            //Get the interests of the user
            user.interests = LoadInterestsFromDatabaseInListInteresses(usersToRetrief[i]).ToArray();

            //Get the images of the user
            byte[][] images = GetPicturesFromDatabase(usersToRetrief[i]);
            var profile = new Profile(user, images);
            
            profiles[i] = profile;
        }
        return profiles;
    }

}

