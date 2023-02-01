using Controller;
using DataModel;

namespace Winder;

public partial class ProfileChange {
    public string OriginPage;
    private const string PageName = "profilepage";

    private UserController UserController;

    private readonly List<string> interests;
    private readonly Color errorColor;
    private byte[][] ProfilePictures { get; set;}
    private bool firstname = true;
    private bool middleName = true;
    private bool lastname = true;
    private bool birthday = true;
    private readonly bool preference = true;
    private readonly bool gender = true;
    private bool bio = true;
    private bool education = true;
    

    /// <summary>
    /// Default constructor, loads the data
    /// </summary>
    public ProfileChange() {
        interests = new List<string>();
        errorColor = new Color(255, 243, 5);
        ProfilePictures = new byte[6][];
        UserController = new UserController();
        
        InitializeComponent();
        FillPlaceholders();
        
    }
    //Fills the form inputs placeholders with the user data
    private void FillPlaceholders() {
        ProfilePictures = Authentication.CurrentUser.GetPicturesFromDatabase(Database.ReleaseConnection);
        Firstname.Placeholder = Authentication.CurrentUser.FirstName;
        Middlename.Placeholder = Authentication.CurrentUser.MiddleName;
        Lastname.Placeholder = Authentication.CurrentUser.LastName;
        Birthdate.Date = Authentication.CurrentUser.BirthDay;
        Bio.Placeholder = Authentication.CurrentUser.Bio;
        Education.Placeholder = Authentication.CurrentUser.Major;
        Gender.SelectedIndex = UserController.GetPreferenceFromUser(Authentication.CurrentUser.Gender);
        Preference.SelectedIndex = UserController.GetPreferenceFromUser(Authentication.CurrentUser.Preference);
        InterestSelection.ItemsSource = new InterestsModel().GetInterestsFromDataBase(Database.ReleaseConnection);
        ListInterests.ItemsSource = Authentication.CurrentUser.Interests;
        SetAllImageButtons();
    }
    private void SetAllImageButtons() {
        if (ProfilePictures != null) {
            if (ProfilePictures[0] != null) {
                byte[] scaledImage = UserController.ScaleImage(ProfilePictures[0],140,200);
                ProfileImage1.Source = ImageSource.FromStream(() => new MemoryStream(scaledImage));
                CloseButton1.IsVisible = true;
            } else ProfileImage1.Source = "plus.png";
            
            if (ProfilePictures[1] != null) {
                byte[] scaledImage = UserController.ScaleImage(ProfilePictures[1], 140, 200);
                ProfileImage2.Source = ImageSource.FromStream(() => new MemoryStream(scaledImage));
                CloseButton2.IsVisible = true;
            } else ProfileImage2.Source = "plus.png";
            
            if (ProfilePictures[2] != null) {
                byte[] scaledImage = UserController.ScaleImage(ProfilePictures[2], 140, 200);
                ProfileImage3.Source = ImageSource.FromStream(() => new MemoryStream(scaledImage));
                CloseButton3.IsVisible = true;
            }
            else ProfileImage3.Source = "plus.png";
            
            if (ProfilePictures[3] != null) {
                byte[] scaledImage = UserController.ScaleImage(ProfilePictures[3], 140, 200);
                ProfileImage4.Source = ImageSource.FromStream(() => new MemoryStream(scaledImage));
                CloseButton4.IsVisible = true;
            } else ProfileImage4.Source = "plus.png";
            
            if (ProfilePictures[4] != null) {
                byte[] scaledImage = UserController.ScaleImage(ProfilePictures[4], 140, 200);
                ProfileImage5.Source = ImageSource.FromStream(() => new MemoryStream(scaledImage));
                CloseButton5.IsVisible = true;
            } else ProfileImage5.Source = "plus.png";
            
            if (ProfilePictures[5] != null) {
                byte[] scaledImage = UserController.ScaleImage(ProfilePictures[5], 140, 200);
                ProfileImage6.Source = ImageSource.FromStream(() => new MemoryStream(scaledImage));
                CloseButton6.IsVisible = true;
            } else ProfileImage6.Source = "plus.png";
        }
    }
    
