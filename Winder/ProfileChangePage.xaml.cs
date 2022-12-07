
using DataModel;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace MAUI;

public partial class ProfileChange : ContentPage
{
    List<string> interesses;
    Database b = new Database();
    Color ErrorColor = new Color(255, 243, 5);
    Authentication auth = new Authentication();
    private bool firstname = true;
    private bool middlename = true;
    private bool lastname = true;
    private bool birthday = true;
    private bool preference = true;
    private bool gender = true;
    private bool bio = true;
    private bool locatie = true;
    public ProfileChange()
    {
        InitializeComponent();
        interesses = new List<string>();
        InterestSelection.ItemsSource = b.GetInterestsFromDataBase();
        b.LoadInterestsFromDatabaseInListInteresses(auth._currentUser.email);
        interesses = fillListOfInterestsWithStringArray(b.LoadInterestsFromDatabaseInListInteresses(auth._currentUser.email));
        listInteresses.ItemsSource = interesses;
        changeLabelsTextSizeAndColor(9, new Color(255,255,255));
    }

    private List<string> fillListOfInterestsWithStringArray(string[] strings)
    {
        List<string> list = new List<string>();
        foreach (string s in strings)
        {
            list.Add(s);
        }
        return list;
    }

    private void changeLabelsTextSizeAndColor(int size, Color color)
    {
        lblFirstname.FontSize = size;
        lblFirstname.TextColor = color;
        lblMiddlename.FontSize = size;
        lblMiddlename.TextColor = color;
        lblLastname.FontSize = size;
        lblLastname.TextColor = color;
        lblBirthdate.FontSize = size;
        lblBirthdate.TextColor = color;
        Gender.FontSize = size;
        Preference.FontSize = size;
        lblBio.FontSize = size;
        lblBio.TextColor = color;
        InterestSelection.FontSize = size;
    }

    private void ChangeUserData(object sender, EventArgs e)
    {
        if (firstname == true && middlename == true && lastname == true && birthday == true && preference == true && gender == true && bio == true && locatie == true)
        {
            DisplayAlert("Melding", "Je gegevens zijn aangepast", "OK");
            Firstname.Placeholder = Firstname.Text;
            Middlename.Placeholder = Middlename.Text;
            Lastname.Placeholder = Lastname.Text;
            Bio.Placeholder = Bio.Text;
            Firstname.Text = "";
            Middlename.Text = "";
            Lastname.Text = "";
            Bio.Text = "";
        }
        else
        {
            DisplayAlert("Er is iets verkeerd gegaan...", "Vul alle gegevens in", "OK");
        }
    }
    private void FirstnameTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Firstname.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Firstname.Text))
            {
                firstname = false;
                Color colorRed = new Color(238, 75, 43);
                lblFirstname.Text = "Voornaam mag alleen letters bevatten";
                lblFirstname.TextColor = colorRed;
            }
            else
            {
                firstname = true;
                lblFirstname.Text = "Voornaam";
                lblFirstname.Text = default;
                Firstname.Text = Firstname.Text.First().ToString().ToUpper() + Firstname.Text[1..].ToLower();
            }
        }
    }
    private void MiddlenameTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Middlename.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Middlename.Text))
            {
                middlename = false;
                Color colorRed = new Color(238, 75, 43);
                lblMiddlename.Text = "Tussenvoegsel mag alleen letters bevatten";
                lblMiddlename.TextColor = colorRed;
            }
            else
            {
                middlename = true;
                lblMiddlename.Text = "Tussenvoegsel";
                lblMiddlename.TextColor = default;
                Middlename.Text = Middlename.Text.First().ToString().ToUpper() + Middlename.Text[1..].ToLower();
            }
        }
    }
    private void LastnameTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Lastname.Text != "")
        {
            if (!checkIfTextIsOnlyLetters(Lastname.Text))
            {
                lastname = false;
                Color colorRed = new Color(238, 75, 43);
                lblLastname.Text = "Achternaam mag alleen letters bevatten";
                lblLastname.TextColor = colorRed;
            }
            else
            {
                lastname = true;
                lblLastname.Text = "Achternaam";
                lblLastname.TextColor = default;
                Lastname.Text = Lastname.Text.First().ToString().ToUpper() + Lastname.Text[1..].ToLower();
            }
        }
    }
    private void BioTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Bio.Text != "")
        {
            if (!checkIfTextIsOnlyLettersAndSpaces(Bio.Text))
            {
                bio = false;
                Color colorRed = new Color(238, 75, 43);
                lblBio.Text = "Bio mag alleen letters bevatten";
                lblBio.TextColor = colorRed;
            }
            else
            {
                bio = true;
                lblBio.Text = "Bio";
                lblBio.TextColor = default;
                Bio.Text = Bio.Text.First().ToString().ToUpper() + Bio.Text[1..].ToLower();
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

    private void PickerIndexChanged(object sender, EventArgs e)
    {
        if (InterestSelection.SelectedItem != null && interesses.Count < 5)
        {
            InterestSelection.Title = "Interesse";
            InterestSelection.TitleColor = default;
            if (!interesses.Contains(InterestSelection.SelectedItem.ToString()))
            {
                interesses.Add(InterestSelection.SelectedItem.ToString());
                listInteresses.ItemsSource = null;
                listInteresses.ItemsSource = interesses;
            }
            else
            {
                Color colorRed = new Color(238, 75, 43);
                InterestSelection.Title = "Je hebt deze interesse al toegevoegd";
                InterestSelection.TitleColor = colorRed;
            }
        }
        else
        {
            Color colorRed = new Color(238, 75, 43);
            InterestSelection.Title = "Je kunt maximaal 5 interesses selecteren";
            InterestSelection.TitleColor = colorRed;
        }
    }
    private void DeleteInterest(object sender, EventArgs e)
    {
        if (listInteresses.SelectedItem != null) {
            var item = listInteresses.SelectedItem.ToString();
            interesses.Remove(item);
            listInteresses.ItemsSource = null;
            listInteresses.ItemsSource = interesses;
            listInteresses.SelectedItem = null;
        }
    }


    private void DateOfBirthSelectedDate(object sender, DateChangedEventArgs e)
    {
                    DateTime today = DateTime.Today;
            int age = today.Year - Birthdate.Date.Year;
            if (Birthdate.Date > today.AddYears(-age)) age--;
            if (age >= 18)
            {
                birthday = true;
            lblBirthdate.Text = "Leeftijd : " + age;
            lblBirthdate.BackgroundColor = default;
            }
            else
            {
            birthday = false;
                Color colorRed = new Color(238, 75, 43);
                lblBirthdate.Text = "Je moet minimaal 18 jaar zijn";
                lblBirthdate.BackgroundColor = colorRed;
            }
    }

    private void checkIfAgeOfBirthDateIsOver18()
    {

    }
}