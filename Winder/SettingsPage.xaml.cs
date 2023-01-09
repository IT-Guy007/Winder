using CommunityToolkit.Maui.Views;
using DataModel;
using MAUI;
using Microsoft.Maui.ApplicationModel.Communication;

namespace Winder;

public partial class SettingsPage {
    private readonly Database database;
    private const string PageName = "settingspage";
    public string OriginPage;
    public SettingsPage() {
        database = new Database();

        InitializeComponent();
        GetMinimaleLeeftijd();
        PlacePreference();
        PlaceLocation();
        PlaceMinAge();
        PlaceMaxAge();
        


    }

    // puts the min and max age in the picker.
    private void GetMinimaleLeeftijd() {
        int[] leeftijd = new int[82];
        for (int i = 0; i < leeftijd.Length; i++) {
            leeftijd[i] = i + 18;

        }

        minimaleLeeftijd.ItemsSource = leeftijd;
        maximaleLeeftijd.ItemsSource = leeftijd;


    }
    //sets the preference in the database
    private void SetPreference() {
        string preference = Preference.SelectedItem.ToString();
        if (preference != null) database.InsertPreference(Authentication._currentUser.email, preference);
        Authentication._profileQueue = new Queue<Profile>();
    }
    
    //sets the location in the database
    private void SetLocation() {
        string location = Location.SelectedItem.ToString();
        if (location != null) database.InsertLocation(Authentication._currentUser.email, location);
    }
    
    // checks if the min age is lower then the max age
    private bool CheckIfMinAgeLowerThenMax() {
        try {
            int minAge = (int)minimaleLeeftijd.SelectedItem;
            int maxAge = (int)maximaleLeeftijd.SelectedItem;
            if (minAge > maxAge) {

                return false;
            }

            return true;

        } catch (Exception e) {
            Console.WriteLine("Error for checking if minage is lower then maxage");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);
            return false;
            
        }
    }
    //sets preference in the picker what the user already has in the database
    private void PlacePreference( ) {
        
        string placePreference = database.GetPreference(Authentication._currentUser.email);
        Preference.SelectedItem = placePreference;
    }
    //sets location in the picker what the user already has in the database
    private void PlaceLocation() {
        string placeLocation = database.GetLocation(Authentication._currentUser.email);
        Location.SelectedItem = placeLocation;
        
        }
    //sets minimum age in the picker what the user already has in the database
    private void PlaceMinAge() {
        
        int placeMinAge = database.GetMinAge(Authentication._currentUser.email);
            minimaleLeeftijd.SelectedItem = placeMinAge;
        
    }
    //sets maximum age in the picker what the user already has in the database
    private void PlaceMaxAge() {
       
        int placeMaxAge = database.GetMaxAge(Authentication._currentUser.email);
        maximaleLeeftijd.SelectedItem = placeMaxAge;
        
    }
    //sets the minimum age of what the user chose in the database
    private void SetMinAge() {
       
        int minAge = (int)minimaleLeeftijd.SelectedItem;
        database.SetMinAge(Authentication._currentUser.email, minAge);

    }
    //sets the maximum age of what the user chose in the database
    private void SetMaxAge() {
       
        int maxAge = (int)maximaleLeeftijd.SelectedItem;
        database.SetMaxAge(Authentication._currentUser.email, maxAge);

    }
    public async void deleteAccountbtn(object sender, EventArgs e)
    {

        bool displayresult = await DisplayAlert("", "Weet u zeker dat u uw account wilt verwijderen?", "Ja", "Nee");
        if (displayresult)
        {
            database.DeleteUser(Authentication._currentUser.email);
            SecureStorage.Default.Remove("email");
            SecureStorage.Remove("email");
            await Navigation.PushAsync(new MainPage());
        }

    }

    public async void logoutBtn(object sender, EventArgs e)
    {
        bool displayresult = await DisplayAlert("", "U wordt uitgelogd", "Ok", "Annuleren");
        if (displayresult)
        {
            SecureStorage.Default.Remove("email");
            SecureStorage.Remove("email");
            await Navigation.PushAsync(new StartPage());
        }
    }
    //all the data that has been changed will be replaced in the database
    private void EditDataBtn(object sender, EventArgs e) {
       
        try {
             CheckIfMinAgeLowerThenMax();
            if (CheckIfMinAgeLowerThenMax() == false) {

                foutLeeftijd.IsVisible = true;

            } else {
                SetPreference();
                SetLocation();
                SetMinAge();
                SetMaxAge();
                DisplayAlert("Melding", "Er zijn succesvol gegevens aangepast", "OK");
            }
            
        } catch {

           DisplayAlert("Melding", "Er zijn geen gegevens aangepast", "OK");
        }
    }
    // shows a popup where you can edit your password
    public void EditPasswordBtn(object sender, EventArgs e)
    {
        
        var popup = new EditPasswordPopUp();
        this.ShowPopup(popup);
    }

    private void MyProfile_Clicked(object sender, EventArgs e)
    {
        ProfileChange myProfile = new ProfileChange();
        
        myProfile.OriginPage = PageName;
        Navigation.PushAsync(myProfile);

    }

    private void Backbutton_Clicked(object sender, EventArgs e) {
        switch (OriginPage) {
            case "matchpage":
                MatchPage page = new MatchPage {
                    BackButtonVisible = true
                };
                Navigation.PushAsync(page);
                break;
            case "profilepage":
                Navigation.PushAsync(new ProfileChange());
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

    private void matchPage_Clicked(object sender, EventArgs e) {
        MatchPage page = new MatchPage {
            OriginPage = PageName
        };
        Navigation.PushAsync(page);

    }
}