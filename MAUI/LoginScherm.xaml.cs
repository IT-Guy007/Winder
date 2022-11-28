


using DataModel;
using Microsoft.Maui.Layouts;

namespace MAUI;
public partial class LoginScherm : ContentPage
{
    Button b = new Button();
    Button b2 = new Button();
   
    public LoginScherm()
    {
        InitializeComponent();
        b.Clicked += Inlog;
        b2.Clicked += WachtwoordVergeten;
        
    }
    private void Inlog(object sender, EventArgs e)
    {
        Database database = new Database();
        var Email = Emailadres.Text.Trim();
        var Password = Wachtwoord.Text.Trim();
        if (database.checkLogin(Email, Password))
        {
            FoutmeldingInloggen.IsVisible = false;
            Navigation.PushAsync(new ProfielOpmaak());
        }
        else
        {
            FoutmeldingInloggen.IsVisible = true;
        }
    }
    private void WachtwoordVergeten(object sender, EventArgs e)
    {
        
    }
   
}