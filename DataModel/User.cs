namespace DataModel;
using System.Drawing;
public class User {
    public string username { get; set; }
    public string firstName { get; set; }
    public string middleName { get; set; }
    public string lastName { get; set; }
    public DateTime? birthDay { get; set; }
    public string preference { get; set; }
    private string email { get; set; }
    private string password { get; set; }
    public string gender { get; set; }
    private Bitmap profilePicture { get; set; }

    public string bio { get; set; }

    public User(string username, string firstName, string middleName, string lastName, DateTime? birthDay,
        string preference, string email, string password, string gender, Bitmap profilePicture, string bio)
    {
        this.username = username;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.birthDay = birthDay;
        this.preference = preference;
        this.email = email;
        this.password = password;
        this.gender = gender;
        this.profilePicture = profilePicture;
        this.bio = bio;
    }

}