using Controller;
using DataModel;

namespace Winder;

public partial class MatchPage {
    
    public string OriginPage;
    private const string PageName = "matchpage";
    private const string BackButtonImage = "backbutton.png";
    public bool BackButtonVisible;
    
    private StackLayout _verticalStackLayout;

    private int _selectedImage;
    private readonly int _swipes = 0;

    private readonly MatchmakingController _matchmakingController;

    private readonly ValidationController _validationController;


    public MatchPage() {
        

        //Creating controller
        _matchmakingController = MauiProgram.ServiceProvider.GetService<MatchmakingController>();
        _validationController = MauiProgram.ServiceProvider.GetService<ValidationController>();
        
        //Set first profile
        _matchmakingController.NextProfile();

        //Set content
        Initialize();
    }

    private void Initialize() {

        Title = "Make your match now!";
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });
        

        _verticalStackLayout = new StackLayout {
            Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill,
            Spacing = 10
        };
        Grid gridLayout = new Grid() {
            ColumnDefinitions = {
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition()
            }
        };
        HorizontalStackLayout horizontalLayout = new HorizontalStackLayout() {
           HorizontalOptions = LayoutOptions.End
        };
        
        // backbutton
        var backButton = new ImageButton {
            Source = BackButtonImage,
            WidthRequest = 40,
            HeightRequest = 40,
            HorizontalOptions = LayoutOptions.Start,
            CornerRadius = 50,
            Aspect = Aspect.AspectFill,
            IsVisible = BackButtonVisible
        };
        backButton.Clicked += BackButton_Clicked;
        gridLayout.Add(backButton,0);
        
        
        //Chat button
        var chatButton = new Button {
            Text = "Chats",
            WidthRequest = 100,
            HeightRequest = 50,
            IsVisible = true,
            TextColor = Color.FromRgb(0, 0, 0)
        };
        chatButton.Clicked += ChatButton_Clicked;
        chatButton.HorizontalOptions = LayoutOptions.End;

        //my profile button
        var myProfile = new Button {
            Text = "Mijn profiel",
            HeightRequest = 50,
            TextColor = Color.FromRgb(0, 0, 0),
            WidthRequest = 110
        };
        myProfile.Clicked += MyProfile_Clicked;
        myProfile.HorizontalOptions = LayoutOptions.End;

        //settings button
        var settings = new Button {
            Text = "Instellingen",
            TextColor = Color.FromRgb(0, 0, 0),
            HeightRequest = 50,
            WidthRequest = 115,
            HorizontalOptions = LayoutOptions.End
        };
        settings.Clicked += Settings_Clicked;


        horizontalLayout.Children.Add(chatButton);
        horizontalLayout.Children.Add(myProfile);
        horizontalLayout.Children.Add(settings);
        gridLayout.Add(horizontalLayout,2);
        _verticalStackLayout.Add(gridLayout);
        
        //The stack with left the image and right the info.
        StackLayout imageLayout = new StackLayout {
            Orientation = StackOrientation.Horizontal,
            Spacing = 10
        };

        //Button StackLayout
        StackLayout buttonStackLayout = new StackLayout {
            Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center,
            Spacing = 10
        };

        //Images
        if (_matchmakingController.CurrentProfile == null) {
            if (User.CurrentUser.ProfilePicture.Length < 0) {

                var profileImage = new Image {
                    Source = ImageSource.FromStream(() => new MemoryStream(User.CurrentUser.ProfilePicture)),
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 800,
                    HeightRequest = 800,
                    BackgroundColor = Color.FromArgb("#CC415F")
                };
                _verticalStackLayout.Add(profileImage);

            } else {

                var profileImage = new Image {
                    Source = "noprofile.jpg",
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 800,
                    HeightRequest = 800,
                    BackgroundColor = Color.FromArgb("#CC415F")
                };
                _verticalStackLayout.Add(profileImage);
            }

            var label = new Label { Text = "No more profiles to match with for now", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            _verticalStackLayout.Add(label);

        } else {
            StackLayout infoStackLayout = new StackLayout { Orientation = StackOrientation.Vertical };


            //Image carousel
            var currentImage = new ImageButton
            {
                WidthRequest = 600,
                HeightRequest = 600,
                Source = ImageSource.FromStream(() => new MemoryStream(_matchmakingController.CurrentProfile.ProfileImages[_selectedImage]))
            };

            currentImage.Clicked += (_, _) => {
                if (_selectedImage < _matchmakingController.CurrentProfile.ProfileImages.Count(x => x != null) - 1) {
                    _selectedImage++;
                    Initialize();
                } else {
                    _selectedImage = 0;
                    Initialize();
                }
            };

            imageLayout.Add(currentImage);

            //Name
            StackLayout nameStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var namelbl = new Label { Text = "Naam: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var name = new Label { Text = _matchmakingController.CurrentProfile.User.FirstName, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to stack
            nameStackLayout.Add(namelbl);
            nameStackLayout.Add(name);
            infoStackLayout.Add(nameStackLayout);


            //Gender
            StackLayout genderStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var genderlbl = new Label { Text = "Geslacht: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var gender = new Label { Text = _matchmakingController.CurrentProfile.User.Gender, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            genderStackLayout.Add(genderlbl);
            genderStackLayout.Add(gender);
            infoStackLayout.Add(genderStackLayout);


            //Age
            StackLayout ageStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var agelbl = new Label { Text = "Leeftijd: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            int birthday = _validationController.CalculateAge(_matchmakingController.CurrentProfile.User.BirthDay);
            var age = new Label { Text = birthday.ToString(), FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            ageStackLayout.Add(agelbl);
            ageStackLayout.Add(age);
            infoStackLayout.Add(ageStackLayout);


            //Location
            StackLayout locationStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var locationlbl = new Label { Text = "Windesheim locatie: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var location = new Label { Text = _matchmakingController.CurrentProfile.User.School, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            locationStackLayout.Add(locationlbl);
            locationStackLayout.Add(location);
            infoStackLayout.Add(locationStackLayout);


            //Education
            StackLayout educationStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var educationlbl = new Label { Text = "Opleiding: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var education = new Label { Text = _matchmakingController.CurrentProfile.User.Major, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to stack
            educationStackLayout.Add(educationlbl);
            educationStackLayout.Add(education);
            infoStackLayout.Add(educationStackLayout);

            //Bio
            StackLayout bioStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var biolbl = new Label { Text = "Bio: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var bio = new Label { Text = _matchmakingController.CurrentProfile.User.Bio ,FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to stack
            bioStackLayout.Add(biolbl);
            bioStackLayout.Add(bio);
            infoStackLayout.Add(bioStackLayout);
            
            //Interests
            StackLayout interestsStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var interestslbl = new Label { Text = "Interesses: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            interestsStackLayout.Add(interestslbl);


            for (int i = 0; i < _matchmakingController.CurrentProfile.User.Interests.Length; i++) {
                if (i != 0) {
                    var spacecommavar = new Label { FontSize = 20, HorizontalOptions = LayoutOptions.Start, Text = ", " };
                    interestsStackLayout.Add(spacecommavar);
                }
                
                var interestvar = new Label { Text = _matchmakingController.CurrentProfile.User.Interests[i], FontSize = 20, HorizontalOptions = LayoutOptions.Start };
                interestsStackLayout.Add(interestvar);

            }

            infoStackLayout.Add(interestsStackLayout);

            //Buttons
            var likeButton = new Button { Text = "Like", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            var dislikeButton = new Button { Text = "Dislike", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            var rightSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            var leftSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };

            rightSwipe.Swiped += OnSwipe;
            leftSwipe.Swiped += OnSwipe;
            likeButton.Clicked += OnLike;
            dislikeButton.Clicked += OnDislike;

            //Add info to ImageLayout
            imageLayout.Add(infoStackLayout);

            buttonStackLayout.Add(dislikeButton);
            buttonStackLayout.Add(likeButton);
            _verticalStackLayout.GestureRecognizers.Add(rightSwipe);
            _verticalStackLayout.GestureRecognizers.Add(leftSwipe);
        }

        //Add the different stacklayouts
        _verticalStackLayout.Add(imageLayout);
        _verticalStackLayout.Add(buttonStackLayout);

        _verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        Content = _verticalStackLayout;

        
    }
    
    private void BackButton_Clicked(object sender, EventArgs e) {
        switch (OriginPage) {
            case "profilepage":
                Navigation.PushAsync(new ProfileChange());
                break;
            case "settingspage":
                Navigation.PushAsync(new SettingsPage());
                break;
        }

    }

    /// <summary>
    /// The back button clicked in the header
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void MyProfile_Clicked(object sender, EventArgs e) {
        //declares origin page, in the my profile page
        ProfileChange myProfile = new ProfileChange();
        BackButtonVisible = true;
        myProfile.OriginPage = PageName;
        Navigation.PushAsync(myProfile);
    }
    
    /// <summary>
    /// Chat button clicked in the header
    /// </summary>
    /// <param name="obj">The sender</param>
    /// <param name="e">The event args</param>
    private void ChatButton_Clicked(object obj, EventArgs e) {
        
        ChatsViewPage chatsViews = new ChatsViewPage();
        BackButtonVisible = true;
        //declares origin page, in the matches page
        chatsViews.OriginPage = PageName;
        Navigation.PushAsync(chatsViews);
    }

    /// <summary>
    /// Settings button clicked in the header
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void Settings_Clicked(object sender, EventArgs e)
    {
        SettingsPage settingsPage = new SettingsPage();
        BackButtonVisible = true;
        settingsPage.OriginPage = PageName;
        Navigation.PushAsync(settingsPage);
    }
    
    
    /// <summary>
    /// The popup that there is a match
    /// </summary>
    private async void MatchPopup()
    {
        await DisplayAlert("Match", "You have a match", "OK");
    }
    
    /// <summary>
    /// The swipe gesture recognizer is triggered
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The objecy</param>
    private void OnSwipe(object sender, SwipedEventArgs e) {
        switch (e.Direction) {
            case SwipeDirection.Right:
                if (_swipes % 2 == 0) {
                    OnLike(sender, e);
                }
                break;
            case SwipeDirection.Left:
                if (_swipes % 2 == 0) {
                    OnDislike(sender, e);
                }
                break;
        }
    }

    /// <summary>
    /// The like button is clicked
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void OnLike(object sender, EventArgs e) {
        if (_matchmakingController.CheckMatch(User.CurrentUser.Email,_matchmakingController.CurrentProfile.User.Email)) {
            MatchPopup();
        }
        _matchmakingController.OnLike();
        _selectedImage = 0;
        Initialize();
    }

    /// <summary>
    /// The dislike button is clicked
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void OnDislike(object sender, EventArgs e) {
        _matchmakingController.OnDislike();
        _selectedImage = 0;
        Initialize();
    }
}