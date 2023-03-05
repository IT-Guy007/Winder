using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories;

public class LikedRepository : ILikedRepository
{
    private readonly IConfiguration _configuration;
    private const int UsersInQueueWhoLikedYou = 5;

    public LikedRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool CheckMatch(string emailLikedPerson, string emailCurrentUser)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                SqlCommand command =
                    new SqlCommand(
                        "SELECT * FROM Winder.Winder.[Liked] WHERE person = @emailLikedPerson AND likedPerson = @emailCurrentUser AND liked = 1",
                        connection);
                command.Parameters.AddWithValue("@emailLikedPerson", emailLikedPerson);
                command.Parameters.AddWithValue("@emailCurrentUser", emailCurrentUser);
                reader = command.ExecuteReader();
                if (reader.Read() && reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (SqlException se)
            {
                Console.WriteLine("Error checking match in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                return false;

            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
    }

    /// <summary>
    /// Deletes a like from the database
    /// </summary>
    /// <param name="emailCurrentUser">The current user</param>
    /// <param name="emailLikedUser">The second user</param>
    public bool DeleteLike(string emailLikedPerson, string emailCurrentUser)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM winder.winder.[Liked] " +
                                                    "WHERE person = @emailLikedUser AND likedPerson = @emailCurrentUser ",
                    connection);
                command.Parameters.AddWithValue("@emailLikedUser", emailLikedPerson);
                command.Parameters.AddWithValue("@emailCurrentUser", emailCurrentUser);
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqlException se)
            {
                Console.WriteLine("Error deleting like on match in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                return false;
            }
        }
    }

    /// <summary>
    /// Retrieves 5 users who liked you
    /// </summary>
    /// <param name="connection">The database connection</param>
    /// <returns>Queue with liked emails</returns>
    public Queue<string> GetUsersWhoLikedYou(string email)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            Queue<string> users = new Queue<string>();
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT TOP " + UsersInQueueWhoLikedYou +
                                                    " person FROM Winder.Winder.Liked " +
                                                    "WHERE likedPerson = @Email AND liked = 1 " + // selects the users that have liked the given user
                                                    "AND person NOT IN (SELECT likedPerson FROM Winder.Winder.Liked WHERE person = @Email) " + // except the ones that the given user has already disliked or liked
                                                    "ORDER BY NEWID()", connection);
                command.Parameters.AddWithValue("@Email", email);
                reader = command.ExecuteReader(); // execute het command
                while (reader.Read())
                {
                    string person = reader["person"] as string ?? "Unknown";
                    users.Enqueue(person); // zet elk persoon in de users 
                }

                reader.Close();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Error retrieving users who liked you from database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return users;
        }
    }

    public bool CreateMatch(string emailFirstPerson, string emailSecondPerson)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Winder.Winder.[Match] ()", connection);

                return true;
            }
            catch (SqlException se)
            {
                Console.WriteLine("Error inserting match into database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                return false;
            }
        }
    }

    public bool NewDislike(string emailLikedPerson, string emailCurrentUser)
    {
        using (SqlConnection connection =
               new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "INSERT INTO Winder.Winder.[Liked] (person, likedPerson, liked) " +
                    "VALUES (@currentUser, @likedUser, 0)", connection);
                command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
                command.Parameters.AddWithValue("@likedUser", emailLikedPerson);
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqlException se)
            {
                Console.WriteLine("Error inserting dislike in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                return false;
            }
        }
    }

    public bool NewLike(string emailLikedPerson, string emailCurrentUser)
    {
        using (SqlConnection connection =
               new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "INSERT INTO Winder.Winder.[Liked] (person, likedPerson, liked) " +
                    "VALUES (@currentUser, @likedUser, 1)", connection);
                command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
                command.Parameters.AddWithValue("@likedUser", emailLikedPerson);
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqlException se)
            {
                Console.WriteLine("Error inserting like in database");
                Console.WriteLine(se.ToString());
                Console.WriteLine(se.StackTrace);
                return false;
            }
        }
    }

}

