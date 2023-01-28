using Microsoft.Maui.Controls;

namespace DataModel;

public class MatchedPerson {
    public string Email { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ImageSource ProfilePicture { get; set; }

    public MatchedPerson(User MatchedStudent) {
        Email = MatchedStudent.Email;
        FirstName = MatchedStudent.FirstName;
        LastName = MatchedStudent.LastName;
        MemoryStream ms = new MemoryStream(MatchedStudent.ProfilePicture);
        ProfilePicture = ImageSource.FromStream(() => ms);
    }

}