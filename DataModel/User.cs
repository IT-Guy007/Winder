namespace DataModel;
using System.Drawing;
public class User {
    private int UID { get; set; }
    private string username { get; set; }
    private string firstName { get; set; }
    private string middleName { get; set; }
    private string lastName { get; set; }
    private DateTime birthDay { get; set; }
    private string preference { get; set; }
    private string email { get; set; }
    private string password { get; set; }
    private string gender { get; set; }
    private Image profilePicture { get; set; }
    
    private string bio { get; set; }

    public User(int UID, string username, string firstName, string middleName, string lastName, DateTime birthDay,
        string preference, string email, string password, string gender, Image profilePicture, string bio)
    {
        this.UID = UID;
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

    public User() {
        
    }
}