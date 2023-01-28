using System.Data;
using System.Data.SqlClient;

namespace DataModel;

using System;
using System.Collections.Generic;

public class Database {
    private static Authentication _authentication;
    private static SqlConnection connection;

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

    public void RegisterInterestInDatabase(string userName, string interest) {

        try {
            OpenConnection();
            string query = "INSERT INTO winder.winder.userHasInterest (winder.UID, winder.interest) VALUES(@Email, @Interest)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", userName);
            command.Parameters.AddWithValue("@Interest", interest);
            command.ExecuteNonQuery();
            CloseConnection();
        } catch (SqlException e) {
            Console.WriteLine("Error registering interest in database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
            CloseConnection();
        }
    }

    public void RemoveInterestOfUser(string userName, string interest) {
        try {
            OpenConnection();
            string query = "Delete From winder.userHasInterest Where UID = @Email and interest = @Interest";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", userName);
            command.Parameters.AddWithValue("@Interest", interest);
            command.ExecuteNonQuery();
            CloseConnection();
        } catch (SqlException e) {
            Console.WriteLine("Error removing interest from user in database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
            CloseConnection();
        }
    }
    public void UpdateUserInDatabaseWithNewUserData(User user) {
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
        } catch (SqlException se) {
            Console.WriteLine("Error updating user in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();
        }
    }

    public void SaveProfilePictures(string email, byte[] profilePicture) {
        OpenConnection();
        string query = "INSERT INTO winder.winder.Photos (winder.[user], winder.photo) VALUES(@Email, @profilepicture)";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@profilepicture", profilePicture);
        try {
            command.ExecuteNonQuery();
            CloseConnection();
        } catch (SqlException se) {
            Console.WriteLine("Error saving profile picture");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            //Close connection
            CloseConnection();
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

}

