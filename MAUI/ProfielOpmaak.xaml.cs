
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace MAUI;

public partial class ProfielOpmaak : ContentPage
{
    List<string> interesses;
    bool voornaam = true, tussenvoegsel = true, achternaam = true, geboortedatum = true,omschrijving = true, geslacht = true,voorkeur = true, interessesGekozen = true;
    public ProfielOpmaak()
    {
        InitializeComponent();
        interesses = new List<string>();
        listInteresses.ItemsSource = interesses;
    }
    private void wijzigProfielGegevens(object sender, EventArgs e)
    {
        checkAllInput();
    }

    private void checkAllInput()
    {
        if (voornaam && tussenvoegsel && achternaam && geboortedatum && omschrijving && geslacht && voorkeur && interessesGekozen)
        {
            DisplayAlert("Melding", "Je gegevens zijn aangepast", "OK");
            Voornaam.Placeholder = Voornaam.Text;
            Tussenvoegsel.Placeholder = Tussenvoegsel.Text;
            Achternaam.Placeholder = Achternaam.Text;
            Omschrijving.Placeholder = Omschrijving.Text;
            Voornaam.Text = "";
            Tussenvoegsel.Text = "";
            Achternaam.Text = "";
            Omschrijving.Text = "";
        }
        else {
            DisplayAlert("Er is iets verkeerd gegaan...", "Vul alle gegevens in", "OK");
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
            if (!checkIfTextIsOnlyLettersAndSpaces(Omschrijving.Text))
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
                Omschrijving.Text = Omschrijving.Text.First().ToString().ToUpper() + Omschrijving.Text[1..].ToLower();
            }
        }
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
            var item = listInteresses.SelectedItem.ToString();
            interesses.Remove(item);
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