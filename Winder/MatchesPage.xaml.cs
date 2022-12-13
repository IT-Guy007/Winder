using MAUI;

namespace Winder;

public partial class MatchesPage : ContentPage
{
	public string originPage;
	public MatchesPage()
	{
		InitializeComponent();
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