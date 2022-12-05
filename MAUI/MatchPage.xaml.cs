using DataModel;
using System.Drawing.Printing;

namespace MAUI;

public partial class MatchPage : ContentPage {
    private VerticalStackLayout _verticalStackLayout = new VerticalStackLayout();
    private LinearGradientBrush _linearGradientBrush;
    private Image[] _images = new Image[10];
    private Button _imageButton;
    private int selectedImage = 0;
    private User feedUser;

    public MatchPage() 
    {
        InitializeComponent();

        Database database = new Database();
        Authentication auth = new Authentication();

        //standaard persoon
        feedUser = (User)database.GetUserFromDatabase("s1168742@student.windesheim.nl");

        string feedUserName = feedUser.firstName + " " + feedUser.middleName + " " + feedUser.lastName;
        if (feedUser.middleName == null)
        {
            feedUserName = feedUser.firstName + " " + feedUser.lastName;
        }

        this.NameFeedUser.Text = feedUserName;
        this.AgeFeedUser.Text = auth.CalculateAge(feedUser.birthDay).ToString();
        this.SchoolFeedUser.Text = feedUser.school;
        this.MajorFeedUser.Text = feedUser.major;
        this.BioFeedUser.Text = feedUser.bio;

        GetImage(feedUser.profilePicture);
        
        //Title = "Make your match now!";

        //_verticalStackLayout.Margin = 20;
        //_verticalStackLayout.WidthRequest = 800;

        //if (_images.Length == 0) {
        //    _imageButton = new Button {
        //        ImageSource = "Resources/Images/NoMoreMatches.png"
        //    };
        //} else {
        //    _imageButton = new Button {
        //        ImageSource = _images[selectedImage].Source,
        //    };
        //};

        //_linearGradientBrush = new LinearGradientBrush() {
        //    StartPoint = new Point(500, 0),
        //    EndPoint = new Point(500,2000),
        //    GradientStops = new GradientStopCollection() {
        //        new GradientStop() { Color = Color.FromHex("#000000")},
        //        new GradientStop() { Color = Color.FromHex("#FFFFFF")}
        //    }
        //};

        //_verticalStackLayout.IsVisible = true;
        //Content.Background = _linearGradientBrush;
        //Content = new VerticalStackLayout {
        //    IsVisible = true,
        //    Children = {
        //        _verticalStackLayout
        //    }
        //};
    }

    public void GetImage(byte[] img)
    {
        using (var ms = new MemoryStream(img))
        {
            this.ImageFeedUser.Source = ImageSource.FromStream(() => ms);
        }
    }
    int swipeAmount = 0;
    private void OnSwipeLike(object sender, EventArgs e)
    {
        if (swipeAmount % 2 == 0)
        {
            OnLike(sender, e);
        }
        swipeAmount++;
    }

    private void OnLike(object sender, EventArgs e)
    {
        Database database = new Database();
        string emailCurrentUser = Authentication._currentUser.email;
        string emailLikedUser = feedUser.email;

        if (database.checkMatch(emailCurrentUser, emailLikedUser))
        {
            database.NewMatch(emailLikedUser, emailCurrentUser);
            database.deleteLikeOnMatch(emailCurrentUser, emailLikedUser);
        }
        else
        {
            database.NewLike(emailCurrentUser, emailLikedUser);
        }

        //en krijg een pop-up dat je een match hebt

        //en daarna door naar de volgende persoon
    }

    private void OnSwipeDislike(object sender, EventArgs e)
    {
        if (swipeAmount % 2 == 0)
        {
            OnDislike(sender, e);
        }
        swipeAmount++;
    }

    private void OnDislike(object sender, EventArgs e)
    {
        Database database = new Database();
        string emailCurrentUser = Authentication._currentUser.email;
        string emaildDislikedUser = feedUser.email;

        database.NewDislike(emailCurrentUser, emaildDislikedUser);

        //naar de volgende persoon
    }
}