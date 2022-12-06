namespace DataModel;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Drawing.Imaging;
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
        try
        {
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

//<<<<<<< HEAD
    public void UpdatePassword(string email, string password)
    {
        Authentication a = new Authentication();
        if (a.EmailIsUnique(email) == false) // checken of email in de database staat
        {
            // connectieopzetten en query maken
            Authentication authentication = new Authentication();
            string hashedpassword = authentication.HashPassword(password); // eerst het password hashen voor het updaten
            openConnection();
            SqlCommand query = new SqlCommand("update winder.winder.[User] set password = @password where email = @Email", connection);
            query.Parameters.AddWithValue("@Email", email);
            query.Parameters.AddWithValue("@password", hashedpassword);

            //Execute query
            try
            {
                query.ExecuteNonQuery();

                //Close connection
                closeConnection();


            }
            catch (SqlException se)
            {

                //Close connection
                closeConnection();

            }
        }
    }


    public void DeleteUser(string email)
    {
        Authentication a = new Authentication();

        if (a.EmailIsUnique(email) == false)
        {


            openConnection(); // connectie opzetten

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
            try
            {
                queryLikedPerson.ExecuteNonQuery();
                queryLikedLikedPerson.ExecuteNonQuery();
                queryMatchPerson1.ExecuteNonQuery();
                queryMatchPerson2.ExecuteNonQuery();
                queryuserHasInterest.ExecuteNonQuery();

                queryUser.ExecuteNonQuery();

                //Close connection
                closeConnection();
            }
            catch (SqlException se)
            {

                //Close connection
                closeConnection();

            }
        }

    }
    




    
//=======
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
                DateTime? bday = reader["birthday"] as DateTime?;
                var bio = reader["bio"] as string;
                
                DateTime birthday = bday ?? new DateTime(1925, 01, 01, 0, 0, 0, 0);
                user = new User(firstName, middleName,lastName,birthday,preferences,email,"",gender, new Bitmap(1024,1024),bio);
            }
        }
        catch (SqlException e)
        {
            closeConnection();
        }
        closeConnection();
        return user;
    }

    
    public void RegisterInterestInDatabase(string username, string interest)
    {
        openConnection();
        string sql = "INSERT INTO winder.winder.userHasInterest (winder.UID, winder.interest) VALUES(@Email, @Interest)";
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

    public void removeInterestOutOfuserHasInterestTableDatabase(string username, string interest)
    {
        openConnection();
        string sql = "Delete From winder.userHasInterest Where UID = @Email and interest = @Interest";
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

    public List<string> LoadInterestsFromDatabaseInListInteresses(string email)
    {
        List<string> interests = new List<string>();
        openConnection();
        string sql = "SELECT * FROM Winder.Winder.[userHasInterest] where UID = @Email;";
        SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
        try
        {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var iets1 = reader["interest"] as string;
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

    public void updateUserInDatabaseWithNewUserProfile(User user)
    {
        try
        {
            //Start connection
            openConnection();
            //Create query
            SqlCommand query = new SqlCommand("UPDATE winder.[User]" +
            "SET firstname = @firstname, middlename = @middlename, lastname = @lastname, birthday = @birthday, bio = @bio " +
            "where email = @Email", connection);
            query.Parameters.AddWithValue("@firstname", user.firstName);
            query.Parameters.AddWithValue("@middlename", user.middleName);
            query.Parameters.AddWithValue("@lastname", user.lastName);
            query.Parameters.AddWithValue("@birthday", user.birthDay);
            query.Parameters.AddWithValue("@preference", user.preference);
            query.Parameters.AddWithValue("@Email", user.email);
            query.Parameters.AddWithValue("@bio", user.bio);
            //Execute query
            query.ExecuteNonQuery();
            //Close connection
            closeConnection();
        }
        catch (SqlException se)
        {

            Console.WriteLine(se.ToString());
            //Close connection
            closeConnection();
        }
    }

    public void registrationFunction(string firstname, string middlename, string lastname, string email, string preference, DateTime birthday, string gender,
                                 string bio, string password, byte[] proficePicture, bool active, string locatie, string opleiding)
    {
        openConnection();
        SqlCommand command = new SqlCommand("INSERT INTO Winder.Winder.[User](firstname, middlename, lastname, birthday, preference, email, password, gender, profilePicture, bio, active, location, education)" +
                       "VALUES('" + firstname + "', '" + middlename + "', '" + lastname + "', @birthday, '" + preference + "', '" + email + "', '" + password + "', '" + gender + "', @img, '" + bio +
                       "', @active, '" + locatie + "', '" + opleiding + "')", connection);
        command.Parameters.AddWithValue("@img", proficePicture);
        command.Parameters.AddWithValue("@active", active); 
        command.Parameters.AddWithValue("@birthday", birthday);
        try
        {
            command.ExecuteReader();

            //Close connection
            closeConnection();
        }
        catch (SqlException se)
        {

            Console.WriteLine(se.ToString());
            //Close connection
            closeConnection();
        }

    }

//>>>>>>> main
}