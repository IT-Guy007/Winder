using DataModel;

namespace MAUI;

public partial class Instellingen : ContentPage
{
    Database database = new Database();
    Authentication _authentication = new Authentication();


    public Instellingen()
    {
        InitializeComponent();
        getMinimaleLeeftijd();
        placePreference();
        placeLocation();
        placeMinAge();
        placeMaxAge();


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
        database.insertPreference(_authentication._currentUser.email, preference);
    }

    public void setLocation()
    {
        
        string location = Location.SelectedItem.ToString();
            database.insertLocation(_authentication._currentUser.email, location);    
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
        
        string placePreference = database.placePreference(_authentication._currentUser.email);
        Preference.SelectedItem = placePreference;
    }
    
    public void placeLocation()
    {
        
        string placeLocation = database.placeLocation(_authentication._currentUser.email);
        Location.SelectedItem = placeLocation;
        
        }

    public void placeMinAge()
    {
        
        int placeMinAge = database.placeMinAge(_authentication._currentUser.email);
            minimaleLeeftijd.SelectedItem = placeMinAge;
        
    }
    
    public void placeMaxAge()
    {
       
        int placeMaxAge = database.placeMaxAge(_authentication._currentUser.email);
        maximaleLeeftijd.SelectedItem = placeMaxAge;
        
    }

    public void setMinAge()
    {
       
        int minAge = (int)minimaleLeeftijd.SelectedItem;
        database.insertMinAge(_authentication._currentUser.email, minAge);

    }

    public void setMaxAge()
    {
       
        int maxAge = (int)maximaleLeeftijd.SelectedItem;
        database.insertMaxAge(_authentication._currentUser.email, maxAge);

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
}