using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace MAUI;

public partial class ProfielOpmaak : ContentPage
{
    public ProfielOpmaak()
	{
		InitializeComponent();
    }
        private void wijzigProfielGegevens(object sender, EventArgs e)
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
                Achternaam.Text = "";
                Achternaam.Placeholder = "Achternaam mag alleen letters bevatten";
                Achternaam.PlaceholderColor = colorRed;
            }
            else
            {
                Achternaam.Text = Achternaam.Text.First().ToString().ToUpper() + Achternaam.Text[1..].ToLower();
            }
        }
    }
    private void LeeftijdSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Naam.Text = "Leeftijd: " + Convert.ToInt16(LeeftijdSlider.Value).ToString();
    }
    private void Omschrijving_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Omschrijving.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Omschrijving.Text))
            {
                Color colorRed = new Color(238, 75, 43);
                Omschrijving.Text = "";
                Omschrijving.Placeholder = "Omschrijving mag alleen letters bevatten";
                Omschrijving.PlaceholderColor = colorRed;
            }
            else
            {
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
    private void Interesses_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Interesses.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Interesses.Text))
            {
                Color colorRed = new Color(238, 75, 43);
                Interesses.Text = "";
                Interesses.Placeholder = "Interesses mag alleen letters bevatten";
                Interesses.PlaceholderColor = colorRed;
            }
            else
            {
                Interesses.Text = Interesses.Text.First().ToString().ToUpper() + Interesses.Text[1..].ToLower();
            }
        }
    }

    private void Voorkeur_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void Geslacht_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}