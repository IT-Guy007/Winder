using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories {
    public class PhotosRepository : IPhotosRepository
    {
        private readonly IConfiguration _configuration;
        private const int MaxAmountOfPictures = 6;

        public PhotosRepository(IConfiguration configuration) {
            System.Diagnostics.Debug.WriteLine("PhotosRepository constructor called");
            _configuration = configuration;
        }
        
        /// <summary>
        /// Adds photo to user account in the database
        /// </summary>
        /// <param name="image">The image to add</param>
        /// <param name="email">The email from the user</param>
        /// <returns>bool if succeed</returns>
        public bool AddPhoto(byte[] image, string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO winder.winder.Photos (winder.[user], winder.photo) VALUES(@Email, @profilepicture)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@profilepicture", image);

                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error inserting picture in database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete all the photos from the database of the given user
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>Bool if succeeded</returns>
        public bool DeleteAllPhotos(string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM winder.winder.Photos WHERE [user] = @Email";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);

                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error deleting pictures from database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the photos for the user
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>Bool if succeeded</returns>
        public byte[][] GetPhotos(string email) {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))) 
            {
                byte[][] result = new byte[MaxAmountOfPictures][];

                //Create query
                SqlCommand query = new SqlCommand("SELECT * FROM winder.Photos WHERE [user] = @Email", connection);
                query.Parameters.AddWithValue("@Email", email);

                //Execute query
                SqlDataReader reader = null;
                try {
                    connection.Open();
                    reader = query.ExecuteReader();
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

                } finally  {
                    if (reader != null) reader.Close();
                }

                return result;
            }
        }
    }
}
