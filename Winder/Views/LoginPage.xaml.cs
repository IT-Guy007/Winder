﻿using DataModel;
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
        var email = Emailadres.Text;
        var password = Wachtwoord.Text;
        User loginUser = new User().CheckLogin(email, password, Database2.ReleaseConnection);
        if (!string.IsNullOrEmpty(loginUser.Email)) {
            FoutmeldingInloggen.IsVisible = false;
            
            Navigation.PushAsync(new MatchPage());
        }
        else {
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