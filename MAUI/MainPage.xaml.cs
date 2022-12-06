namespace MAUI;

public partial class MainPage : ContentPage {

    public MainPage() {
        InitializeComponent();
    }

    private void RegisterButton(object sender, EventArgs e) {
        Navigation.PushAsync(new RegisterPage());
    }

    private void LoginButton(object sender, EventArgs e) {
        Navigation.PushAsync(new MatchPage());
    }
}