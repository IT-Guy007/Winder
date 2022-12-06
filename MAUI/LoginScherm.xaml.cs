using DataModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAUI;
public partial class LoginScherm : ContentPage {
    Button b = new Button();
    Button b2 = new Button();

    Database database = new Database();

    public LoginScherm() {
        InitializeComponent();
        
        loginButton.Clicked += Login;

        forgotPasswordButton.Clicked += WachtwoordVergeten;

        loginButton.Clicked += Login;
        forgotPasswordButton.Clicked += WachtwoordVergeten;
    }

    private void Inlog(object sender, EventArgs e)
    {
        var Email = Emailadres.Text;
        var Password = Wachtwoord.Text;
        if (database.checkLogin(Email, Password)) {
            FoutmeldingInloggen.IsVisible = false;
            Navigation.PushAsync(new ProfielOpmaak());
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