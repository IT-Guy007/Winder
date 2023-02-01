using System.Data.SqlClient;

namespace Controller;

public class SwipeController {


    /// <summary>
    /// Checks in the database if there is a match between the current user and the liked person
    /// </summary>
    /// <param name="emailCurrentUser">email of the currentUser</param>
    /// <param name="emailLikedPerson">email of the likedUser</param>
    /// <param name="connection">The databaseconnection</param>
    /// <returns></returns>
    SqlDataReader reader = null;
    public bool CheckMatch(string emailCurrentUser, string emailLikedPerson, SqlConnection connection) {

        SqlCommand command = new SqlCommand("SELECT * FROM Winder.Winder.[Liked] WHERE person = @emailLikedPerson AND likedPerson = @emailCurrentUser AND liked = 1", connection);
        command.Parameters.AddWithValue("@emailLikedPerson", emailLikedPerson);
        command.Parameters.AddWithValue("@emailCurrentUser", emailCurrentUser);

        try {
            reader = command.ExecuteReader();

            reader.Read();
            return reader.HasRows;

        } catch (SqlException se) {
            Console.WriteLine("Error checking match in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
            return false;

        } finally  {
            if (reader != null) reader.Close();
        }
    }

    /// <summary>
    /// Adds a like to the database
    /// </summary>
    /// <param name="emailCurrentUser">Email of the current user</param>
    /// <param name="emailLikedPerson">Email of the liked user</param>
    /// <param name="connection">The database connection</param>
    public void NewLike(string emailCurrentUser, string emailLikedPerson, SqlConnection connection) {
        //There is no match yet
        SqlCommand command = new SqlCommand("INSERT INTO Winder.Winder.[Liked] (person, likedPerson, liked) " +
                                            "VALUES (@currentUser, @likedUser, 1)", connection);
        command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
        command.Parameters.AddWithValue("@likedUser", emailLikedPerson);

        try {
            command.ExecuteNonQuery();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting like in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);

        }
    }
    
    /// <summary>
    /// Adds a dislike to the database
    /// </summary>
    /// <param name="emailCurrentUser">Email of the current user</param>
    /// <param name="emailLikedPerson">Email of the liked user</param>
    /// <param name="connection">The database connection</param>
    public void NewDislike(string emailCurrentUser, string emailLikedPerson,SqlConnection connection) {
        //There is no match yet
        SqlCommand command = new SqlCommand("INSERT INTO Winder.Winder.[Liked] (person, likedPerson, liked) " +
                                            "VALUES (@currentUser, @likedUser, 0)", connection);
        command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
        command.Parameters.AddWithValue("@likedUser", emailLikedPerson);

        try {
            command.ExecuteNonQuery();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting dislike in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
    }
    
    /// <summary>
    /// Creates a new match in the database
    /// </summary>
    /// <param name="emailCurrentUser">Email of the current user</param>
    /// <param name="emailLikedPerson">Email of the liked person</param>
    /// <param name="connection">The database connection</param>
    public void NewMatch(string emailCurrentUser, string emailLikedPerson, SqlConnection connection) {

        SqlCommand command = new SqlCommand("INSERT INTO winder.winder.[Match] (person1, person2) " +
                                            "VALUES (@currentUser, @likedUser)", connection);
        command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
        command.Parameters.AddWithValue("@likedUser", emailLikedPerson);

        try {
            command.ExecuteNonQuery();
        } catch (SqlException se) {
            Console.WriteLine("Error inserting match in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);
        }
    }
    
    /// <summary>
    /// Deletes a like from the database
    /// </summary>
    /// <param name="emailCurrentUser">The current user</param>
    /// <param name="emailLikedUser">The second user</param>
    public void DeleteLike(string emailCurrentUser, string emailLikedUser, SqlConnection connection) {


        SqlCommand command = new SqlCommand("DELETE FROM winder.winder.[Liked] " +
                                            "WHERE person = @emailLikedUser AND likedPerson = @emailCurrentUser ", connection);
        command.Parameters.AddWithValue("@emailLikedUser", emailLikedUser);
        command.Parameters.AddWithValue("@emailCurrentUser", emailCurrentUser);

        try {
            command.ExecuteNonQuery();

        } catch (SqlException se) {
            Console.WriteLine("Error deleting like on match in database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);

        }
    }
}