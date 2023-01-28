using DataModel;
using MAUI;

namespace Winder;

public partial class ChatsViewPage {
    public string OriginPage;
    private const string pageName = "Chatpage";
    
    public ChatsViewPage() {
        InitializeComponent();

        List<User> MatchedStudents = Authentication.CurrentUser.GetMatchedStudentsFromUser(Database2.ReleaseConnection);
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
        Navigation.PushAsync(new ChatPage(Authentication.CurrentUser, new User().GetUserFromDatabase(tappedItem.Email, Database2.ReleaseConnection)));
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