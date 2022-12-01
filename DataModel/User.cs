namespace DataModel;
using System.Drawing;
public class User {
    public string firstName { get; set; }
    public string middleName { get; set; }
    public string lastName { get; set; }
    public DateTime? birthDay { get; set; }
    public string preference { get; set; }
    public string email { get; set; }
    private string password { get; set; }
    public string gender { get; set; }
    public Image profilePicture { get; set; }
    public string bio { get; set; }
    public User(string firstName, string middleName, string lastName, DateTime birthDay,
        string preference, string email, string password, string gender, Bitmap profilePicture, string bio)
    {
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

    public User(string firstName, string middleName, string lastName, DateTime birthDay,
        string preference, string email, string password, string gender, string bio)
    {
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.birthDay = birthDay;
        this.preference = preference;
        this.email = email;
        this.password = password;
        this.gender = gender;
        this.bio = bio;
    }

}