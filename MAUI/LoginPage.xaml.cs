using DataModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAUI;
public partial class LoginPage : ContentPage {
    Button b = new Button();
    Button b2 = new Button();

    Database database = new Database();

    public LoginPage() {
        InitializeComponent();
        b.Clicked += Inlog;
        b2.Clicked += WachtwoordVergeten;
    }

    private void Inlog(object sender, EventArgs e)
    {
        var Email = Emailadres.Text;
        var Password = Wachtwoord.Text;
        if (database.checkLogin(Email, Password)) {
            FoutmeldingInloggen.IsVisible = false;
            Navigation.PushAsync(new MainPage());
        }
        else {
            FoutmeldingInloggen.IsVisible = true;
        }
    }
    private void WachtwoordVergeten(object sender, EventArgs e) {
        
    }

    private void Backbutton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}