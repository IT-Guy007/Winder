
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;
using DataModel;
using System.Text.RegularExpressions;
using System.Collections;

namespace MAUI;

public partial class ProfielOpmaak : ContentPage
{
    List<string> interesses;
    Database b = new Database();
    Color ErrorColor = new Color(255,243,5);
    User user;
    bool voornaam = true, tussenvoegsel = true, achternaam = true, geboortedatum = false,omschrijving = true, geslacht = true,voorkeur = true, interessesGekozen = true;
    public ProfielOpmaak()
    {
        InitializeComponent();
        interesses = new List<string>();
        interessePicker.ItemsSource = getAllListOfInterestsInStringFromDatabase();
        loadUserFromDatabaseInForm();
        listInteresses.ItemsSource = interesses;
        changeLabelsTextSizeAndColor(9, new Color(0, 0, 0));
    }

    private void changeLabelsTextSizeAndColor(int fontSize, Color color)
    {
        lblVoornaam.FontSize = fontSize;
        lblVoornaam.TextColor = color;
        lblTussenvoegsel.FontSize = fontSize;
        lblTussenvoegsel.TextColor = color;
        lblAchternaam.FontSize = fontSize;
        lblAchternaam.TextColor = color;
        lblGeboortedatum.FontSize = fontSize;
        lblGeboortedatum.TextColor = color;
        Gender.FontSize = fontSize;
        Voorkeur.FontSize = fontSize;
        lblOmschrijving.FontSize = fontSize;
        lblOmschrijving.TextColor = color;
        interessePicker.FontSize = fontSize;
        
    }

    private void loadUserFromDatabaseInForm()
    {
        if (Authentication._currentUser != null)
        {
            user = Authentication._currentUser;
            Voornaam.Placeholder = user.firstName;
            Tussenvoegsel.Placeholder = user.middleName;
            Achternaam.Placeholder = user.lastName;
            Geboortedatum.Date = getUserBirthday(user);
            Omschrijving.Placeholder = user.bio;
            Gender.SelectedIndex = getGenderFromUser(user);
            Voorkeur.SelectedIndex = getPreferenceFromUser(user);
        }
    }

    private int getPreferenceFromUser(User user)
    {
        if (user.preference == "male") return 1;
        if (user.preference == "female") return 2;
        return 0;
    }

    private int getGenderFromUser(User user)
    {
        if (user.gender == "male")return 1;
        if (user.gender == "female")return 2;
        return 0;
    }

    private DateTime getUserBirthday(User user)
    {
        if (user.birthDay == null)
        {
            return DateTime.Now;
        }
        else
        {
            return (DateTime)user.birthDay;
        }
    }


    private List<string> getAllListOfInterestsInStringFromDatabase()
    {
        List<string> interests = new List<string>();
        interests = b.GetInterestsFromDataBase();
        return interests;
    }

    private void wijzigProfielGegevens(object sender, EventArgs e)
    {
        if (voornaam && tussenvoegsel && achternaam && geboortedatum && omschrijving && geslacht && voorkeur && interessesGekozen)
        {
            updateUserPropertiesPrepareForUpdateQuery();
            b.updateUserInDatabaseWithNewUserProfile();
            registerInterestsInDatabase();
            DisplayAlert("Melding", "Je gegevens zijn aangepast", "OK");
            fillInFormWithUserProperties(user);
        }
        else
        {
            DisplayAlert("Er is iets verkeerd gegaan...", "Vul alle gegevens in", "OK");
        }
    }

    private void fillInFormWithUserProperties(User user)
    {
        clearTextFromEntries();
        updatePlaceholders();
    }

    private void updatePlaceholders()
    {
        Voornaam.Placeholder = user.firstName;
        Tussenvoegsel.Placeholder = user.middleName;
        Achternaam.Placeholder = user.lastName;
        Geboortedatum.Date = getUserBirthday(user);
        Omschrijving.Placeholder = user.bio;
    }

    private void clearTextFromEntries()
    {
        Voornaam.Text = "";
        Tussenvoegsel.Text = "";
        Achternaam.Text = "";
        Omschrijving.Text = "";
    }

