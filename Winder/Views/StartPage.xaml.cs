using Winder;

namespace MAUI;

public partial class StartPage : ContentPage {

    public StartPage() {
        InitializeComponent();
        
    }
    private void RegisterButton(object sender, EventArgs e) {
        Navigation.PushAsync(new RegisterPage());
    }

    private void LoginButton(object sender, EventArgs e) {
        Navigation.PushAsync(new LoginPage());
    }
}