using DataModel;

namespace Winder;

public partial class ChatsViewPage {
    public string OriginPage;
    private readonly Database database;
	private const string PageName = "chatpage";

	public ChatsViewPage() {
	    this.database = new Database();
	    InitializeComponent();
        List<User> matchedStudents = database.GetMatchedStudentsFromUser(Authentication._currentUser.email);
        List<MatchedPerson> matchedPeople = ConvertUserToMatchPerson(matchedStudents);
        ListOfMatches.ItemsSource = matchedPeople;
        if (matchedStudents.Count < 1) {
            NoMatchDisplay.Text = "Je hebt helaas geen matches ;(";
            NoMatchDisplay.IsVisible= true;
        }
    }
    
    private List<MatchedPerson> ConvertUserToMatchPerson(List<User> matchedStudents) {
        List<MatchedPerson> matchedPeople = new List<MatchedPerson>();
        foreach (var student in matchedStudents)
        {
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
	    if (e.Item is MatchedPerson tappedItem) {
		    User user = database.GetUserFromDatabase(tappedItem.Email);
	    }
    }

    private void MyProfile_Clicked(object sender, EventArgs e) {
		ProfileChange profileChange = new ProfileChange {
			OriginPage = PageName
		};
		Navigation.PushAsync(profileChange);
	}
	private void Settings_Clicked(object sender, EventArgs e) {
		SettingsPage settings = new SettingsPage {
			OriginPage = PageName
		};
		Navigation.PushAsync(settings);
	}

    private void MatchPage_Clicked(object sender, EventArgs e) {
        MatchPage page = new MatchPage {
	        OriginPage = PageName
        };
        Navigation.PushAsync(page);
    }


}