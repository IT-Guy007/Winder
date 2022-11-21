namespace DataModel;
using System.Drawing;
public class User {
    private int UID { get; set; }
    private string username { get;}
    private string firstName { get;}
    private string middleName { get;}
    private string lastName { get;}
    private DateTime birthDay { get;}
    private string preference { get;}
    private string email { get; }
    private string password { get;}
    private string gender { get;}
    private Bitmap profilePicture { get;}

    public User(int UID, string username, string firstName, string middleName, string lastName, DateTime birthDay,
        string preference, string email, string password, string gender, Bitmap profilePicture)
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
    }
}