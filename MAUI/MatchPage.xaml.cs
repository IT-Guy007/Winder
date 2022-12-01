using DataModel;

namespace MAUI;

public partial class MatchPage : ContentPage
{
    private  Authentication _authentication= new Authentication();
    
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
            Image profileImage = new Image() { Source = ImageSource.FromResource(_authentication._currentUser.profilePicture.ToString())};
            profileImage.Aspect = Aspect.AspectFit;
            profileImage.WidthRequest = 800;
            profileImage.HeightRequest = 800;
            _verticalStackLayout.Add(profileImage);
            
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