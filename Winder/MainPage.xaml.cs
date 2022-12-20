using MAUI;
using System;
using System.Threading.Tasks;
namespace MAUI;


public partial class MainPage : ContentPage
{

   
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


