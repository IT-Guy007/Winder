namespace DataModel;

public class User
{

    public static User CurrentUser { get; set; }

    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDay { get; set; }
    public string Preference { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public byte[] ProfilePicture { get; set; }
    public string Bio { get; set; }
    public string School { get; set; }
    public string Major { get; set; }
    public string[] Interests { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }


    public User() { }

    public User(string firstName, string middleName, string lastName, DateTime birthDay, string preference, string email, string gender, byte[] profilePicture, string bio, string school, string major, string[] interests, int minAge, int maxAge)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        BirthDay = birthDay;
        Preference = preference;
        Email = email;
        Gender = gender;
        ProfilePicture = profilePicture;
        Bio = bio;
        School = school;
        Major = major;
        Interests = interests;
        MinAge = minAge;
        MaxAge = maxAge;
    }

}

