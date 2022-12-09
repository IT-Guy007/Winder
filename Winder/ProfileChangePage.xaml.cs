
using DataModel;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace MAUI;

public partial class ProfileChange : ContentPage
{
    List<string> interesses  =new List<string>();
    Database Database = new Database();
    Color ErrorColor = new Color(255, 243, 5);
    private bool firstname = true;
    private bool middlename = true;
    private bool lastname = true;
    private bool birthday = true;
    private bool preference = true;
    private bool gender = true;
    private bool bio = true;
    private bool education = true;
    public ProfileChange()
    {
        InitializeComponent();
        LoadUserFromDatabaseInForm();
        InterestSelection.ItemsSource = Database.GetInterestsFromDataBase();
        interesses = Database.LoadInterestsFromDatabaseInListInteresses(Authentication._currentUser.email);
        ListInterests.ItemsSource = interesses;
    }

    private List<string> convertStringArrayToList(string[] strings)
    {
        List<string> list = new List<string>();
        foreach (var item in strings)
        {
            list.Add(item);
        }

        return list;
    }

    private void ChangeSizeEntriesAndLabels(int size)
    {
        Firstname.FontSize = size;
        Middlename.FontSize = size;
        Lastname.FontSize = size;
        Birthdate.FontSize = size;
        Preference.FontSize = size;
        Gender.FontSize = size;
        Bio.FontSize = size;
        Education.FontSize = size;
        lblFirstname.FontSize = size;
        lblMiddlename.FontSize = size;
        lblLastname.FontSize = size;
        lblBirthdate.FontSize = size;
        Gender.FontSize = size;
        Preference.FontSize = size;
        lblBio.FontSize = size;
        InterestSelection.FontSize = size;
        lblEducation.FontSize = size;
    }

    private void LoadUserFromDatabaseInForm()
    {
        if (Authentication._currentUser != null)
        {
            Firstname.Placeholder = Authentication._currentUser.firstName;
            Middlename.Placeholder = Authentication._currentUser.middleName;
            Lastname.Placeholder = Authentication._currentUser.lastName;
            Birthdate.Date = Authentication._currentUser.birthDay;
            Bio.Placeholder = Authentication._currentUser.bio;
            Education.Placeholder = Authentication._currentUser.major;
            Gender.SelectedIndex = GetGenderFromUser();
            Preference.SelectedIndex = GetPreferenceFromUser();
        }
    }

    private int GetPreferenceFromUser()
    {
        if (Authentication._currentUser.preference == "Man") return 1;
        if (Authentication._currentUser.preference == "Vrouw") return 2;
        return 0;
    }

    private int GetGenderFromUser()
    {
        if (Authentication._currentUser.gender == "Man") return 1;
        if (Authentication._currentUser.gender == "Vrouw") return 2;
        return 0;
    }

    private void ChangeUserData(object sender, EventArgs e)
    {
        if (firstname == true && middlename == true && lastname == true && birthday == true && preference == true && gender == true && bio == true && education == true)
        {
            UpdateUserPropertiesPrepareForUpdateQuery();
            Database.UpdateUserInDatabaseWithNewUserData(Authentication._currentUser);
            RegisterInterestsInDatabase();
            DisplayAlert("Melding", "Je gegevens zijn aangepast", "OK");
            ClearTextFromEntries();
            UpdatePlaceholders();
        }
        else
        {
            DisplayAlert("Er is iets verkeerd gegaan...", "Vul alle gegevens in", "OK");
        }
    }

    private void UpdatePlaceholders()
    {

        Firstname.Placeholder = Authentication._currentUser.firstName;
        Middlename.Placeholder = Authentication._currentUser.middleName;
        Lastname.Placeholder = Authentication._currentUser.lastName;
        Birthdate.Date = (DateTime)Authentication._currentUser.birthDay;
        Bio.Placeholder = Authentication._currentUser.bio;
        Education.Placeholder = Authentication._currentUser.major;
    }

    private void ClearTextFromEntries()
    {
        Firstname.Text = "";
        Middlename.Text = "";
        Lastname.Text = "";
        Bio.Text = "";
        Education.Text = "";
    }

    private void RegisterInterestsInDatabase()
    {
        foreach (var interest in interesses)
        {
            Database.RegisterInterestInDatabase(Authentication._currentUser.email, interest);
        }
    }

