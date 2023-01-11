


using Microsoft.Maui.Controls;

namespace DataModel;

public class MatchedPerson {
    public string Email { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ImageSource ProfilePicture { get; set; }

    public MatchedPerson(User MatchedStudent) {
        Email = MatchedStudent.email;
        FirstName = MatchedStudent.firstName;
        LastName = MatchedStudent.lastName;
        MemoryStream ms = new MemoryStream(MatchedStudent.profilePicture);
        ProfilePicture = ImageSource.FromStream(() => ms);
    }

}