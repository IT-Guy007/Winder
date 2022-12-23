
using DataModel;
using Microsoft.Maui.Controls;
using System.ComponentModel.DataAnnotations;
using Winder;
using System.Drawing.Drawing2D;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.PlatformConfiguration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
namespace MAUI;

public partial class ProfileChange : ContentPage
{
    public string originPage;
    private const string pageName = "profilepage";

    List<string> interesses = new List<string>();
    Database Database = new Database();
    Microsoft.Maui.Graphics.Color ErrorColor = new Microsoft.Maui.Graphics.Color(255, 243, 5);
    private byte[][] profilePictures { get; set;}
    private bool firstname = true;
    private bool middlename = true;
    private bool lastname = true;
    private bool birthday = true;
    private bool preference = true;
    private bool gender = true;
    private bool bio = true;
    private bool education = true;
    //Load all necessary components to the page

    public ProfileChange()
    {
        InitializeComponent();
        profilePictures = new byte[6][];
        LoadUserFromDatabaseInForm();
        InterestSelection.ItemsSource = Database.GetInterestsFromDataBase();
        interesses = Database.LoadInterestsFromDatabaseInListInteresses(Authentication._currentUser.email);
        ListInterests.ItemsSource = interesses;
    }
    //Fills the form inputs placeholders with the user data
    private void LoadUserFromDatabaseInForm()
    {
        if (Authentication._currentUser != null)
        {
            profilePictures = Database.GetPicturesFromDatabase(Authentication._currentUser.email);
            SetAllImageButtons();
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
    private void SetAllImageButtons()
    {
        if (profilePictures != null)
        {
            if (profilePictures[0] != null)
            {
                byte[] ScaledImage = ScaleImage(profilePictures[0],140,200);
                ProfileImage1.Source = ImageSource.FromStream(() => new MemoryStream(ScaledImage));
                CloseButton1.IsVisible = true;
            }
            else ProfileImage1.Source = "plus.png";
            if (profilePictures[1] != null)
            {
                byte[] ScaledImage = ScaleImage(profilePictures[1], 140, 200);
                ProfileImage2.Source = ImageSource.FromStream(() => new MemoryStream(ScaledImage));
                CloseButton2.IsVisible = true;
            }
            else ProfileImage2.Source = "plus.png";
            if (profilePictures[2] != null)
            {
                byte[] ScaledImage = ScaleImage(profilePictures[2], 140, 200);
                ProfileImage3.Source = ImageSource.FromStream(() => new MemoryStream(ScaledImage));
                CloseButton3.IsVisible = true;
            }
            else ProfileImage3.Source = "plus.png";
            if (profilePictures[3] != null)
            {
                byte[] ScaledImage = ScaleImage(profilePictures[3], 140, 200);
                ProfileImage4.Source = ImageSource.FromStream(() => new MemoryStream(ScaledImage));
                CloseButton4.IsVisible = true;
            }
            else ProfileImage4.Source = "plus.png";
            if (profilePictures[4] != null)
            {
                byte[] ScaledImage = ScaleImage(profilePictures[4], 140, 200);
                ProfileImage5.Source = ImageSource.FromStream(() => new MemoryStream(ScaledImage));
                CloseButton5.IsVisible = true;
            }
            else ProfileImage5.Source = "plus.png";
            if (profilePictures[5] != null)
            {
                byte[] ScaledImage = ScaleImage(profilePictures[5], 140, 200);
                ProfileImage6.Source = ImageSource.FromStream(() => new MemoryStream(ScaledImage));
                CloseButton6.IsVisible = true;
            }
            else ProfileImage6.Source = "plus.png";
        }
    }
public byte[] ScaleImage(byte[] bytes, int width, int height)
{
    using (MemoryStream ms = new MemoryStream(bytes))
    {
        using (Bitmap image = new Bitmap(ms))
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, 0, 0, width, height);
            }

            using (MemoryStream output = new MemoryStream())
            {
                resizedImage.Save(output, image.RawFormat);
                return output.ToArray();
            }
        }
    }
}
//Gets the preference of user
private int GetPreferenceFromUser()
    {
        if (Authentication._currentUser.preference == "Man") return 1;
        if (Authentication._currentUser.preference == "Vrouw") return 2;
        return 0;
    }
    //Gets the gender of user
    private int GetGenderFromUser()
    {
        if (Authentication._currentUser.gender == "Man") return 1;
        if (Authentication._currentUser.gender == "Vrouw") return 2;
        return 0;
    }
    //Changes the userdata en updates the form
    private void ChangeUserData(object sender, EventArgs e)
    {
        if (firstname == true && middlename == true && lastname == true && birthday == true && preference == true && gender == true && bio == true && education == true)
        {
            UpdateUserPropertiesPrepareForUpdateQuery();
            Database.UpdateUserInDatabaseWithNewUserData(Authentication._currentUser);
            Database.DeleteAllPhotosFromDatabase(Authentication._currentUser.email);
            InsertAllPhotosInDatabase(Authentication._currentUser);
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

    private void InsertAllPhotosInDatabase(User currentUser)
    {
        if (profilePictures != null)
        {
            for (int i = 0; i < profilePictures.Length; i++)
            {
                if (profilePictures[i] != null)
                {
                    Database.InsertPictureInDatabase(currentUser.email, profilePictures[i]);
                }
            }
        }
    }
    private byte[] ConvertImageSourceToByteArray(ImageSource source)
    {
        var stream = source as StreamImageSource;
        if (stream != null)
        {
            var task = stream.Stream.Invoke(CancellationToken.None);
            task.Wait();
            var s = task.Result;
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                s.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }
        return null;
    }


    //Updates the placeholders value after a change has been made
    private void UpdatePlaceholders()
    {

        Firstname.Placeholder = Authentication._currentUser.firstName;
        Middlename.Placeholder = Authentication._currentUser.middleName;
        Lastname.Placeholder = Authentication._currentUser.lastName;
        Birthdate.Date = (DateTime)Authentication._currentUser.birthDay;
        Bio.Placeholder = Authentication._currentUser.bio;
        Education.Placeholder = Authentication._currentUser.major;
    }
    //Clears all text from input after profile change
    private void ClearTextFromEntries()
    {
        Firstname.Text = "";
        Middlename.Text = "";
        Lastname.Text = "";
        Bio.Text = "";
        Education.Text = "";
    }
    //Adds all interests to users list of interests
    private void RegisterInterestsInDatabase()
    {
        foreach (var interest in interesses)
        {
            Database.RegisterInterestInDatabase(Authentication._currentUser.email, interest);
        }
    }
    //Update the users data 
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
    //Checks if the firstname input is valid
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
    //Checks if the middlename input is valid
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
    //Checks if the lastname input is valid
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
    //Checks if the education input is valid
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


    private void Backbutton_Clicked(object sender, EventArgs e)
    {
        switch (originPage)
        {
            case "matchpage":
                Navigation.PushAsync(new MatchPage());
                break;
            case "settingspage":
                Navigation.PushAsync(new Instellingen());
                break;
            case "chatpage":
            Navigation.PushAsync(new ChatPage());
                break;
        }
    }

    private void ChatButton_Clicked(object sender, EventArgs e)
    {
        ChatPage chats = new ChatPage();
        chats.originPage = pageName;
        Navigation.PushAsync(chats);
    }

    private void matchButton_Clicked(object sender, EventArgs e)
    {
        MatchPage matchpage = new MatchPage();
        matchpage.originPage = pageName;
        Navigation.PushAsync(new MatchPage());
    }

    private void Settings_Clicked(object sender, EventArgs e)
    {
        Instellingen settings = new Instellingen();
        settings.originPage = pageName;
        Navigation.PushAsync(settings); 
    }

    //Checks if input has spaces, letters or dashes
    private bool CheckIfTextIsOnlyLettersAndSpaces(string text)
    {
        foreach (char c in text)
        {
            if (!char.IsLetter(c) && c != ' ' && c != '-' && c != '\n' && c != '\r')
            {
                return false;
            }
        }
        return true;
    }
    //Check if input only consists of letters
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
    private async void OnProfilePictureClicked(object sender, EventArgs e)
    {
        try
        {
            ImageButton ClickedImageButton = (ImageButton)sender;
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
            byte[] ScaledImage = null;
            FileStream fileStream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
            Stream stream = await image.OpenReadAsync();
            BinaryReader binary = new BinaryReader(fileStream);
            imageArr = binary.ReadBytes((int)fileStream.Length);
            string ImageButtonId = ClickedImageButton.AutomationId;
            TurnOnVisibilityCloseButton(ImageButtonId);
            profilePictures[int.Parse(ImageButtonId)] = imageArr;
            ScaledImage = ScaleImage(imageArr, 140, 200);
            ClickedImageButton.Source = ImageSource.FromStream(() => new MemoryStream(ScaledImage));
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "Er is iets fout gegaan", "Ok");
        }
    }

    private void TurnOnVisibilityCloseButton(string imageButtonId)
    {
        switch (imageButtonId)
        {
            case "0":
                CloseButton1.IsVisible = true;
                break;
            case "1":
                CloseButton2.IsVisible = true;
                break;
            case "2":
                CloseButton3.IsVisible = true;
                break;
            case "3":
                CloseButton4.IsVisible = true;
                break;
            case "4":
                CloseButton5.IsVisible = true;
                break;
            case "5":
                CloseButton6.IsVisible = true;
                break;
        }
    }

    //Handles the selected interests and adds them to the listview of interests
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
            InterestSelection.Title = "Je kunt maximaal 5 interesses selecteren";
            InterestSelection.TitleColor = ErrorColor;
        }
    }
    //Deletes the selected item in listview of interests
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

    //Checks if selected birthdate is a birthdate that is 18 years or older
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
    //Check if an item has been selected and delete the selected item of ListInterests
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

    private void BioTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Bio.Text != "")
        {
            if (!CheckIfTextIsOnlyLettersAndSpaces(Bio.Text))
            {
                bio = false;
                lblBio.Text = "Bio mag alleen letters bevatten";
                lblBio.TextColor = ErrorColor;
            }
            else
            {
                bio = true;
                lblBio.Text = "Bio";
                lblBio.TextColor = default;
            }
        }
    }

    private void ImageButtonClicked(object sender, EventArgs e)
    {
        ImageButton ImageButton = (ImageButton)sender;
        profilePictures[int.Parse(ImageButton.AutomationId)] = null;
        switch (ImageButton.AutomationId)
        {
            case "0":
                ProfileImage1.Source = "plus.png";
                CloseButton1.IsVisible = false;
                break;
            case "1":
                ProfileImage2.Source = "plus.png";
                CloseButton2.IsVisible = false;
                break;
            case "2":
                ProfileImage3.Source = "plus.png";
                CloseButton3.IsVisible = false;
                break;
            case "3":
                ProfileImage4.Source = "plus.png";
                CloseButton4.IsVisible = false;
                break;
            case "4":
                ProfileImage5.Source = "plus.png";
                CloseButton5.IsVisible = false;
                break;
            case "5":
                ProfileImage6.Source = "plus.png";
                CloseButton6.IsVisible = false;
                break;
            default:
                break;
        }
    }
}