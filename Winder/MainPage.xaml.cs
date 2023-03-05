using System.Data.SqlClient;
using Controller;
using DataModel;
using MAUI;
using Microsoft.Extensions.Configuration;
using Winder.Repositories;

namespace Winder;


public partial class MainPage {

    private bool connectionsucceeded = true;
    private bool displayresult;

    private UserRepository _userRepository;
    private IConfigurationRoot _configuration;
    public MainPage() {
        InitializeComponent();
        
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _userRepository = new UserRepository(configuration);
        
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
    }

    protected override async void OnAppearing() {
        
        Console.WriteLine("App started");

        try{
            Console.WriteLine("Testing database connection");
            
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
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
                User.CurrentUser = _userRepository.GetUserFromDatabase(userEmail);
                Console.WriteLine("Restored");
            } else {
                Console.WriteLine("No user found");
                await Navigation.PushAsync(new StartPage());
            }

            if (!(User.CurrentUser == null)) {
                Console.WriteLine("Pushing new MatchPage");
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


