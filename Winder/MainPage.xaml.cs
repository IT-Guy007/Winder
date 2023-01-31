using System.Data.SqlClient;
using DataModel;
using MAUI;

namespace Winder;


public partial class MainPage {

    private bool connectionsucceeded = true;
    private bool displayresult;
    public MainPage() {
        InitializeComponent();
        
    }

    protected override async void OnAppearing() {
        
        Console.WriteLine("App started");

        try{
            Console.WriteLine("Testing database connection");
            Database.InitializeReleaseConnection();
            Console.WriteLine("Successful connection");
        }
        catch (SqlException se) {
            Console.WriteLine("Failed to open connection");
            Console.WriteLine(se.Message);
            Console.WriteLine(se.StackTrace);
            displayresult = await DisplayAlert("", "Oeps, er is iets mis gegaan met de database connectie", "Probeer opnieuw", "Sluit de app");
            connectionsucceeded = false;
            if (displayresult) {
                await Navigation.PushAsync(new StartPage());
            } else {
                Application.Current.Quit();
            }
        }

        if (connectionsucceeded) {
            
            //Check if user was previously logged in
            var userEmail = await SecureStorage.Default.GetAsync("Email");
            if (!String.IsNullOrWhiteSpace(userEmail))
            {
                Console.WriteLine("Found user who was logged in, restoring session");
                Authentication.CurrentUser = new User().GetUserFromDatabase(userEmail, Database.ReleaseConnection);
                Console.WriteLine("Restored");
            } else {
                Console.WriteLine("No user found");
                await Navigation.PushAsync(new StartPage());
            }

            if (!String.IsNullOrWhiteSpace(Authentication.CurrentUser.Email)) {
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


