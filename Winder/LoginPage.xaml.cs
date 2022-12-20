using DataModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAUI;
public partial class LoginPage : ContentPage {
    Button loginButton = new Button();
    Button forgotPasswordButton = new Button();

    Database database = new Database();

    public LoginPage() {
        InitializeComponent();
        
        loginButton.Clicked += Login;
        forgotPasswordButton.Clicked += WachtwoordVergeten;

    }

    private void Login(object sender, EventArgs e) {
        var Email = Emailadres.Text;
        var Password = Wachtwoord.Text;
        Console.WriteLine(Email);
        if (database.CheckLogin(Email, Password)) {

            //Set the first profile
            Authentication.CheckIfQueueNeedsMoreProfiles();
            if (Authentication._profileQueue.Count != 0) {
                try {
                    Authentication._currentProfile = Authentication._profileQueue.Dequeue();

                } catch (Exception ex) {
                    //No profiles found
                    Console.WriteLine("Couldn't find a new profile");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(ex.StackTrace);

                }

                Authentication.selectedImage = 0;
                Navigation.PushAsync(new MatchPage());

            } else {
                Console.WriteLine("Couldn't find a new profile");
                Authentication._currentProfile = null;
                Navigation.PushAsync(new MatchPage());
            
            }
        }
        else {
            FoutmeldingInloggen.IsVisible = true;
        }
    }
   
    private void WachtwoordVergeten(object sender, EventArgs e) {
        Navigation.PushAsync(new WijzigWachtwoordScherm());

    }

    private void Backbutton_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new StartPage());

    }
}