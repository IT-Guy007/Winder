
using CommunityToolkit.Maui.Views;
using DataModel;


namespace Winder;

public partial class editPasswordPopUp : Popup
{
    
    public editPasswordPopUp()
	{
       
        InitializeComponent();
	}

    public void closeBtn(object sender, EventArgs e)
    {
        Close();
    }

    public void saveBtn(object sender, EventArgs e)
    {
        Authentication auth = new Authentication();
        var nieuwWachtwoord = NieuwWachtwoord.Text;
        var herhaalWachtwoord = HerhaalWachtwoord.Text;
        string Email = auth._currentUser.email;

        if (nieuwWachtwoord == null)
        {
            foutMelding.Text = "Wachtwoord mag niet leeg zijn!";
            foutMelding.IsVisible = true;

        }
        else if (nieuwWachtwoord.Equals(herhaalWachtwoord)) // checkt of de 2 wachtwoorden gelijk zijn
        {


            if (auth.CheckPassword(nieuwWachtwoord) == false) // checkt of het wachtwoord aan de eisen voldoet
            {
                //DisplayAlert("", "Wachtwoord moet minimaal 8 karakters, 1 getal en 1 hoofdletter bevatten", "OK"); // popup
            }


            else
            {
                Database db = new Database();


                db.UpdatePassword(Email, nieuwWachtwoord);




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
       
    
        
    
