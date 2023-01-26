using DataModel;
using MAUI;

namespace Winder;

public partial class ChatsViewPage {
    public string OriginPage;
    private Database Database;
    private const string pageName = "Chatpage";

    public ChatsViewPage() {
        Database = new Database();
        InitializeComponent();
        
        List<User> MatchedStudents = Database.GetMatchedStudentsFromUser(Authentication._currentUser.email);
        List<MatchedPerson> MatchedPeople = ConvertUserToMatchPerson(MatchedStudents);
        ListOfMatches.ItemsSource = MatchedPeople;
    }

    private List<MatchedPerson> ConvertUserToMatchPerson(List<User> MatchedStudents) {
        List<MatchedPerson> matchedPeople = new List<MatchedPerson>();

        foreach (var student in MatchedStudents) {
            matchedPeople.Add(new MatchedPerson(student));
        }

        return matchedPeople;
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
        Navigation.PushAsync(new ChatPage(Authentication._currentUser, Database.GetUserFromDatabase(tappedItem.Email)));
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