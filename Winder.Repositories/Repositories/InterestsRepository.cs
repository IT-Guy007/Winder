using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories;

public class InterestsRepository : IInterestsRepository
{
    private readonly IConfiguration _configuration;

    public InterestsRepository(IConfiguration configuration)
    {
        System.Diagnostics.Debug.WriteLine("InterestsRepository constructor called");
        _configuration = configuration;
    }

    /// <summary>
    /// Gets all the interests from the database
    /// </summary>
    /// <returns>Returns the interests in the form of a string list</returns>
    public List<string> GetInterests()
    {
        using (SqlConnection connection =
               new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            List<string> interests = new List<string>();

            string sql = "SELECT * FROM Winder.Winder.[Interests];";
            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = reader["name"] as string;
                    interests.Add(item);
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

            return interests;
        }
    }
}

