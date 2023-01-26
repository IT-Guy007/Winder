﻿using DataModel;
using MAUI;

namespace Winder;

public partial class RegisterPage {
	private string email;
    private string firstname;
    private string middleName;
    private string lastname;
    private DateTime dateOfBirth;
    private string gender;
    private string password;
    private string preference;
    private string major;
    private string school;
    private byte[] profilePicture;

    private readonly Database database;
    private readonly List<string> interestsList;
    private readonly List<string> chosenInterestsList;



    public RegisterPage() {
        database = new Database();
        interestsList = new List<string>();
        chosenInterestsList = new List<string>();
        
        InitializeComponent();
        interestsList = database.GetInterestsFromDataBase();
        foreach (string interest in interestsList) {
            Interesses.Items.Add(interest);
        }
    }


    //Gebruiker verwijdert een " interesse " uit de selectie door erop te klikken
    private void SelectedItemsOfInterests(object sender, EventArgs e) {
        if (Gekozeninteresses.SelectedItem != null) {
            chosenInterestsList.Remove(Gekozeninteresses.SelectedItem.ToString());

        }
        Gekozeninteresses.ItemsSource = null;
        Gekozeninteresses.IsVisible = false;
        if (chosenInterestsList.Count > 0) {
            Gekozeninteresses.IsVisible = true;
            Gekozeninteresses.ItemsSource = chosenInterestsList;
        }



    }

    // Voegt geselecteerde items toe aan listbox zodat de gebruiker zijn selectie kan zien
    private void OnSelectedItems(object sender, EventArgs e) {
        if (chosenInterestsList.Count() < 5 && Interesses.SelectedItem != null) {
            if (chosenInterestsList.Contains(Interesses.SelectedItem.ToString())) {
                Foutinteresses.Text = "interesse is al toegevoegd";
                Foutinteresses.IsVisible = true;
            }
            else {
                Foutinteresses.IsVisible = false;
                chosenInterestsList.Add(Interesses.SelectedItem.ToString());
                Gekozeninteresses.ItemsSource = null;
                Gekozeninteresses.ItemsSource = chosenInterestsList;
                Gekozeninteresses.IsVisible = true;

            }
        }

        Gekozeninteresses.ItemsSource = chosenInterestsList;
        Gekozeninteresses.IsVisible = true;
    }
    //Declaring objects by "Opslaan" button
    // checks
    private bool SaveEventChecks() {
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
            preference = Voorkeur.SelectedItem.ToString();
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
            major = Opleiding.Text;
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
            school = Locatie.SelectedItem.ToString();
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
            Interesses.ItemsSource = interestsList;
            aantalchecks += 1;
        }
        #endregion

        if (aantalchecks == 5) {
            return true;
        }else {
            return false;
        }

    }

    private void SaveEvent (object sender, EventArgs e) {
        if (SaveEventChecks()) {
            middleName ??= "";

            database.RegistrationFunction(firstname, middleName, lastname, email, preference, dateOfBirth, gender, " ", password, profilePicture, true, school, major);
            database.SaveProfilePictures(email, profilePicture);
            User currentUser = new User(firstname, middleName, lastname, dateOfBirth, preference, email, password, gender, profilePicture, " ", school, major,18,23);
            Authentication.CurrentUser = currentUser;

            foreach (string interesse in chosenInterestsList) {
                database.RegisterInterestInDatabase(email, interesse);
            }

            Navigation.PushAsync(new StartPage());

        }
    }


    //checkt of waardes naar eisen voldoen en declareert de variabelen
    private bool RegisterBtnEventCheck() {
        int aantalchecks = 0;
        Authentication auth = new Authentication();
       
        DateTime geboortedatumtijdelijk;
        geboortedatumtijdelijk = new DateTime(Geboortedatum.Date.Year, Geboortedatum.Date.Month, Geboortedatum.Date.Day);


        #region Email checks
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
            if (CheckEmail(Email.Text))
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
            firstname = Voornaam.Text;
            aantalchecks += 1;
        }
        #endregion

        #region tussenvoegsel

        if (Tussenvoegsel.Text != null)
        {
            middleName = Tussenvoegsel.Text;
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
            lastname = Achternaam.Text;
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
                password = auth.HashPassword(Wachtwoord.Text);
                aantalchecks += 1;
            }



        }
        #endregion

        #region geboortedatum checks

        if (Authentication.CalculateAge(geboortedatumtijdelijk) < 18)
        {
            FoutLeeftijd.IsVisible = true;
            aantalchecks -= 1;
        }
        else
        {
            FoutLeeftijd.IsVisible = false;
            dateOfBirth = new DateTime(Geboortedatum.Date.Year, Geboortedatum.Date.Month, Geboortedatum.Date.Day);
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
            gender = Geslacht.SelectedItem.ToString();
            aantalchecks += 1;
        }
        #endregion

        if (aantalchecks == 7) {
            return true;
        }
        else {
            return false;
        }
    }

    // maakt gegevens van gebruiker zichtbaar en onzichtbaar
    private void RegisterBtnEvent(object sender, EventArgs e) {

        if (RegisterBtnEventCheck()) {
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

    private async void OnProfilePictureClicked(object sender, EventArgs e) {
        try {
            var image = await FilePicker.PickAsync(new PickOptions {
                PickerTitle = "Kies een profielfoto",
                FileTypes = FilePickerFileType.Images
            });

            if (image == null) {
                return;
            }
            string imgLocation = image.FullPath;
            FileStream fileStream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
            Stream stream = await image.OpenReadAsync();
            BinaryReader binary = new BinaryReader(fileStream);
            byte[] imageArr = binary.ReadBytes((int)fileStream.Length);
            profilePicture = imageArr;
            ProfileImage.Source = ImageSource.FromStream(() => stream);

        } catch (Exception ex) {
            FoutProfielfoto.Text = ex.Message;
            FoutProfielfoto.IsVisible = true;
        }
    }

    private void Backbutton_Clicked(object sender, EventArgs e) {
        Navigation.PushAsync(new MainPage());
    }
    
    // checks if Email belongs to Windesheim, returns true if so
    private bool CheckEmail(string email) {
        if (email.EndsWith(Authentication.emailEndsWith) && email.StartsWith(Authentication.emailStartsWith)) {
            return true;
        }
        return false;
    }
}


