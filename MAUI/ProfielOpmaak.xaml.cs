using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace MAUI;

public partial class ProfielOpmaak : ContentPage
{
    List<string> interesses;
    bool voornaam, tussenvoegsel, achternaam, geboortedatum,omschrijving, geslacht,voorkeur, interessesGekozen;
    public ProfielOpmaak()
    {
        InitializeComponent();
        interesses = new List<string>();
        listInteresses.ItemsSource = interesses;
    }
    private void wijzigProfielGegevens(object sender, EventArgs e)
    {
        //checkAllInput(Voornaam, Tussenvoegsel, Achternaam, Geboortedatum, Gender, Voorkeur, Omschrijving, interesses);
    }

    private void checkAllInput(Entry voornaam, Entry tussenvoegsel, Entry achternaam, DatePicker geboortedatum, Picker gender, Picker voorkeur, Entry omschrijving, List<string> interesses)
    {
        
    }

    private void Voornaam_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Voornaam.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Voornaam.Text))
            {
                Color colorRed = new Color(238, 75, 43);
                lblVoornaam.Text = "Voornaam mag alleen letters bevatten";
                lblVoornaam.BackgroundColor = colorRed;
            }
            else
            {
                lblVoornaam.Text = "Voornaam";
                lblVoornaam.BackgroundColor = default;
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
                Color colorRed = new Color(238, 75, 43);
                lblTussenvoegsel.Text = "Tussenvoegsel mag alleen letters bevatten";
                lblTussenvoegsel.BackgroundColor = colorRed;
            }
            else
            {
                lblTussenvoegsel.Text = "Tussenvoegsel";
                lblTussenvoegsel.BackgroundColor = default;
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
                Color colorRed = new Color(238, 75, 43);
                lblAchternaam.Text = "Achternaam mag alleen letters bevatten";
                lblAchternaam.BackgroundColor = colorRed;
            }
            else
            {
                lblAchternaam.Text = "Tussenvoegsel";
                lblAchternaam.BackgroundColor = default;
                Achternaam.Text = Achternaam.Text.First().ToString().ToUpper() + Achternaam.Text[1..].ToLower();
            }
        }
    }
    private void Omschrijving_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Omschrijving.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Omschrijving.Text))
            {
                Color colorRed = new Color(238, 75, 43);
                lblOmschrijving.Text = "Omschrijving mag alleen letters bevatten";
                lblOmschrijving.BackgroundColor = colorRed;
            }
            else
            {
                lblOmschrijving.Text = "Omschrijving";
                lblOmschrijving.BackgroundColor = default;
                Omschrijving.Text = Omschrijving.Text.First().ToString().ToUpper() + Omschrijving.Text[1..].ToLower();
            }
        }
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
        
    }
}