    //Changes the userdata en updates the form
    private void ChangeUserData(object sender, EventArgs e) {
        if (firstname && middleName && lastname && birthday  && preference && gender && bio && education ) {
            UpdateUserPropertiesPrepareForUpdateQuery();
            Authentication.CurrentUser.UpdateUserDataToDatabase(Database.ReleaseConnection);
            Authentication.CurrentUser.DeleteAllPhotosFromDatabase(Database.ReleaseConnection);
            Authentication.CurrentUser.InsertAllPhotosInDatabase(ProfilePictures,Database.ReleaseConnection);
            RegisterInterestsInDatabase();
            DisplayAlert("Melding", "Je gegevens zijn aangepast", "OK");
            ClearTextFromEntries();
            UpdatePlaceholders();
        } else {
            DisplayAlert("Er is iets verkeerd gegaan...", "Vul alle gegevens in", "OK");
        }
    }

    //Updates the placeholders value after a change has been made
    private void UpdatePlaceholders() {

        Firstname.Placeholder = Authentication.CurrentUser.FirstName;
        Middlename.Placeholder = Authentication.CurrentUser.MiddleName;
        Lastname.Placeholder = Authentication.CurrentUser.LastName;
        Birthdate.Date = Authentication.CurrentUser.BirthDay;
        Bio.Placeholder = Authentication.CurrentUser.Bio;
        Education.Placeholder = Authentication.CurrentUser.Major;
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
    private void RegisterInterestsInDatabase() {
        foreach (var interest in interests) {
            Authentication.CurrentUser.SetInterestInDatabase(interest, Database.ReleaseConnection);
        }
    }
    //Update the users data 
    private void UpdateUserPropertiesPrepareForUpdateQuery() {
        if (Authentication.CurrentUser.FirstName != Firstname.Text && !string.IsNullOrEmpty(Firstname.Text)) Authentication.CurrentUser.FirstName = Firstname.Text;
        if (Authentication.CurrentUser.MiddleName != Middlename.Text && !string.IsNullOrEmpty(Middlename.Text)) Authentication.CurrentUser.MiddleName = Middlename.Text;
        if (Authentication.CurrentUser.LastName != Lastname.Text && !string.IsNullOrEmpty(Lastname.Text)) Authentication.CurrentUser.LastName = Lastname.Text;
        if (Authentication.CurrentUser.Major != Education.Text && !string.IsNullOrEmpty(Education.Text)) Authentication.CurrentUser.Major = Education.Text;
        if (Authentication.CurrentUser.BirthDay != Birthdate.Date) Authentication.CurrentUser.BirthDay = Birthdate.Date;
        if (Authentication.CurrentUser.Bio != Bio.Text && !string.IsNullOrEmpty(Bio.Text)) Authentication.CurrentUser.Bio = Bio.Text;
        if (Authentication.CurrentUser.Gender != Gender.SelectedItem.ToString()) Authentication.CurrentUser.Gender = Gender.SelectedItem.ToString() ?? string.Empty;
        if (Authentication.CurrentUser.Preference != Preference.SelectedItem.ToString()) Authentication.CurrentUser.Preference = Preference.SelectedItem.ToString() ?? string.Empty;
    }
    //Checks if the firstname input is valid
    private void FirstnameTextChanged(object sender, TextChangedEventArgs e) {
        if (!string.IsNullOrWhiteSpace(Firstname.Text)) {
            if (!CheckIfTextIsOnlyLetters(Firstname.Text)) {
                firstname = false;
                lblFirstname.Text = "Voornaam mag alleen letters bevatten";
                lblFirstname.TextColor = errorColor;
            } else {
                firstname = true;
                lblFirstname.Text = "Voornaam";
                lblFirstname.TextColor = default;
                Firstname.Text = Firstname.Text.First().ToString().ToUpper() + Firstname.Text[1..].ToLower();
            }
        }
    }
    //Checks if the MiddleName input is valid
    private void MiddlenameTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Middlename.Text != "")
        {
            if (!CheckIfTextIsOnlyLetters(Middlename.Text))
            {
                middleName = false;
                lblMiddlename.Text = "Tussenvoegsel mag alleen letters bevatten";
                lblMiddlename.TextColor = errorColor;
            }
            else
            {
                middleName = true;
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
                lblLastname.TextColor = errorColor;
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
                lblEducation.TextColor = errorColor;
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
        switch (OriginPage)
        {
            case "matchpage":
                Navigation.PushAsync(new MatchPage());
                break;
            case "settingspage":
                Navigation.PushAsync(new SettingsPage());
                break;
            case "chatpage":
            Navigation.PushAsync(new ChatsViewPage());
                break;
        }
    }

    private void ChatButton_Clicked(object sender, EventArgs e) {
        ChatsViewPage chatsViews = new ChatsViewPage {
            OriginPage = PageName
        };
        Navigation.PushAsync(chatsViews);
    }

    private void matchButton_Clicked(object sender, EventArgs e)
    {
        MatchPage matchpage = new MatchPage {
            OriginPage = PageName
        };
        Navigation.PushAsync(matchpage);
    }

    private void Settings_Clicked(object sender, EventArgs e) {
        SettingsPage settings = new SettingsPage {
            OriginPage = PageName
        };
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
            ImageButton clickedImageButton = (ImageButton)sender;
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
            FileStream fileStream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
            BinaryReader binary = new BinaryReader(fileStream);
            byte[] imageArr = binary.ReadBytes((int)fileStream.Length);
            string imageButtonId = clickedImageButton.AutomationId;
            TurnOnVisibilityCloseButton(imageButtonId);
            ProfilePictures[int.Parse(imageButtonId)] = imageArr;
            byte[] scaledImage = UserController.ScaleImage(imageArr, 140, 200);
            clickedImageButton.Source = ImageSource.FromStream(() => new MemoryStream(scaledImage));
        } catch (Exception ex) {
            Console.WriteLine("Error picking profilefoto");
            Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.StackTrace);
            await DisplayAlert("Error", "Er is iets fout gegaan", "Ok");
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
        if (InterestSelection.SelectedItem != null && interests.Count < 5)
        {
            InterestSelection.Title = "Interesse";
            InterestSelection.TitleColor = default;
            if (!interests.Contains(InterestSelection.SelectedItem.ToString()))
            {
                interests.Add(InterestSelection.SelectedItem.ToString());
                ListInterests.ItemsSource = null;
                ListInterests.ItemsSource = interests;
            }
            else
            {
                InterestSelection.Title = "Je hebt deze interesse al toegevoegd";
                InterestSelection.TitleColor = errorColor;
            }
        }
        else
        {
            InterestSelection.Title = "Je kunt maximaal 5 interests selecteren";
            InterestSelection.TitleColor = errorColor;
        }
    }

