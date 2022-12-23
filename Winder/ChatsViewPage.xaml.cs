using DataModel;
using MAUI;

namespace Winder;

public partial class ChatsViewPage : ContentPage
{
    public string OriginPage;
    Database Database = new Database();
    private const string pageName = "chatpage";
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

    public ChatsViewPage() {
        InitializeComponent();
        List<User> MatchedStudents = Database.GetMatchedStudentsFromUser(Authentication._currentUser.email);
        List<MatchedPerson> MatchedPeople = ConvertUserToMatchPerson(MatchedStudents);
        ListOfMatches.ItemsSource = MatchedPeople;
    }

    private List<MatchedPerson> ConvertUserToMatchPerson(List<User> MatchedStudents) {
        List<MatchedPerson> MatchedPeople = new List<MatchedPerson>();

        foreach (var student in MatchedStudents) {
            MatchedPeople.Add(new MatchedPerson(student));
        }

        return MatchedPeople;
    }


    private void Backbutton_Clicked(object sender, EventArgs e) {
        switch (OriginPage) {
            case "matchpage":
                Navigation.PushAsync(new MatchPage());
                break;
            case "profilepage":
                Navigation.PushAsync(new ProfileChange());
                break;
            case "settingspage":
                Navigation.PushAsync(new SettingsPage());
                break;
        }

    }

    private void ListOfMatches_ItemTapped(object sender, ItemTappedEventArgs e) {
        var tappedItem = e.Item as MatchedPerson;
        User sendToUser = Database.GetUserFromDatabase(tappedItem.Email);
        Navigation.PushAsync(new ChatView(Authentication._currentUser, sendToUser));
    }

    private void MyProfile_Clicked(object sender, EventArgs e) {
        ProfileChange myProfile = new ProfileChange();
        myProfile.OriginPage = pageName;
        Navigation.PushAsync(myProfile);
    }

    private void Settings_Clicked(object sender, EventArgs e) {
        SettingsPage settings = new SettingsPage();
        settings.OriginPage = pageName;
        Navigation.PushAsync(settings);
    }

    private void MatchPage_Clicked(object sender, EventArgs e) {
        MatchPage matchPage = new MatchPage();
        matchPage.OriginPage = pageName;
        Navigation.PushAsync(matchPage);
    }

}