
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;
using DataModel;
using System.Text.RegularExpressions;

namespace MAUI;

public partial class ProfielOpmaak : ContentPage
{
    List<string> interesses;
    Database b = new Database();
    User user;
    bool voornaam = true, tussenvoegsel = true, achternaam = true, geboortedatum = false,omschrijving = true, geslacht = true,voorkeur = true, interessesGekozen = true;
    public ProfielOpmaak()
    {
        InitializeComponent();
        interesses = new List<string>();
        interessePicker.ItemsSource = getAllListOfInterestsInStringFromDatabase();
        loadUserFromDatabaseInForm();
        interesses = b.LoadInterestsFromDatabaseInListInteresses(user.username);
        listInteresses.ItemsSource = interesses;
    }
    private void loadUserFromDatabaseInForm()
    {
        user = b.GetUserFromDatabase("s1416890@student.windesheim.nl");
        Voornaam.Placeholder = user.firstName;
        Tussenvoegsel.Placeholder = user.middleName;
        Achternaam.Placeholder = user.lastName;
        Geboortedatum.Date = getUserBirthday(user);
        Omschrijving.Placeholder = user.bio;
        Gender.SelectedIndex = getGenderFromUser(user);
        Voorkeur.SelectedIndex = getPreferenceFromUser(user);
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
            updateUserPropertiesPrepareForUpdateQuery(user);
            b.updateUserInDatabaseWithNewUserProfile(user);
            registerInterestsInDatabase();
            DisplayAlert("Melding", "Je gegevens zijn aangepast", "OK");
            Voornaam.Text = "";
            Tussenvoegsel.Text = "";
            Achternaam.Text = "";
            Omschrijving.Text = "";
        }
        else
        {
            DisplayAlert("Er is iets verkeerd gegaan...", "Vul alle gegevens in", "OK");
        }
    }

    private void updateUserPropertiesPrepareForUpdateQuery(User user)
    {
        if(user.firstName != Voornaam.Text) user.firstName = Voornaam.Text;
        if(user.middleName != Tussenvoegsel.Text) user.middleName = Tussenvoegsel.Text;
        if(user.lastName != Achternaam.Text) user.lastName = Achternaam.Text;
        if(user.bio != Omschrijving.Text) user.bio = Omschrijving.Text;
        if(user.birthDay != Geboortedatum.Date) user.birthDay = Geboortedatum.Date;
        if(user.gender != Gender.SelectedItem.ToString())user.gender = Gender.SelectedItem.ToString();
        if(user.preference != Voorkeur.SelectedItem.ToString()) user.preference = Voorkeur.SelectedItem.ToString();
    }

    private void registerInterestsInDatabase()
    {
        foreach (var interest in interesses)
        {
            b.RegisterInterestInDatabase(user.username, interest);
        }
    }
    private void Voornaam_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Voornaam.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Voornaam.Text))
            {
                voornaam = false;
                Color colorRed = new Color(238, 75, 43);
                lblVoornaam.Text = "Voornaam mag alleen letters bevatten";
                lblVoornaam.TextColor = colorRed;
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
                Color colorRed = new Color(238, 75, 43);
                lblTussenvoegsel.Text = "Tussenvoegsel mag alleen letters bevatten";
                lblTussenvoegsel.TextColor = colorRed;
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
                Color colorRed = new Color(238, 75, 43);
                lblAchternaam.Text = "Achternaam mag alleen letters bevatten";
                lblAchternaam.TextColor = colorRed;
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
                Color colorRed = new Color(238, 75, 43);
                lblOmschrijving.Text = "Omschrijving mag alleen letters bevatten";
                lblOmschrijving.TextColor = colorRed;
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
        addSelectedValueToListInteresses();
    }

    private void addSelectedValueToListInteresses()
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
            else {
                Color colorRed = new Color(238, 75, 43);
                interessePicker.Title = "Je hebt deze interesse al toegevoegd";
                interessePicker.TitleColor = colorRed;
            }
        }
        else
        {
            Color colorRed = new Color(238, 75, 43);
            interessePicker.Title = "Je kunt maximaal 5 interesses selecteren";
            interessePicker.TitleColor = colorRed;
        }
    }

    private void DeleteItem_Clicked(object sender, EventArgs e)
    {
        if (listInteresses.SelectedItem != null) {
            interessePicker.Title = "Interesse";
            interessePicker.TitleColor = default;
            var interest = listInteresses.SelectedItem.ToString();
            b.removeInterestOutOfuserHasInterestTableDatabase(user.username, interest);
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
                Color colorRed = new Color(238, 75, 43);
                lblGeboortedatum.Text = "Je moet minimaal 18 jaar zijn";
                lblGeboortedatum.BackgroundColor = colorRed;
            }
    }
}