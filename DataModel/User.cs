

using System.Data.SqlClient;

namespace DataModel;

public class User {
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDay { get; set; }
    public string Preference { get; set; }
    public string Email { get; set; }
    private string Password { get; set; }
    public string Gender { get; set; }
    public byte[] ProfilePicture { get; set; }
    public string Bio { get; set; }
    public string School { get; set; }
    public string Major { get; set; }
    public string[] Interests { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    
        
    private const int MinAgePreference = 18;
    private const int MaxAgePreference = 99;
    private static DateTime MinDateTimeBirth = new DateTime(1925, 01, 01, 0, 0, 0, 0);
    

    public User(string firstName, string middleName, string lastName, DateTime birthDay,
        string preference, string email, string password, string gender, byte[] profilePicture, string bio, string school, string major, int minAge, int maxAge) {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        BirthDay = birthDay;
        Preference = preference;
        Email = email;
        Password = password;
        Gender = gender;
        ProfilePicture = profilePicture;
        Bio = bio;
        School = school;
        Major = major;
        MinAge = minAge;
        MaxAge = maxAge;

    }
    
    public User(){}
    
    public User GetUserFromDatabase(string email, SqlConnection connection) {

        string query = "SELECT * FROM Winder.Winder.[User] where email = @Email";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);
        
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                Email = reader["email"] as string ?? string.Empty;
                FirstName = reader["firstname"] as string ?? string.Empty;
                MiddleName = reader["middlename"] as string ?? string.Empty;
                LastName = reader["lastname"] as string ?? string.Empty;
                Preference = reader["preference"] as string ?? string.Empty;
                Gender = reader["gender"] as string ?? string.Empty;
                BirthDay = reader["birthday"] as DateTime? ?? MinDateTimeBirth;
                Bio = reader["bio"] as string ?? string.Empty;
                School = reader["location"] as string ?? string.Empty;
                Major = reader["education"] as string ?? string.Empty;
                ProfilePicture = (byte[])(reader["profilePicture"]);
                var minAge = reader["min"] as int?;
                var maxAge = reader["max"] as int?;

                MinAge = minAge ?? MinAgePreference;
                MaxAge = maxAge ?? MaxAgePreference;
                
            }
        } catch (SqlException e) {
            Console.WriteLine("Error retrieving user from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);

        }

        return this;
    }

}

