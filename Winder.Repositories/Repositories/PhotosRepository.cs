﻿using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories {
    internal class PhotosRepository : IPhotosRepository
    {
        IConfiguration _configuration;
        private const int MaxAmountOfPictures = 6;

        public PhotosRepository(IConfiguration configuration) {
            System.Diagnostics.Debug.WriteLine("PhotosRepository contructor called");
            _configuration = configuration;
        }
        
        public void AddPhoto(byte[] image, string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    string query =
                        "INSERT INTO winder.winder.Photos (winder.[user], winder.photo) VALUES(@Email, @profilepicture)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@profilepicture", image);

                    command.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error inserting picture in database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                }
            }
        }

        /// <summary>
        /// Delete all the photos from the database of the given user
        /// </summary>
        /// <param name="email">The email of the user</param>
        public void DeleteAllPhotos(string email)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    string query = "DELETE FROM winder.winder.Photos WHERE [user] = @Email";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);

                    command.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Error deleting pictures from database");
                    Console.WriteLine(se.ToString());
                    Console.WriteLine(se.StackTrace);
                }
            }
        }

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