    private void updateUserPropertiesPrepareForUpdateQuery()
    {
        if(Authentication._currentUser.firstName != Voornaam.Text && Voornaam.Text != null && Voornaam.Text != "") Authentication._currentUser.firstName = Voornaam.Text;
        if (Authentication._currentUser.middleName != Tussenvoegsel.Text && Tussenvoegsel.Text != null && Tussenvoegsel.Text != "") Authentication._currentUser.middleName = Tussenvoegsel.Text;
        if (Authentication._currentUser.lastName != Achternaam.Text && Achternaam.Text != null && Achternaam.Text != "") Authentication._currentUser.lastName = Achternaam.Text;
        if (Authentication._currentUser.birthDay != Geboortedatum.Date) Authentication._currentUser.birthDay = Geboortedatum.Date;
        if (Authentication._currentUser.bio != Omschrijving.Text && Omschrijving.Text != null && Omschrijving.Text != "") Authentication._currentUser.bio = Omschrijving.Text;
        if (Authentication._currentUser.gender != Gender.SelectedItem.ToString()) Authentication._currentUser.gender = Gender.SelectedItem.ToString();
        if(Authentication._currentUser.preference != Voorkeur.SelectedItem.ToString()) Authentication._currentUser.preference = Voorkeur.SelectedItem.ToString();
    }
    private void registerInterestsInDatabase()
    {
        foreach (var interest in interesses)
        {
            b.RegisterInterestInDatabase(Authentication._currentUser.email, interest);
        }
    }
    private void Voornaam_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Voornaam.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Voornaam.Text))
            {
                voornaam = false;
                lblVoornaam.Text = "Voornaam mag alleen letters bevatten";
                lblVoornaam.TextColor = ErrorColor;
            }
            else
            {
                voornaam = true;
                lblVoornaam.Text = "Voornaam";
                lblVoornaam.Text = default;
                Voornaam.Text = Voornaam.Text.First().ToString().ToUpper() + Voornaam.Text[1..].ToLower();
            }
        }
    }
    private void Tussenvoegsel_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Tussenvoegsel.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Tussenvoegsel.Text))
            {
                tussenvoegsel = false;
                lblTussenvoegsel.Text = "Tussenvoegsel mag alleen letters bevatten";
                lblTussenvoegsel.TextColor = ErrorColor;
            }
            else
            {
                tussenvoegsel = true;
                lblTussenvoegsel.Text = "Tussenvoegsel";
                lblTussenvoegsel.TextColor = default;
                Tussenvoegsel.Text = Tussenvoegsel.Text.First().ToString().ToUpper() + Tussenvoegsel.Text[1..].ToLower();
            }
        }
    }
    private void Achternaam_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Achternaam.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Achternaam.Text))
            {
                achternaam = false;
                lblAchternaam.Text = "Achternaam mag alleen letters bevatten";
                lblAchternaam.TextColor = ErrorColor;
            }
            else
            {
                achternaam = true;
                lblAchternaam.Text = "Tussenvoegsel";
                lblAchternaam.TextColor = default;
                Achternaam.Text = Achternaam.Text.First().ToString().ToUpper() + Achternaam.Text[1..].ToLower();
            }
        }
    }
    private void Omschrijving_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Omschrijving.Text != "")
        {
            if (!checkIfTextIsOnlyLettersSpacesCommasAndDots(Omschrijving.Text))
            {
                omschrijving = false;
                lblOmschrijving.Text = "Omschrijving mag alleen letters bevatten";
                lblOmschrijving.TextColor = ErrorColor;
            }
            else
            {
                omschrijving = true;
                lblOmschrijving.Text = "Omschrijving";
                lblOmschrijving.TextColor = default;
            }
        }
    }

    private bool checkIfTextIsOnlyLettersSpacesCommasAndDots(string text)
    {
        return Regex.IsMatch(text, @"^[a-zA-Z ,.]+$");
    }

    private bool checkIfTextIsOnlyLettersAndSpaces(string text)
    {
        foreach (char c in text)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                return false;
            }
        }
        return true;
    }

    private bool checkIfTextIsOnlyLetters(string text)
    {
        if (text.All(char.IsLetter))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void interessePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (interessePicker.SelectedItem != null && interesses.Count < 5)
        {
            interessePicker.Title = "Interesse";
            interessePicker.TitleColor = default;
            if (!interesses.Contains(interessePicker.SelectedItem.ToString()))
            {
                interesses.Add(interessePicker.SelectedItem.ToString());
                listInteresses.ItemsSource = null;
                listInteresses.ItemsSource = interesses;
            }
            else
            {
                interessePicker.Title = "Je hebt deze interesse al toegevoegd";
                interessePicker.TitleColor = ErrorColor;
            }
        }
        else
        {
            interessePicker.Title = "Je kunt maximaal 5 interesses selecteren";
            interessePicker.TitleColor = ErrorColor;
        }
    }

    private void DeleteItem_Clicked(object sender, EventArgs e)
    {
        if (listInteresses.SelectedItem != null) {
            interessePicker.Title = "Interesse";
            interessePicker.TitleColor = default;
            var interest = listInteresses.SelectedItem.ToString();
            b.removeInterestOutOfuserHasInterestTableDatabase(Authentication._currentUser.email, interest);
            interesses.Remove(interest);
            listInteresses.ItemsSource = null;
            listInteresses.ItemsSource = interesses;
            listInteresses.SelectedItem = null;
        }
    }

    private void Geboortedatum_DateSelected(object sender, DateChangedEventArgs e)
    {
        checkIfAgeOfBirthDateIsOver18();
    }

    private void checkIfAgeOfBirthDateIsOver18()
    {
        DateTime today = DateTime.Today;
        int age = today.Year - Geboortedatum.Date.Year;
        if (Geboortedatum.Date > today.AddYears(-age)) age--;
        if (age >= 18)
        {
            geboortedatum = true;
            lblGeboortedatum.Text = "Leeftijd : " + age;
            lblGeboortedatum.BackgroundColor = default;
        }
        else
        {
            geboortedatum = false;
            lblGeboortedatum.Text = "Je moet minimaal 18 jaar zijn";
            lblGeboortedatum.BackgroundColor = ErrorColor;
        }
    }
}