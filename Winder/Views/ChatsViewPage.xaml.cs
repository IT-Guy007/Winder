using Controller;
using DataModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using Winder.Repositories;
using Winder.Repositories.Interfaces;

namespace Winder;

public partial class ChatsViewPage {
    public string OriginPage;
    private const string pageName = "Chatpage";

    private readonly UserController _userController;

    private MatchModel MatchModel;
    
    public ChatsViewPage() {
        InitializeComponent();
        this._userController = MauiProgram.ServiceProvider.GetService<UserController>();

        MatchModel = new MatchModel(Authentication.CurrentUser.GetMatchedStudentsFromUser(Database.ReleaseConnection));
        ListOfMatches.ItemsSource = MatchModel.Matches.GetUsers();
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
        var tappedItem = e.Item as User;
        Navigation.PushAsync(new ChatPage(Authentication.CurrentUser, tappedItem));
    }

    /// <summary>
    /// The profile button clicked in the header
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void MyProfile_Clicked(object sender, EventArgs e) {
        ProfileChange myProfile = new ProfileChange();
        myProfile.OriginPage = pageName;
        Navigation.PushAsync(myProfile);
    }

    /// <summary>
    /// The settings button clicked in the header
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void Settings_Clicked(object sender, EventArgs e) {
        SettingsPage settings = new SettingsPage();
        settings.OriginPage = pageName;
        Navigation.PushAsync(settings);
    }

    /// <summary>
    /// Match button clicked in the header
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void MatchPage_Clicked(object sender, EventArgs e) {
        MatchPage matchPage = new MatchPage();
        matchPage.OriginPage = pageName;
        Navigation.PushAsync(matchPage);
    }

}