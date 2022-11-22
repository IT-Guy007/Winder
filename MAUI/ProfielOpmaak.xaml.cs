namespace MAUI;

public partial class ProfielOpmaak : ContentPage
{
    Button b = new Button();
    public ProfielOpmaak()
	{
		InitializeComponent();
        b.Clicked += wijzigProfielGegevens;
    }
        private void wijzigProfielGegevens(object sender, EventArgs e)
    {
        
    }

}