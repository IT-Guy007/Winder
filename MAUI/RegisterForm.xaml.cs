using System.Drawing;
using DataModel;

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

    public RegisterForm() {
        InitializeComponent();
        
    }

    public void SaveEvent (object sender, EventArgs e)
    {
        int aantalchecks = 0;
        //Declaring objects by "Opslaan" button
        // checks
        #region voorkeur check
        if (Voorkeur.SelectedItem == null)
        {
            FoutVoorkeur.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            FoutVoorkeur.IsVisible = false;
            voorkeur = Voorkeur.ToString();
            aantalchecks += 1;
        }
        #endregion
        #region Opleiding check
        if (Opleiding.SelectedItem == null)
        {
            FoutOpleiding.IsVisible = true;
            aantalchecks-= 1;
        }
        else
        {
            FoutOpleiding.IsVisible= false;
            opleiding = Opleiding.ToString();
            aantalchecks += 1;
        }
        #endregion
        #region Locatie check
        if (Locatie.SelectedItem == null)
        {
            FoutLocatie.IsVisible = true;
            aantalchecks-= 1;
        }
        else
        {
            FoutLocatie.IsVisible= false;
            locatie = Locatie.ToString();
            aantalchecks+= 1;
        }
        #endregion
        #region interesses check
        if (Interesses.SelectedItem == null)
        {
            Foutinteresses.IsVisible = true;
            aantalchecks-= 1;
        }
        else
        {
            Foutinteresses.IsVisible= false;
            interesses = Interesses.ToString();
            aantalchecks+= 1;
        }
                
        #endregion



        

    }


    public void RegisterBtnEvent(object sender, EventArgs e)
    {
        int aantalchecks = 0;
        Authentication auth = new Authentication();
        //declaring objects by form values by clicking " registreer " button
        // also doing checks
        DateTime geboortedatumtijdelijk;
        geboortedatumtijdelijk = new DateTime(Geboortedatum.Date.Year, Geboortedatum.Date.Month, Geboortedatum.Date.Day);
        #region email checks
        if (Email.Text == null)
        {
            FoutEmail.Text= "Email mag niet leeg zijn";
            FoutEmail.IsVisible = true;
            aantalchecks -= 1;
        } else
        {
            if (auth.EmailIsUnique(Email.Text))
            {
                FoutEmail.IsVisible = false;
                email = Email.Text;
                aantalchecks += 1;
            }
            else
            {
                FoutEmail.Text = "Email is al in gebruik";
                FoutEmail.IsVisible = true;
                aantalchecks -= 1;
            }
            if (auth.CheckEmail(Email.Text))
            {
                email = Email.Text;
                aantalchecks += 1;
            }
            else
            {
                FoutEmail.IsVisible= true;
                FoutEmail.Text = "Email is niet van Windesheim";
                aantalchecks -= 1;
            }
        }


        #endregion

        #region voornaam check
        if (Voornaam.Text == null)
        {
            Foutvoornaam.IsVisible = true;
            aantalchecks -= 1;
        } else
        {
            Foutvoornaam.IsVisible = false;
            voornaam = Voornaam.Text;
            aantalchecks += 1;
        }
        #endregion

        #region achernaam check

        if (Achternaam.Text == null)
        {
            
            FoutAchternaam.IsVisible= true;
            aantalchecks -= 1;
        }
        else
        {
            FoutAchternaam.IsVisible = false;
            achternaam = Achternaam.Text;
            aantalchecks += 1;
        }

        #endregion

        #region wachtwoord check
        if (Wachtwoord.Text == null)
        {
            FoutWachtwoord.Text = "Wachtwoord mag niet leeg zijn";
            FoutWachtwoord.IsVisible= true;
              aantalchecks -= 1;
        }else
        {
            if (auth.CheckPassword(Wachtwoord.Text) == false)
            {
                FoutWachtwoord.Text = "Wachtwoord moet minimaal 8 karakters, 1 getal en 1 hoofdletter bevatten";
                FoutWachtwoord.IsVisible= true;
                aantalchecks -= 1;
            }
            else
            {
                FoutWachtwoord.IsVisible= false;
                wachtwoord = Wachtwoord.Text;
                aantalchecks += 1;
            }
           
            
            
        }
        #endregion

        #region geboortedatum checks
        
        if (auth.CalculateAge(geboortedatumtijdelijk) < 18)
        {
            FoutLeeftijd.IsVisible= true;
            aantalchecks -= 1;
        }else
        {
            FoutLeeftijd.IsVisible= false;
            geboortedatum = new DateTime(Geboortedatum.Date.Year, Geboortedatum.Date.Month, Geboortedatum.Date.Day);
            aantalchecks += 1;
        }
        #endregion

        #region geslacht check
        
        if (Geslacht.SelectedItem == null)
        {
            FoutGeslacht.IsVisible= true;
            aantalchecks -= 1;
        }
        else
        {
            FoutGeslacht.IsVisible= false;
            geslacht = Geslacht.SelectedItem.ToString();
            aantalchecks += 1;
        }
        #endregion


        Database database = new Database();
        Random random = new Random();
        if (database.register(voornaam, tussenvoegsel, achternaam, random.Next(100000, 999999).ToString(), email,
                voorkeur, geboortedatum,
                geslacht, "", wachtwoord, "", true))
        {
            Navigation.PushAsync(new MatchPage());
        }

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
            Geslacht.IsVisible = false;

            //Opslaan button visible
            Opslaan.IsVisible = true;
        }
    }