using DataModel;

namespace MAUI;

public partial class Instellingen : ContentPage
{
    Database database = new Database();
    Authentication authentication = new Authentication();
    string email = Authentication._currentUser.email;

    public Instellingen()
    {
        InitializeComponent();
        getMinimaleLeeftijd();
        placePreference();
        placeLocation();

    }

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
    public void setPreference()
    {

        string preference = Preference.SelectedItem.ToString();
        database.insertPreference(email, preference);
    }

    public void setLocation()
    {
       
            string location = Location.SelectedItem.ToString();
            database.insertLocation(email, location);    
    }
    
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

    public void placePreference( )
    {
       string placePreference = database.placePreference(email);
        Preference.SelectedItem = placePreference;
    }
    
    public void placeLocation()
    {
        string placeLocation = database.placeLocation(email);
        Location.SelectedItem = placeLocation;
        
        }

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
                DisplayAlert("Melding", "Er zijn succesvol gegevens aangepast", "OK");
            }
           
            
        }
        catch
        {

           DisplayAlert("Melding", "Er zijn geen gegevens aangepast", "OK");
        }


    }
}