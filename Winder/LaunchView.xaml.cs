using Microsoft.Maui.Dispatching;
using System.Windows.Threading;

namespace Winder;

public partial class LaunchView : ContentPage
{
    private DispatcherTimer timer;

    public LaunchView()
    {
        InitializeComponent();

        // Set the logo image source
        logoImage.Source = new BitmapImage(new Uri("logo.png", UriKind.Relative));

        // Initialize the timer
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(3);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // Stop the timer
        timer.Stop();

        // Navigate to the "mainpage"
        this.NavigationService.Navigate(new Uri("mainpage.xaml", UriKind.Relative));
    }
}