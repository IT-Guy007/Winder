


using Microsoft.Maui.Controls;

namespace DataModel;

public class MatchedPerson {
    public string email { get; set; }
    private string firstName { get; set; }
    private string lastName { get; set; }
    private ImageSource profilePicture { get; set; }
    public MatchedPerson(User matchedStudent) {
        email = matchedStudent.email;
        firstName = matchedStudent.firstName;
        lastName = matchedStudent.lastName;
        MemoryStream memoryStream = new MemoryStream(matchedStudent.profilePicture);
        profilePicture = ImageSource.FromStream(() => memoryStream);
    }
    
}