    //Checks if selected birthdate is a birthdate that is 18 years or older
    private void DateOfBirthSelectedDate(object sender, DateChangedEventArgs e) {
        DateTime today = DateTime.Today;
        int age = today.Year - Birthdate.Date.Year;
        if (Birthdate.Date > today.AddYears(-age)) age--;
        if (age >= 18) {
            birthday = true;
            lblBirthdate.Text = "Leeftijd : " + age;
            lblBirthdate.BackgroundColor = default;
        } else {
            birthday = false;
            lblBirthdate.Text = "Je moet minimaal 18 jaar zijn";
            lblBirthdate.BackgroundColor = errorColor;
        }
    }
    //Check if an item has been selected and delete the selected item of ListInterests
    private void ListInterestsItemSelected(object sender, SelectedItemChangedEventArgs e) {
        if (ListInterests.SelectedItem == null) return;
        InterestSelection.Title = "Interesse";
        InterestSelection.TitleColor = default;
        var interest = ListInterests.SelectedItem.ToString();
        if (interest != null) {
            Authentication.CurrentUser.DeleteInterestInDatabase(interest,Database.ReleaseConnection);
            interests.Remove(interest);
        }

        ListInterests.ItemsSource = null;
        ListInterests.ItemsSource = interests;
        ListInterests.SelectedItem = null;
    }

    private void BioTextChanged(object sender, TextChangedEventArgs e)
    {
        if (Bio.Text != "")
        {
            if (!CheckIfTextIsOnlyLettersAndSpaces(Bio.Text))
            {
                bio = false;
                lblBio.Text = "Bio mag alleen letters bevatten";
                lblBio.TextColor = errorColor;
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
        ImageButton imageButton = (ImageButton)sender;
        ProfilePictures[int.Parse(imageButton.AutomationId)] = null;
        switch (imageButton.AutomationId)
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
        }
    }
}