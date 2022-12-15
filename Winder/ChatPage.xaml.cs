using MAUI;

namespace Winder;

public partial class ChatPage : ContentPage
{
	public string originPage;
	private const string pageName = "chatpage";
	public ChatPage()
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
			case "settingspage":
				Navigation.PushAsync(new Instellingen());
				break;
		}


	}

	private void MyProfile_Clicked(object sender, EventArgs e)
	{
		ProfileChange myProfile = new ProfileChange();
		myProfile.originPage = pageName;
		Navigation.PushAsync(myProfile);
	}
	private void Settings_Clicked(object sender, EventArgs e)
	{
		Instellingen settings = new Instellingen();
		settings.originPage = pageName;
		Navigation.PushAsync(settings);
	}

    private void MatchPage_Clicked(object sender, EventArgs e)
    {
        MatchPage matchPage = new MatchPage();
        matchPage.originPage = pageName;
        Navigation.PushAsync(matchPage);
    }


}