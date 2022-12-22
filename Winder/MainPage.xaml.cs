using MAUI;
using System.Data.SqlClient;
using DataModel;
namespace Winder;


public partial class MainPage : ContentPage {

    private bool connectionsucceeded = true;
    private bool displayresult;
    public MainPage()
    {
        InitializeComponent();
        

    }

    protected override async void OnAppearing()
    {
    


        // wait for 4 seconds
        await Task.Delay(3000);
        Console.WriteLine("App started");
        Authentication.Initialize();
        Database.GenerateConnection();
        Database db = new Database();
        
        


        

        try
        {

            Console.WriteLine("Testing database connection");
            Database.OpenConnection();
            Console.WriteLine("Successful connection");
        }
        catch (SqlException se)
        {
            Console.WriteLine("Failed to open connection");
            Console.WriteLine(se.Message);
            Console.WriteLine(se.StackTrace);
            displayresult = await DisplayAlert("", "Oeps, er is iets mis gegaan met de database connectie", "Probeer opnieuw", "Sluit de app");
            connectionsucceeded = false;
            if (displayresult)
            {
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                Application.Current.Quit();
            }
        }

        if (connectionsucceeded)
        {
            Database.CloseConnection();


            //Check if user was previously logged in
            var userEmail = await SecureStorage.Default.GetAsync("email");
            if (!String.IsNullOrWhiteSpace(userEmail))
            {
                Console.WriteLine("Found user who was logged in, restoring session");
                db.UpdateLocalUserFromDatabase(userEmail);
                Console.WriteLine("Restored");
                db.Get5Profiles(userEmail);
            }
            else
            {
                Console.WriteLine("No user found");
            }

            if (!String.IsNullOrWhiteSpace(Authentication._currentUser.email))
            {
                Console.WriteLine("Pusing new MatchPage");
                await Navigation.PushAsync(new MatchPage());
            }
            else
            {
                Console.WriteLine("Pushing the startPage");
                await Navigation.PushAsync(new StartPage());
            }
        }




    }
}


