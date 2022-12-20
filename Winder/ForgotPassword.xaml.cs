using DataModel;

namespace MAUI;
public partial class WijzigWachtwoordScherm : ContentPage
{
    

    public WijzigWachtwoordScherm()
    {
        InitializeComponent();
    }

    string authenticatiecode;
    //knop die de mail verstuurt
    private void VerstuurEmailknop(object sender, EventArgs e)
    {
        
        Authentication auth = new Authentication();
        var Email = Emailadres.Text;
        //check of het emailadres in de database staat
        if (auth.EmailIsUnique(Email.ToLower()))
        {
            DisplayAlert("", "Dit emailadres is niet bekend bij ons", "OK"); // popup
        }
        else // maakt de mail aan en verstuurt hem
        {
            
            authenticatiecode = Authentication.RandomString(6);  // maakt de authenticatiecode aan
            string body = "<h1>Authenticatie-code voor Winder</h1>" + 
                "De authenthenticatie-code voor het resetten van het wachtwoord van uw Winder account is: <b>" + $"{authenticatiecode}</b>" +
                "<br>Met vriendelijke groet,     Het Winder team";
            string subject = "Authenticatie-code";
            auth.SendEmail(Email, body, subject);

            //  maakt de rest van de pagina zichtbaar
            Emailadres.IsVisible = false;
            VerstuurEmail.IsVisible = false;
            Authenticatiecode.IsVisible = true;
            NieuwWachtwoord.IsVisible = true;
            HerhaalWachtwoord.IsVisible=true;
            ResetWachtwoord.IsVisible = true;

            DisplayAlert("", "Er is een email verstuurd naar " + Email.ToLower(), "OK"); // popup
            Emailadres.IsEnabled = false;   // zorgt ervoor dat de gebruiker niet meer kan typen in het emailadres veld
        }
        

    }
    private void ResetWachtwoordknop(object sender, EventArgs e)
    {
        Authentication auth = new Authentication();
        var Email = Emailadres.Text;
        
        var authenticatiecodeentry = Authenticatiecode.Text;
        var nieuwWachtwoord = NieuwWachtwoord.Text;
        var herhaalWachtwoord = HerhaalWachtwoord.Text;

        //check of de authenticatiecode klopt
        if (authenticatiecodeentry.Equals(authenticatiecode))
        {
            // checkt of er iets in is gevuld bij wachtwoord
            if (nieuwWachtwoord == null)
            {
                DisplayAlert("", "Wachtwoord mag niet leeg zijn", "OK"); // popup
            }
            else if (nieuwWachtwoord.Equals(herhaalWachtwoord)) // checkt of de 2 wachtwoorden gelijk zijn
                {

                   
                if (auth.CheckPassword(nieuwWachtwoord) == false) // checkt of het wachtwoord aan de eisen voldoet
                    {
                        DisplayAlert("", "Wachtwoord moet minimaal 8 karakters, 1 getal en 1 hoofdletter bevatten", "OK"); // popup
                    }


                else
                {
                    Database db = new Database();
                    
                    
                    db.UpdatePassword(Email, nieuwWachtwoord); 


                    
                  
                    DisplayAlert("", "Wachtwoord is succesvol gewijzigd", "OK"); // popup
                    Navigation.PushAsync(new LoginPage()); // terug naar het loginscherm
                }

                
            }
            
            else
            {
                DisplayAlert("", "De wachtwoorden komen niet overeen", "OK"); // popup
            }
            
        }
        else
            {
                DisplayAlert("", "De authenticatiecode is onjuist", "OK"); // popup
            }
       
    }



}