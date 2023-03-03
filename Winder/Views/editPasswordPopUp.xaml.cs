using Controller;
using DataModel;

namespace Winder;

public partial class EditPasswordPopUp {

    private readonly UserController _userController;

    public EditPasswordPopUp() {
        _userController = MauiProgram.ServiceProvider.GetService<UserController>();

        InitializeComponent();
	}

    private void CloseBtn(object sender, EventArgs e) {
        Close();
    }

    private void SaveBtn(object sender, EventArgs e)
    {
        string email = Authentication.CurrentUser.Email;
        string newPassword = NieuwWachtwoord.Text;
        string repeatPassword = HerhaalWachtwoord.Text;

        if (newPassword == null) {
            foutMelding.Text = "Wachtwoord mag niet leeg zijn!";
            foutMelding.IsVisible = true;

            // checkt of de 2 wachtwoorden gelijk zijn
        } else if (newPassword.Equals(repeatPassword)) {

            // checkt of het wachtwoord voldoet aan de eisen
            if (new User().CheckPassword(newPassword) == false) {
                foutMelding.Text = "Wachtwoord moet minimaal 8 karakters, 1 getal en 1 hoofdletter bevatten!";
                foutMelding.IsVisible = true;
            } else {
                _userController.UpdatePassword(email, newPassword);

                Close();
            }
        }

        else
        {
            foutMelding.Text = "Wachtwoorden komen niet overeen!";
            foutMelding.IsVisible = true;
        }

    }
        
} 
       
    
        
    
