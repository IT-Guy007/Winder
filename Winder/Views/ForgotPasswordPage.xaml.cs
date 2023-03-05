using Controller;
using DataModel;
using EmailMessage = DataModel.EmailMessage;

namespace Winder;
public partial class ForgotPasswordPage {


    private readonly ValidationController _validationController;
    private readonly SettingsController _settingsController;
    private readonly EmailController _emailController;

    public ForgotPasswordPage() {
        _validationController = MauiProgram.ServiceProvider.GetService<ValidationController>();
        _settingsController = MauiProgram.ServiceProvider.GetService<SettingsController>();
        _emailController = MauiProgram.ServiceProvider.GetService<EmailController>();


        InitializeComponent();
    }

    private string authenticationCode;
    
    //Button to send Email
    private void SendEmailButton(object sender, EventArgs e) {
        
        string email = Emailadres.Text;
        
        //Check if Email exists
        if (_validationController.EmailIsUnique(email.ToLower())) {
            DisplayAlert("", "Dit emailadres is niet bekend bij ons", "OK"); // popup
            
        } else {

            //Send forgotten password Email
            string code = _validationController.RandomString();
        
            string body = "<h1>Authenticatie-code voor Winder</h1>" + 
                          "De authenthenticatie-code voor het resetten van het wachtwoord van uw Winder account is: <b>" + $"{code}</b>" +
                          "<br>Met vriendelijke groet, <br> Het Winder team";
        
            string subject = "Authenticatie-code";
            EmailMessage message = new EmailMessage(email, body, subject);
            _emailController.SendEmail(message);

            //Enables the rest of the page
            Emailadres.IsVisible = false;
            VerstuurEmail.IsVisible = false;
            Authenticatiecode.IsVisible = true;
            NieuwWachtwoord.IsVisible = true;
            HerhaalWachtwoord.IsVisible=true;
            ResetWachtwoord.IsVisible = true;

            DisplayAlert("", "Er is een Email verstuurd naar " + email.ToLower(), "OK"); // popup
            Emailadres.IsEnabled = false;  
        }
        

    }
    private void ResetPasswordButton(object sender, EventArgs e) {
        string email = Emailadres.Text;
        
        string authenticationText = Authenticatiecode.Text;
        string newPassword = NieuwWachtwoord.Text;
        string repeatedPassword = HerhaalWachtwoord.Text;

        //Checks if code is correct
        if (authenticationText.Equals(authenticationText)) {
            //Checks for null
            if (newPassword == null) {
                DisplayAlert("", "Wachtwoord mag niet leeg zijn", "OK"); // popup
            } else if (newPassword.Equals(repeatedPassword)) {
                
                //Checks for different requirements
                if (_validationController.CheckPassword(newPassword) == false) {
                        DisplayAlert("", "Wachtwoord moet minimaal 8 karakters, 1 getal en 1 hoofdletter bevatten", "OK"); // popup
                        
                } else {
                    User user = new User() {
                        Email = email
                    };
                    _settingsController.UpdatePassword(email, newPassword); 
                    
                    DisplayAlert("", "Wachtwoord is succesvol gewijzigd", "OK"); // popup
                    Navigation.PushAsync(new LoginPage()); // terug naar het loginscherm
                }

                
            } else {
                DisplayAlert("", "De wachtwoorden komen niet overeen", "OK"); // popup
            }
            
        } else {
                DisplayAlert("", "De AuthenticationCode is onjuist", "OK"); // popup
        }
       
    }



}