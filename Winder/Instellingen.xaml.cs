using CommunityToolkit.Maui.Views;
using DataModel;
using Winder;

namespace MAUI;

public partial class Instellingen : ContentPage
{
    
    Database database = new Database();
    Authentication _authentication = new Authentication();
    string email = Authentication._currentUser.email;
    private const string pageName = "settingspage";
    public string originPage;
    public Instellingen()
    {
        InitializeComponent();
        getMinimaleLeeftijd();
        placePreference();
        placeLocation();
        placeMinAge();
        placeMaxAge();
        


    }

    // puts the min and max age in the picker.
    public void getMinimaleLeeftijd()
    {
        int[] leeftijd = new int[82];
        for (int i = 0; i < leeftijd.Length; i++)
        {
            leeftijd[i] = i + 18;

        }

        minimaleLeeftijd.ItemsSource = leeftijd;
        maximaleLeeftijd.ItemsSource = leeftijd;


    }
    //sets the preference in the database
    public void setPreference()
    {
        
        string preference = Preference.SelectedItem.ToString();
        database.InsertPreference(email, preference);
    }
    //sets the location in the database
    public void setLocation()
    {
        
        string location = Location.SelectedItem.ToString();
            database.InsertLocation(email, location);    
    }
    
    // checks if the min age is lower then the max age
    public bool checkIfMinAgeLowerThenMax()
    {
        try
        {
            int minAge = (int)minimaleLeeftijd.SelectedItem;
            int maxAge = (int)maximaleLeeftijd.SelectedItem;
            if (minAge > maxAge)
            {

                return false;
            }
            else
            {

                return true;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }
    //sets preference in the picker what the user already has in the database
    public void placePreference( )
    {
        
        string placePreference = database.GetPreference(email);
        Preference.SelectedItem = placePreference;
    }
    //sets location in the picker what the user already has in the database
    public void placeLocation()
    {
        
        string placeLocation = database.GetLocation(email);
        Location.SelectedItem = placeLocation;
        
        }
    //sets minimum age in the picker what the user already has in the database
    public void placeMinAge()
    {
        
        int placeMinAge = Database.GetMinAge(email);
            minimaleLeeftijd.SelectedItem = placeMinAge;
        
    }
    //sets maximum age in the picker what the user already has in the database
    public void placeMaxAge()
    {
       
        int placeMaxAge = Database.GetMaxAge(email);
        maximaleLeeftijd.SelectedItem = placeMaxAge;
        
    }
    //sets the minimum age of what the user chose in the database
    public void setMinAge()
    {
       
        int minAge = (int)minimaleLeeftijd.SelectedItem;
        database.SetMinAge(email, minAge);

    }
    //sets the maximum age of what the user chose in the database
    public void setMaxAge()
    {
       
        int maxAge = (int)maximaleLeeftijd.SelectedItem;
        database.SetMaxAge(email, maxAge);

    }
    //all the data that has been changed will be replaced in the database
    public void editDataBtn(object sender, EventArgs e)
    {
       
        try
        {
             checkIfMinAgeLowerThenMax();
            if (checkIfMinAgeLowerThenMax() == false)
            {

                foutLeeftijd.IsVisible = true;

            } else
            {
                setPreference();
                setLocation();
                setMinAge();
                setMaxAge();
                DisplayAlert("Melding", "Er zijn succesvol gegevens aangepast", "OK");
            }
           
            
        }
        catch
        {

           DisplayAlert("Melding", "Er zijn geen gegevens aangepast", "OK");
        }
    }
    // shows a popup where you can edit your password
    public void editPasswordBtn(object sender, EventArgs e)
    {
        
        var popup = new editPasswordPopUp();
        this.ShowPopup(popup);
    }

    private void MyProfile_Clicked(object sender, EventArgs e)
    {
        ProfileChange myProfile = new ProfileChange();
        
        myProfile.originPage = pageName;
        Navigation.PushAsync(myProfile);

    }

    private void Backbutton_Clicked(object sender, EventArgs e)
    {
        switch (originPage)
        {
            case "matchpage":
                MatchPage matchPage = new MatchPage();
                matchPage.backButtonVisible = true;
                Authentication.SetCurrentProfile();
                Navigation.PushAsync(new MatchPage());
                break;
            case "profilepage":
                Navigation.PushAsync(new ProfileChange());
                break;
        }

    }

    private void matchesbutton_Clicked(object sender, EventArgs e) {
        
    }

    private void matchPage_Clicked(object sender, EventArgs e)
    {
        MatchPage matchPage = new MatchPage();
        matchPage.originPage= pageName;
        Authentication.SetCurrentProfile();
        Navigation.PushAsync(matchPage);

    }
}