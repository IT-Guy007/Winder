
using System.Buffers.Text;
using System.Drawing;
using DataModel;


namespace MAUI;

public partial class RegisterPage : ContentPage
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
    private byte[] profielfoto;

    Database database = new Database();
    private List<string> interesseslist = new List<string>();
    private List<string> GekozenInteressesLijst = new List<string>();



    public RegisterPage() {

        InitializeComponent();
        interesseslist = database.GetInterestsFromDataBase();
        foreach (string interest in interesseslist)
        {
            Interesses.Items.Add(interest);
        }
    }


    //Gebruiker verwijdert een " interesse " uit de selectie door erop te klikken
    public void Gekozeninteresses_ItemSelected(object sender, EventArgs e)
    {
        if (Gekozeninteresses.SelectedItem != null)
        {
            GekozenInteressesLijst.Remove(Gekozeninteresses.SelectedItem.ToString());

        }
        Gekozeninteresses.ItemsSource = null;
        Gekozeninteresses.IsVisible = false;
        if (GekozenInteressesLijst.Count > 0)
        {
            Gekozeninteresses.IsVisible = true;
            Gekozeninteresses.ItemsSource = GekozenInteressesLijst;
        }



    }

    // Voegt geselecteerde items toe aan listbox zodat de gebruiker zijn selectie kan zien
    public void OnSelectedItems(object sender, EventArgs e)
    {
        if (GekozenInteressesLijst.Count() < 5 && Interesses.SelectedItem != null)
        {
            if (GekozenInteressesLijst.Contains(Interesses.SelectedItem.ToString()))
            {
                Foutinteresses.Text = "interesse is al toegevoegd";
                Foutinteresses.IsVisible = true;
            }
            else
            {
                Foutinteresses.IsVisible = false;
                GekozenInteressesLijst.Add(Interesses.SelectedItem.ToString());
                Gekozeninteresses.ItemsSource = null;
                Gekozeninteresses.ItemsSource = GekozenInteressesLijst;
                Gekozeninteresses.IsVisible = true;

            }
        }

        Gekozeninteresses.ItemsSource = GekozenInteressesLijst;
        Gekozeninteresses.IsVisible = true;
    }
    //Declaring objects by "Opslaan" button
    // checks
    public bool SaveEventChecks()
    {
        int aantalchecks = 0;
        
        #region voorkeur check
        if (Voorkeur.SelectedItem == null)
        {
            FoutVoorkeur.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            FoutVoorkeur.IsVisible = false;
            voorkeur = Voorkeur.SelectedItem.ToString();
            aantalchecks += 1;
        }
        #endregion
        #region Profielfotocheck
        if (ProfileImage.Source == null)
        {
            FoutProfielfoto.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            FoutProfielfoto.IsVisible = false;
            aantalchecks += 1;
        }
        #endregion
        #region Opleiding check
        if (Opleiding.Text == null)
        {
            FoutOpleiding.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            FoutOpleiding.IsVisible = false;
            opleiding = Opleiding.Text.ToString();
            aantalchecks += 1;
        }
        #endregion
        #region Locatie check
        if (Locatie.SelectedItem == null)
        {
            FoutLocatie.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            FoutLocatie.IsVisible = false;
            locatie = Locatie.SelectedItem.ToString();
            aantalchecks += 1;
        }
        #endregion
        #region interesses check
        if (Interesses.SelectedItem == null)
        {
            Foutinteresses.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            Foutinteresses.IsVisible = false;
            Interesses.ItemsSource = interesseslist;
            aantalchecks += 1;
        }
        #endregion

        if (aantalchecks == 5)
        {
            return true;
        }else
        {
            return false;
        }

    }

    public void SaveEvent (object sender, EventArgs e)
    {

        if (SaveEventChecks())
        {

            
            if (tussenvoegsel == null)
            {
                tussenvoegsel = "";
            }
             database.registrationFunction(voornaam, tussenvoegsel, achternaam, email, voorkeur, geboortedatum, geslacht, "random tekst", wachtwoord, profielfoto, true, locatie, opleiding);
            
               foreach (string interesse in GekozenInteressesLijst)
                {
                    database.RegisterInterestInDatabase(email, interesse);
                }
                Navigation.PushAsync(new MainPage());
            
        }


    }

    //checkt of waardes naar eisen voldoen en declareert de variabelen
    public bool RegisterBtnEventCheck()
    {
        int aantalchecks = 0;
        Authentication auth = new Authentication();
       
        DateTime geboortedatumtijdelijk;
        geboortedatumtijdelijk = new DateTime(Geboortedatum.Date.Year, Geboortedatum.Date.Month, Geboortedatum.Date.Day);


        #region email checks
        if (Email.Text == null)
        {
            FoutEmail.Text = "Email mag niet leeg zijn";
            FoutEmail.IsVisible = true;
            aantalchecks -= 1;
        }
        else
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
                FoutEmail.IsVisible = true;
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
        }
        else
        {
            Foutvoornaam.IsVisible = false;
            voornaam = Voornaam.Text;
            aantalchecks += 1;
        }
        #endregion

        #region tussenvoegsel

        if (Tussenvoegsel.Text != null)
        {
            tussenvoegsel = Tussenvoegsel.Text;
        }

        #endregion

        #region achernaam check

        if (Achternaam.Text == null)
        {

            FoutAchternaam.IsVisible = true;
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
            FoutWachtwoord.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            if (auth.CheckPassword(Wachtwoord.Text) == false)
            {
                FoutWachtwoord.Text = "Wachtwoord moet minimaal 8 karakters, 1 getal en 1 hoofdletter bevatten";
                FoutWachtwoord.IsVisible = true;
                aantalchecks -= 1;
            }
            else
            {
                FoutWachtwoord.IsVisible = false;
                wachtwoord = auth.HashPassword(Wachtwoord.Text);
                aantalchecks += 1;
            }



        }
        #endregion

        #region geboortedatum checks

        if (auth.CalculateAge(geboortedatumtijdelijk) < 18)
        {
            FoutLeeftijd.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            FoutLeeftijd.IsVisible = false;
            geboortedatum = new DateTime(Geboortedatum.Date.Year, Geboortedatum.Date.Month, Geboortedatum.Date.Day);
            aantalchecks += 1;
        }
        #endregion

        #region geslacht check

        if (Geslacht.SelectedItem == null)
        {
            FoutGeslacht.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            FoutGeslacht.IsVisible = false;
            geslacht = Geslacht.SelectedItem.ToString();
            aantalchecks += 1;
        }
        #endregion

        if (aantalchecks == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // maakt gegevens van gebruiker zichtbaar en onzichtbaar
    public void RegisterBtnEvent(object sender, EventArgs e)
    {
       

        if (RegisterBtnEventCheck())
        {
            //setting objects visible to proceed the registerform
            LblVoorkeur.IsVisible = true;
            Voorkeur.IsVisible = true;
            LblOpleiding.IsVisible = true;
            Opleiding.IsVisible = true;
            LblLocatie.IsVisible = true;
            Locatie.IsVisible = true;
            LblInteresses.IsVisible = true;
            Interesses.IsVisible = true;
            ProfileImage.IsVisible = true;


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

    private async void OnProfilePictureClicked(object sender, EventArgs e)
    {
        try
        {
            var image = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Kies een profielfoto",
                FileTypes = FilePickerFileType.Images
            });

            if (image == null)
            {
                return;
            }
            string imgLocation = image.FullPath;
            byte[] imageArr = null;
            FileStream fileStream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
            Stream stream = await image.OpenReadAsync();
            BinaryReader binary = new BinaryReader(fileStream);
            imageArr = binary.ReadBytes((int)fileStream.Length);
            profielfoto = imageArr;
            ProfileImage.Source = ImageSource.FromStream(() => stream);

        } catch (Exception ex) {
            FoutProfielfoto.Text = ex.Message;
            FoutProfielfoto.IsVisible = true;
        }
    }

}