    private void UpdateUserPropertiesPrepareForUpdateQuery()
    {
        if (Authentication._currentUser.firstName != Firstname.Text && Firstname.Text != null && Firstname.Text != "") Authentication._currentUser.firstName = Firstname.Text;
        if (Authentication._currentUser.middleName != Middlename.Text && Middlename.Text != null && Middlename.Text != "") Authentication._currentUser.middleName = Middlename.Text;
        if (Authentication._currentUser.lastName != Lastname.Text && Lastname.Text != null && Lastname.Text != "") Authentication._currentUser.lastName = Lastname.Text;
        if (Authentication._currentUser.major != Education.Text && Education.Text != null && Education.Text != "") Authentication._currentUser.major = Education.Text;
        if (Authentication._currentUser.birthDay != Birthdate.Date) Authentication._currentUser.birthDay = Birthdate.Date;
        if (Authentication._currentUser.bio != Bio.Text && Bio.Text != null && Bio.Text != "") Authentication._currentUser.bio = Bio.Text;
        if (Authentication._currentUser.gender != Gender.SelectedItem.ToString()) Authentication._currentUser.gender = Gender.SelectedItem.ToString();
        if (Authentication._currentUser.preference != Preference.SelectedItem.ToString()) Authentication._currentUser.preference = Preference.SelectedItem.ToString();   
    }

    private void FirstnameTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Firstname.Text != "")
        {
            if (!CheckIfTextIsOnlyLetters(Firstname.Text))
            {
                firstname = false;
                lblFirstname.Text = "Voornaam mag alleen letters bevatten";
                lblFirstname.TextColor = ErrorColor;
            }
            else
            {
                firstname = true;
                lblFirstname.Text = "Voornaam";
                lblFirstname.TextColor = default;
                Firstname.Text = Firstname.Text.First().ToString().ToUpper() + Firstname.Text[1..].ToLower();
            }
        }
    }
    private void MiddlenameTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Middlename.Text != "")
        {
            if (!CheckIfTextIsOnlyLetters(Middlename.Text))
            {
                middlename = false;
                lblMiddlename.Text = "Tussenvoegsel mag alleen letters bevatten";
                lblMiddlename.TextColor = ErrorColor;
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
            if (!CheckIfTextIsOnlyLetters(Lastname.Text))
            {
                lastname = false;
                lblLastname.Text = "Achternaam mag alleen letters bevatten";
                lblLastname.TextColor = ErrorColor;
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
        
    }
    private void EducationTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Education.Text != "")
        {
            if (!CheckIfTextIsOnlyLettersAndSpaces(Education.Text))
            {
                education = false;
                lblEducation.Text = "Opleiding mag alleen letters bevatten";
                lblEducation.TextColor = ErrorColor;
            }
            else
            {
                education = true;
                lblEducation.Text = "Opleiding";
                lblEducation.TextColor = default;
            }
        }
    }
    private bool CheckIfTextIsOnlyLettersAndSpaces(string text)
    {
        foreach (char c in text)
        {
            if (!char.IsLetter(c) && c != ' ' && c != '-')
            {
                return false;
            }
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckIfTextIsOnlyLetters(string text)
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
                ListInterests.ItemsSource = null;
                ListInterests.ItemsSource = interesses;
            }
            else
            {
                InterestSelection.Title = "Je hebt deze interesse al toegevoegd";
                InterestSelection.TitleColor = ErrorColor;
            }
        }
        else
        {
            Color ErrorColor = new Color(238, 75, 43);
            InterestSelection.Title = "Je kunt maximaal 5 interesses selecteren";
            InterestSelection.TitleColor = ErrorColor;
        }
    }
    private void DeleteInterest(object sender, EventArgs e)
    {
        if (ListInterests.SelectedItem != null) {
            var item = ListInterests.SelectedItem.ToString();
            interesses.Remove(item);
            ListInterests.ItemsSource = null;
            ListInterests.ItemsSource = interesses;
            ListInterests.SelectedItem = null;
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
                lblBirthdate.Text = "Je moet minimaal 18 jaar zijn";
                lblBirthdate.BackgroundColor = ErrorColor;
            }
    }

    private void ListInterestsItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
                if (ListInterests.SelectedItem != null)
        {
            InterestSelection.Title = "Interesse";
            InterestSelection.TitleColor = default;
            var interest = ListInterests.SelectedItem.ToString();
            Database.RemoveInterestOfUser(Authentication._currentUser.email, interest);
            interesses.Remove(interest);
            ListInterests.ItemsSource = null;
            ListInterests.ItemsSource = interesses;
            ListInterests.SelectedItem = null;
        }
    }
}