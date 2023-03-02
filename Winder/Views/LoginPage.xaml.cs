﻿using Controller;
using DataModel;
using MAUI;

namespace Winder;
public partial class LoginPage {
    private readonly Button loginButton = new Button();
    private readonly Button forgotPasswordButton = new Button();
    public LoginPage() {
        InitializeComponent();
        
        loginButton.Clicked += Login;

        forgotPasswordButton.Clicked += WachtwoordVergeten;

        loginButton.Clicked += Login;
        forgotPasswordButton.Clicked += WachtwoordVergeten;

    }

    private void Login(object sender, EventArgs e) {
        User loginUser = new User().CheckLogin(Emailadres.Text, Wachtwoord.Text, Database.ReleaseConnection);
        
        if (!string.IsNullOrEmpty(loginUser.Email)) {

            //Logging in went well
            FoutmeldingInloggen.IsVisible = false;

            //Set the CurrentUser to the user who logged in
            Authentication.CurrentUser = loginUser;

            //Bring the user to the swipe screen
            Navigation.PushAsync(new MatchPage());

        } else {
            FoutmeldingInloggen.IsVisible = true;
        }
    }
   
    private void WachtwoordVergeten(object sender, EventArgs e) {
        Navigation.PushAsync(new ForgotPasswordPage());

    }

    private void Backbutton_Clicked(object sender, EventArgs e) {
        Navigation.PushAsync(new StartPage());
    }
}