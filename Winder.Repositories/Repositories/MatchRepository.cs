using DataModel;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly IConfiguration _configuration;

        public MatchRepository(IConfiguration configuration)
        {
            System.Diagnostics.Debug.WriteLine("MatchRepository constructor called");
            _configuration = configuration;
        }

        /// <summary>
        /// Adds match to user in the database
        /// </summary>
        /// <param name="emailLikedPerson">email address of person 1</param>
        /// <param name="emailCurrentUser">email address of person 2</param>
        /// <returns>Bool if succeeded</returns>
        public bool AddMatch(string emailLikedPerson, string emailCurrentUser)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand command = new SqlCommand("INSERT INTO winder.winder.[Match] (person1, person2) VALUES (@currentUser, @likedUser)", connection);
                command.Parameters.AddWithValue("@currentUser", emailCurrentUser);
                command.Parameters.AddWithValue("@likedUser", emailLikedPerson);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error inserting match in database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the matched of the given User
        /// </summary>
        /// <param name="user">The user of who the matches need to retrieved for</param>
        /// <returns>List of emails from with who a match is</returns>
        public List<string> GetMatchedStudentsFromUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                List<string> emails = new List<string>();
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    string query = "SELECT person1, person2 FROM Winder.Winder.Match WHERE person1 = @Email OR person2 = @Email";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string person1 = reader["person1"] as string ?? "Unknown";
                        string person2 = reader["person2"] as string ?? "Unknown";
                        if (person1 == user.Email)
                        {
                            emails.Add(person2);
                        }
                        else
                        {
                            emails.Add(person1);
                        }
                    }

                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error retrieving matches from database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                return emails;
            }
        }
    }
}