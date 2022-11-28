


namespace MAUI;
public partial class LoginScherm : ContentPage
{
    Button b = new Button();
    Button b2 = new Button();
   
    public LoginScherm()
    {
        InitializeComponent();
        b.Clicked += Inlog;
        b2.Clicked += WachtwoordVergeten;
        
    }
    private void Inlog(object sender, EventArgs e) {

    }
    private void WachtwoordVergeten(object sender, EventArgs e) {
        
    }
   
}