using DataModel;

namespace MAUI;

public partial class MainPage : ContentPage {

    public MainPage() {
        InitializeComponent();
        
    }
    private void RegisterButton(object sender, EventArgs e) {
        Navigation.PushAsync(new RegisterPage());
    }

    private void LoginButton(object sender, EventArgs e) {
        Database b = new Database();
        b.GetUsersWhoLikedYou("s12@student.windesheim.nl");
        Navigation.PushAsync(new LoginPage());
    }
}