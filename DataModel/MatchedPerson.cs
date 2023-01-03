


using Microsoft.Maui.Controls;

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
        ProfilePicture = ImageSource.FromStream(() => ms);
    }
    
}