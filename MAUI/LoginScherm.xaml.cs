


namespace MAUI;
public partial class InlogScherm : ContentPage
{
    Button b = new Button();
    Button b2 = new Button();
   
    public InlogScherm()
    {
        InitializeComponent();
        b.Clicked += Inlog;
        b2.Clicked += WachtwoordVergeten;
        
    }
    private void Inlog(object sender, EventArgs e)
    {

    }
    private void WachtwoordVergeten(object sender, EventArgs e)
    {
        
    }
   
}