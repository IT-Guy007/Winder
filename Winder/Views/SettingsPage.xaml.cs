using CommunityToolkit.Maui.Views;
using DataModel;
using MAUI;
using Microsoft.Maui.ApplicationModel.Communication;

namespace Winder;

public partial class SettingsPage {
    private const string PageName = "settingspage";
    public string OriginPage;
    public SettingsPage() {

        InitializeComponent();
        GetMinimaleLeeftijd();
      
        PlaceLocation();
        PlaceMinAge();
        PlaceMaxAge();

    }

    // puts the min and max age in the picker.
    private void GetMinimaleLeeftijd()
    {
        int[] leeftijd = new int[82];
        for (int i = 0; i < leeftijd.Length; i++)
        {
            leeftijd[i] = i + 18;

        }

        minimaleLeeftijd.ItemsSource = leeftijd;
        maximaleLeeftijd.ItemsSource = leeftijd;


    }
    
    //sets the location in the database
    private void SetLocation() {
        string location = Location.SelectedItem.ToString();
        if (location != null) Authentication.CurrentUser.SetSchool(location, Database.ReleaseConnection);
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
 
  
    //sets location in the picker what the user already has in the database
    private void PlaceLocation() {
        Location.SelectedItem = Authentication.CurrentUser.GetSchool(Database.ReleaseConnection);
        
        }
    //sets minimum age in the picker what the user already has in the database
    private void PlaceMinAge() {
        minimaleLeeftijd.SelectedItem = Authentication.CurrentUser.GetMinAge(Database.ReleaseConnection);
        
    }
    //sets maximum age in the picker what the user already has in the database
    private void PlaceMaxAge() {
        
        maximaleLeeftijd.SelectedItem = Authentication.CurrentUser.GetMaxAge(Database.ReleaseConnection);
        
    }
    //sets the minimum age of what the user chose in the database
    private void SetMinAge() {
        
        Authentication.CurrentUser.SetMinAge((int)minimaleLeeftijd.SelectedItem, Database.ReleaseConnection);

    }
    //sets the maximum age of what the user chose in the database
    private void SetMaxAge() {
        
       Authentication.CurrentUser.SetMaxAge((int)maximaleLeeftijd.SelectedItem, Database.ReleaseConnection);

    }
    private async void DeleteAccountButton(object sender, EventArgs e) {

        bool displayresult = await DisplayAlert("", "Weet u zeker dat u uw account wilt verwijderen?", "Ja", "Nee");
        if (displayresult) {
            Authentication.CurrentUser.DeleteUser(Database.ReleaseConnection);
            await Navigation.PushAsync(new MainPage());
        }

    }

    private async void logoutBtn(object sender, EventArgs e)
    {
        bool displayresult = await DisplayAlert("", "U wordt uitgelogd", "Ok", "Annuleren");
        if (displayresult)
        {
            SecureStorage.Default.Remove("Email");
            SecureStorage.Remove("Email");
            SecureStorage.RemoveAll();
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
               
                SetLocation();
                SetMinAge();
                SetMaxAge();
                foutLeeftijd.IsVisible = false;
                DisplayAlert("Melding", "Er zijn succesvol gegevens aangepast", "OK");
            }
            
        } catch {

           DisplayAlert("Melding", "Er zijn geen gegevens aangepast", "OK");
        }
    }
    // shows a popup where you can edit your password
    private void EditPasswordBtn(object sender, EventArgs e) {
        
        var popup = new EditPasswordPopUp();
        this.ShowPopup(popup);
    }

    private void MyProfile_Clicked(object sender, EventArgs e) {
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