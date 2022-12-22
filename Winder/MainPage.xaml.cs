using MAUI;

namespace Winder;


public partial class MainPage {

   
    public MainPage()
    {
        InitializeComponent();
        

    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(4000); // wait for 4 seconds
        await Navigation.PushAsync(new StartPage());
    }
}


