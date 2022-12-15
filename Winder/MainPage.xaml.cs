using MAUI;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoreFoundation;
using DataModel;

namespace MAUI;


public partial class MainPage : ContentPage
{

   
    public MainPage()
    {
        InitializeComponent();
        
    }
    
    protected override async void OnAppearing() {
        
        // wait for 4 seconds
        await Task.Delay(4000);
        Console.WriteLine("App started");
        Authentication.Initialize();
        
        
        try {
            Database db = new Database();
            Console.WriteLine("Testing database connection");
            db.OpenConnection();
            Console.WriteLine("Successful connection");
        } catch (SqlException se) {
            Console.WriteLine("Failed to open connection");
            Console.WriteLine(se.Message);
            Console.WriteLine(se.StackTrace);
        }
        
        //Check if user was previously logged in
        var userEmail = await SecureStorage.Default.GetAsync("MyKey");
        if (!String.IsNullOrWhiteSpace(userEmail)) {
            Console.WriteLine("Found user who was logged in, restoring session");
            Database db = new Database();
            db.UpdateLocalUserFromDatabase(userEmail);
            Console.WriteLine("Restored");
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
        //base.OnAppearing();
    }
}


