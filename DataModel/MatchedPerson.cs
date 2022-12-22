using Microsoft.Maui.Controls;
using static Microsoft.Maui.Controls.ImageSource;

namespace DataModel;

public class MatchedPerson {
    public string Email { get; set; }
    private string FirstName { get; set; }
    private string LastName { get; set; }
    private ImageSource ProfilePicture { get; set; }
    public MatchedPerson(User matchedStudent) {
        Email = matchedStudent.email;
        FirstName = matchedStudent.firstName;
        LastName = matchedStudent.lastName;
        MemoryStream ms = new MemoryStream(matchedStudent.profilePicture);
        ProfilePicture = FromStream(() => ms);
    }

}