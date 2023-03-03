using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Storage;


namespace DataModel;

public class UserModel {
    
    public readonly string EmailStartsWith = "s";
    public readonly string EmailEndsWith = "@student.windesheim.nl";
    
    
    /// <summary>
    /// Checks if the email is unique in the database
    /// </summary>
    /// <param name="email">The email to check if it is unique</param>
    /// <param name="connection">The database connection to check on</param>
    /// <returns>A bool if it is unique</returns>
    public bool EmailIsUnique(string email, SqlConnection connection) {
        List<string> emails = new List<string>();
        
        string sql = "SELECT Email FROM Winder.Winder.[User];";
        SqlCommand command = new SqlCommand(sql, connection);
        
        SqlDataReader reader = null;
        try {
            reader = command.ExecuteReader();
            while (reader.Read()) {
                var item = reader["Email"] as string;
                emails.Add(item);
            }

        } catch (SqlException e) {
            Console.WriteLine("Error getting emails from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);

        } finally  {
            if (reader != null) reader.Close();
        }

        if (emails.Contains(email)) {
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Hashed a plain string to a hashed string
    /// </summary>
    /// <param name="password">The plain string to hash</param>
    /// <returns>The hashed string</returns>
    public string HashPassword(string password) {
        if (!string.IsNullOrEmpty(password)) {
            string result = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
            return result;
        }
        return "";
    }
    
        
    /// <summary>
    /// Sets the login email in the secure storage
    /// </summary>
    /// <param name="email">The email to set</param>
    public async Task SetLoginEmail(string email) {
        Console.WriteLine("Setting login Email");
        await SecureStorage.SetAsync("Email", email);
        
    }


    /// <summary>
    /// Checks if the email is from Windesheim student
    /// </summary>
    /// <param name="email">The given email</param>
    /// <returns></returns>
    public bool CheckEmail(string email) {
        return email.EndsWith(EmailEndsWith) && email.StartsWith(EmailStartsWith);
    }

}