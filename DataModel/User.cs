namespace DataModel;

public class User {
    public string firstName { get; set; }
    public string middleName { get; set; }
    public string lastName { get; set; }
    public DateTime birthDay { get; set; }
    public string preference { get; set; }
    public string email { get; set; }
    private string password { get; set; }
    public string gender { get; set; }
    public byte[] profilePicture { get; set; }
    public string bio { get; set; }
    public string school { get; set; }
    public string major { get; set; }
    public string[] interests { get; set; }

    public User(string firstName, string middleName, string lastName, DateTime birthDay,
        string preference, string email, string password, string gender, byte[] profilePicture, string bio, string school, string major)
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
        this.school = school;
        this.major = major;
    }
    
    public User(){}
    
}