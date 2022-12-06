using DataModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAUI;
public partial class LoginPage : ContentPage {
    Button loginButton = new Button();
    Button forgotPasswordButton = new Button();

    Database database = new Database();

    public LoginPage() {
        InitializeComponent();
<<<<<<< HEAD
        b.Clicked += Inlog;
//<<<<<<< HEAD:MAUI/LoginScherm.xaml.cs
        
        
//=======
        b2.Clicked += WachtwoordVergeten;
//>>>>>>> main:MAUI/LoginPage.xaml.cs
=======
        loginButton.Clicked += Login;
        forgotPasswordButton.Clicked += WachtwoordVergeten;
>>>>>>> preMain
    }

    private void Login(object sender, EventArgs e)
    {
        var Email = Emailadres.Text;
        var Password = Wachtwoord.Text;
        if (database.CheckLogin(Email, Password)) {
            FoutmeldingInloggen.IsVisible = false;
            Navigation.PushAsync(new MatchPage());
        }
        else {
            FoutmeldingInloggen.IsVisible = true;
        }
    }
   
    private void WachtwoordVergeten(object sender, EventArgs e)
    {
        Navigation.PushAsync(new WijzigWachtwoordScherm());

    }

}