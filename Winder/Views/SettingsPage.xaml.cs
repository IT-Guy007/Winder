using CommunityToolkit.Maui.Views;
using Controller;
using DataModel;
using MAUI;

namespace Winder;

public partial class SettingsPage
{
    private const string PageName = "settingspage";
    public string OriginPage;

    private readonly SettingsController _settingsController;

    public SettingsPage()
    {
        _settingsController = MauiProgram.ServiceProvider.GetService<SettingsController>();

        InitializeComponent();

        Location.SelectedItem = User.CurrentUser.School;   //aa
        minimaleLeeftijd.SelectedItem = User.CurrentUser.MaxAge; // aa
        maximaleLeeftijd.SelectedItem = User.CurrentUser.MaxAge; // aa


        minimaleLeeftijd.ItemsSource = _settingsController.GetPickerData();
        maximaleLeeftijd.ItemsSource = _settingsController.GetPickerData();

    }

    /// <summary>
    /// Delete the account button
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private async void DeleteAccountButton(object sender, EventArgs e)
    {
        if (!await DisplayAlert("", "Weet u zeker dat u uw account wilt verwijderen?", "Ja", "Nee")) return;
        _settingsController.DeleteAccount();
        await Navigation.PushAsync(new MainPage());
    }

    /// <summary>
    /// Logout button
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private async void LogoutButton(object sender, EventArgs e)
    {
        if (!await DisplayAlert("", "U wordt uitgelogd", "Ok", "Annuleren")) return;
        _settingsController.Logout();
        await Navigation.PushAsync(new StartPage());
    }


    /// <summary>
    /// Edit data button, this will set the preferences the data of the user
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void EditDataButton(object sender, EventArgs e)
    {
        if (minimaleLeeftijd.SelectedItem == null || maximaleLeeftijd.SelectedItem == null ||
            (int)minimaleLeeftijd.SelectedItem > (int)maximaleLeeftijd.SelectedItem)
        {
            foutLeeftijd.IsVisible = true;
        }
        else
        {
            _settingsController.SetPreference((int)minimaleLeeftijd.SelectedItem, (int)maximaleLeeftijd.SelectedItem, Location.SelectedItem.ToString());
            User.CurrentUser.MinAge = (int)minimaleLeeftijd.SelectedItem;
            User.CurrentUser.MaxAge = (int)maximaleLeeftijd.SelectedItem;
            User.CurrentUser.School = Location.SelectedItem.ToString();

            foutLeeftijd.IsVisible = false;
            DisplayAlert("Melding", "Er zijn succesvol gegevens aangepast", "OK");
        }

    }

    /// <summary>
    /// The edit password button
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void EditPasswordBtn(object sender, EventArgs e)
    {
        this.ShowPopup(new EditPasswordPopUp());
    }

    /// <summary>
    /// The profile clicked button, pushed new Profile page
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The eventargs</param>
    private void MyProfile_Clicked(object sender, EventArgs e)
    {
        ProfileChange myProfile = new ProfileChange();

        myProfile.OriginPage = PageName;
        Navigation.PushAsync(myProfile);

    }

    /// <summary>
    /// The back button, takes the user to the previous page
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void Backbutton_Clicked(object sender, EventArgs e)
    {
        switch (OriginPage)
        {
            case "matchpage":
                MatchPage page = new MatchPage
                {
                    BackButtonVisible = true
                };
                Navigation.PushAsync(page);
                break;
            case "profilepage":
                Navigation.PushAsync(new ProfileChange());
                break;
            case "chatpage":
                Navigation.PushAsync(new ChatsViewPage());
                break;
        }

    }

    /// <summary>
    /// The chat button in the header, pushes a new ChatsViewPage
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The eventargs</param>
    private void ChatButton_Clicked(object sender, EventArgs e)
    {
        ChatsViewPage chatsViews = new ChatsViewPage
        {
            OriginPage = PageName
        };
        Navigation.PushAsync(chatsViews);

    }

    /// <summary>
    /// The match button in the header, pushes a new MatchPage
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The object</param>
    private void matchPage_Clicked(object sender, EventArgs e)
    {
        MatchPage page = new MatchPage
        {
            OriginPage = PageName
        };
        Navigation.PushAsync(page);

    }

}