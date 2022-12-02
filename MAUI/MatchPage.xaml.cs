using System.Drawing.Printing;

namespace MAUI;

public partial class MatchPage : ContentPage {
    private VerticalStackLayout _verticalStackLayout = new VerticalStackLayout();
    private LinearGradientBrush _linearGradientBrush;
    private Image[] _images = new Image[10];
    private Button _imageButton;
    private int selectedImage = 0;

    public MatchPage() {
        Title = "Make your match now!";

        _verticalStackLayout.Margin = 20;
        _verticalStackLayout.WidthRequest = 800;

        if (_images.Length == 0) {
            _imageButton = new Button {
                ImageSource = "Resources/Images/NoMoreMatches.png"
            };
        } else {
            _imageButton = new Button {
                ImageSource = _images[selectedImage].Source,
            };
        };



        _linearGradientBrush = new LinearGradientBrush() {
            StartPoint = new Point(500, 0),
            EndPoint = new Point(500,2000),
            GradientStops = new GradientStopCollection() {
                new GradientStop() { Color = Color.FromHex("#000000")},
                new GradientStop() { Color = Color.FromHex("#FFFFFF")}
            }
            
            
        };

      
        _verticalStackLayout.IsVisible = true;
        Content.Background = _linearGradientBrush;
        Content = new VerticalStackLayout {
            IsVisible = true,
            Children = {
                _verticalStackLayout
            }
        };


    }

}