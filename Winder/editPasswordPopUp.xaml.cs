using DataModel;


namespace Winder;

public partial class EditPasswordPopUp {
    
    public EditPasswordPopUp() {
        InitializeComponent();
	}

    private void CloseBtn(object sender, EventArgs e) {
        Close();
    }

    private void SaveBtn(object sender, EventArgs e)
    {
        Authentication auth = new Authentication();
        string newPassword = NieuwWachtwoord.Text;
        string repeatPassword = HerhaalWachtwoord.Text;
        string email = Authentication._currentUser.email;

        if (newPassword == null) {
            foutMelding.Text = "Wachtwoord mag niet leeg zijn!";
            foutMelding.IsVisible = true;

            // checkt of de 2 wachtwoorden gelijk zijn
        } else if (newPassword.Equals(repeatPassword)) {

            // checkt of het wachtwoord voldoet aan de eisen
            if (auth.CheckPassword(newPassword) == false) {
                foutMelding.Text = "Wachtwoord moet minimaal 8 karakters, 1 getal en 1 hoofdletter bevatten!";
                foutMelding.IsVisible = true;
            } else {
                Database db = new Database();
                
                db.UpdatePassword(email, newPassword);

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
       
    
        
    
