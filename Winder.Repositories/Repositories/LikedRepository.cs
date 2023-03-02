using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories
{
    public class LikedRepository : ILikedRepository
    {
        private readonly IConfiguration _configuration;

        public LikedRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool CheckMatch(string emailLikedPerson, string emailCurrentUser)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Winder.Winder.[Liked] WHERE person = @emailLikedPerson AND likedPerson = @emailCurrentUser AND liked = 1", connection);
                command.Parameters.AddWithValue("@emailLikedPerson", emailLikedPerson);
                command.Parameters.AddWithValue("@emailCurrentUser", emailCurrentUser);

                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    reader.Read();
                    return reader.HasRows;

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

        public bool DeleteLike(string emailLikedPerson, string emailCurrentUser)
        {
            throw new NotImplementedException();
        }

        public List<string> GetLikes(string emailCurrentUser)
        {
            throw new NotImplementedException();
        }

        public bool NewDislike(string emailLikedPerson, string emailCurrentUser)
        {
            throw new NotImplementedException();
        }

        public bool NewLike(string emailLikedPerson, string emailCurrentUser)
        {
            throw new NotImplementedException();
        }
    }
}
