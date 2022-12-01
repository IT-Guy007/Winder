namespace DataModel;
using System.Drawing;
public class User {
    private string firstName { get; set; }
    private string middleName { get; set; }
    private string lastName { get; set; }
    private DateTime? birthDay { get; set; }
    private string preference { get; set; }
    private string email { get; set; }
    private string password { get; set; }
    public string gender { get; set; }
    private Bitmap profilePicture { get; set; }
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