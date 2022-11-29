namespace MAUI;

public partial class MainPage : ContentPage {

    public MainPage() {
        InitializeComponent();
        
    }

<<<<<<< HEAD
    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);

        
=======
    private void RegisterButton(object sender, EventArgs e) {
        Navigation.PushAsync(new RegisterForm());
>>>>>>> 71666a27444e2bd04c4981139628d4e1bfc6b2c1
    }

    private void LoginButton(object sender, EventArgs e) {
        Navigation.PushAsync(new LoginScherm());
    }
}