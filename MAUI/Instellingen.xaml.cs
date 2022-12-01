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

    public void editDataBtn(object sender, EventArgs e)
    {
        setPreference();
    }


}