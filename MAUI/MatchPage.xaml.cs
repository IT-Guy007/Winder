using System.Drawing;
using System.Drawing.Printing;
using DataModel;
using Microsoft.Maui.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using Image = Microsoft.Maui.Controls.Image;
using Point = Microsoft.Maui.Graphics.Point;

namespace MAUI;

public partial class MatchPage : ContentPage
{

    private Image _imageButton;
    private Queue<Profile> profileQueue = new Queue<Profile>();

    private Image[] _images = new Image[10];
    private int selectedImage = 0;

    public MatchPage() {
        Title = "Make your match now!";

        StackLayout _verticalStackLayout = new StackLayout{Orientation = StackOrientation.Vertical};
        
        StackLayout ImageLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
        
        //Images
        if (profileQueue.Count == 0) {
            Image noMoreUsers = new Image() { Source = "nomorematches.png" };
            noMoreUsers.Aspect = Aspect.AspectFit;
            noMoreUsers.WidthRequest = 800;
            noMoreUsers.HeightRequest = 800;
            ImageLayout.Add(noMoreUsers);
            
        } else {
            
            //Image carousel

            //fullname
            
            //Age
            
            //Bio
            
            
            //Buttons
            
            //Undo
            
            //Cancel
            
            //Like

        }

        _verticalStackLayout.Add(ImageLayout);
        _verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        
        Content = _verticalStackLayout;
    }

}