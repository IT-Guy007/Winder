using CommunityToolkit.Maui.Core.Extensions;
using DataModel;
using MAUI;
using System.Collections.ObjectModel;

namespace Winder;

public partial class MatchesPage : ContentPage
{
	public string originPage;
    Database Database = new Database();
	public MatchesPage()
	{
		InitializeComponent();
        List<User> MatchedStudents = Database.GetMatchesFromUser(Authentication._currentUser.email);
        xamlList.ItemsSource = MatchedStudents;
    }


    private void Backbutton_Clicked(object sender, EventArgs e)
	{

		
		switch (originPage)
		{
			case "matchpage":
				Navigation.PushAsync(new MatchPage());
				break;
			case "profilepage":
				Navigation.PushAsync(new ProfileChange());
				break;
			
		}


	}
}