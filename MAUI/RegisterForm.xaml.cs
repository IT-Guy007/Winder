



namespace MAUI;

public partial class RegisterForm : ContentPage
{
	private string email;
    private string voornaam;
    private string achternaam;
    private DateTime geboortedatum;
    private string geslacht;
    private string tussenvoegsel;
    private string wachtwoord;
    private string voorkeur;
    private string opleiding;
    private string locatie;
    private string interesses;






    public RegisterForm() 
    {
        InitializeComponent();
        
    }

    public void SaveEvent (object sender, EventArgs e)
    {
        //Declaring objects
        voorkeur = Voorkeur.ToString();
        opleiding = Opleiding.ToString();
        locatie = Locatie.ToString();
        interesses = Interesses.ToString();

    }


    public void RegisterBtnEvent(object sender, EventArgs e)
    {

        //declaring objects by form values
        email = Email.Text;
        voornaam = Voornaam.Text;
        tussenvoegsel = Tussenvoegsel.Text;
        achternaam = Achternaam.Text;
        wachtwoord = Wachtwoord.Text;
        geboortedatum = new DateTime(Geboortedatum.Date.Year, Geboortedatum.Date.Month, Geboortedatum.Date.Day);
        geslacht = Geslacht.SelectedItem.ToString();

        

        //setting objects visible to proceed the registerform
        LblVoorkeur.IsVisible = true;
        Voorkeur.IsVisible = true;
        LblOpleiding.IsVisible = true;
        Opleiding.IsVisible = true;
        LblLocatie.IsVisible = true;
        Locatie.IsVisible = true;
        LblInteresses.IsVisible = true;
        Interesses.IsVisible = true;


        // Making objects unvisible to proceed the registerform
        Email.IsVisible = false;
        Lblemail.IsVisible = false;
        Voornaam.IsVisible = false;
        LblVoornaam.IsVisible = false;
        Tussenvoegsel.IsVisible = false;
        LblTussenvoegsel.IsVisible = false;
        Achternaam.IsVisible = false;
        LblAchternaam.IsVisible = false;
        Wachtwoord.IsVisible = false;
        LblWachtwoord.IsVisible = false;
        Geboortedatum.IsVisible = false;
        LblGeboortedatum.IsVisible = false;
        Registreer.IsVisible = false;
        LblGeslacht.IsVisible = false;
        Geslacht.IsVisible=false;
        //Opslaan button visible
        Opslaan.IsVisible = true;
    }




}