using System.Data.SqlClient;
using DataModel;

namespace MAUI;


public partial class MainPage : ContentPage {


public partial class MainPage : ContentPage
{

   
    public MainPage()
    {
        InitializeComponent();
    }
    
 
    }
    
    protected override async void OnAppearing() {
        
        // wait for 4 seconds
        await Task.Delay(3000);
        Console.WriteLine("App started");
        Authentication.Initialize();
        Database.Initialize();

        Database db = new Database();
        try {
            
            Console.WriteLine("Testing database connection");
            Database.OpenConnection();
            Console.WriteLine("Successful connection");
        } catch (SqlException se) {
            Console.WriteLine("Failed to open connection");
            Console.WriteLine(se.Message);
            Console.WriteLine(se.StackTrace);
        }
        Database.CloseConnection();
        
        
        //Check if user was previously logged in
        var userEmail = await SecureStorage.Default.GetAsync("email");
        if (!String.IsNullOrWhiteSpace(userEmail)) {
            Console.WriteLine("Found user who was logged in, restoring session");
            db.UpdateLocalUserFromDatabase(userEmail);
            Console.WriteLine("Restored");
            Authentication.Get5Profiles(userEmail);
        } else {
            Console.WriteLine("No user found");
        }
        
        if (!String.IsNullOrWhiteSpace(Authentication._currentUser.email)) {
            Console.WriteLine("Pusing new MatchPage");
            await Navigation.PushAsync(new MatchPage());
        } else {
            Console.WriteLine("Pushing the startPage");
            await Navigation.PushAsync(new StartPage());
        }

    }

}


