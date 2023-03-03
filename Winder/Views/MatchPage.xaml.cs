using Controller;
using DataModel;

namespace Winder;

public partial class MatchPage {
    
    public string OriginPage;
    private const string PageName = "matchpage";
    private const string BackbuttonImage = "backbutton.png";
    public bool BackButtonVisible;
    
    private StackLayout verticalStackLayout;

    private int SelectedImage;
    private readonly int swipes = 0;

    private ProfileQueueController ProfileQueueController;
    
    private MatchModel MatchModel;


    public MatchPage() {
        //Gets the controller
        ProfileQueueController = new ProfileQueueController(Authentication.CurrentUser,Database.ReleaseConnection);

        //Set first profile
        ProfileQueueController.NextProfile(Database.ReleaseConnection);
        
        //Set the match model
        MatchModel = new MatchModel(Authentication.CurrentUser.GetMatchedStudentsFromUser(Database.ReleaseConnection));

        //Set content
        Initialize();
    }

    private void Initialize() {

        Title = "Make your match now!";
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });
        

        verticalStackLayout = new StackLayout {
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
            Source = BackbuttonImage,
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
        verticalStackLayout.Add(gridLayout);
        
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
        if (ProfileQueueController.CurrentProfile == null) {
            if (Authentication.CurrentUser.ProfilePicture.Length < 0) {

                var profileImage = new Image {
                    Source = ImageSource.FromStream(() => new MemoryStream(Authentication.CurrentUser.ProfilePicture)),
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 800,
                    HeightRequest = 800,
                    BackgroundColor = Color.FromArgb("#CC415F")
                };
                verticalStackLayout.Add(profileImage);

            } else {

                var profileImage = new Image {
                    Source = "noprofile.jpg",
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 800,
                    HeightRequest = 800,
                    BackgroundColor = Color.FromArgb("#CC415F")
                };
                verticalStackLayout.Add(profileImage);
            }

            var label = new Label { Text = "No more profiles to match with for now", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            verticalStackLayout.Add(label);

        } else {
            StackLayout infoStackLayout = new StackLayout { Orientation = StackOrientation.Vertical };


            //Image carousel
            var currentImage = new ImageButton();

            currentImage.WidthRequest = 600;
            currentImage.HeightRequest = 600;

            currentImage.Source = ImageSource.FromStream(() => new MemoryStream(ProfileQueueController.CurrentProfile.ProfileImages[SelectedImage]));

            currentImage.Clicked += (_, _) => {
                if (SelectedImage < ProfileQueueController.CurrentProfile.ProfileImages.Count(x => x != null) - 1) {
                    SelectedImage++;
                    Initialize();
                } else {
                    SelectedImage = 0;
                    Initialize();
                }
            };

            imageLayout.Add(currentImage);

            //Name
            StackLayout nameStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var namelbl = new Label { Text = "Naam: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var name = new Label { Text = ProfileQueueController.CurrentProfile.User.FirstName, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to stack
            nameStackLayout.Add(namelbl);
            nameStackLayout.Add(name);
            infoStackLayout.Add(nameStackLayout);


            //Gender
            StackLayout genderStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var genderlbl = new Label { Text = "Geslacht: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var gender = new Label { Text = ProfileQueueController.CurrentProfile.User.Gender, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            genderStackLayout.Add(genderlbl);
            genderStackLayout.Add(gender);
            infoStackLayout.Add(genderStackLayout);


            //Age
            StackLayout ageStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var agelbl = new Label { Text = "Leeftijd: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var birthday = new User().CalculateAge(ProfileQueueController.CurrentProfile.User.BirthDay);
            var age = new Label { Text = birthday.ToString(), FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            ageStackLayout.Add(agelbl);
            ageStackLayout.Add(age);
            infoStackLayout.Add(ageStackLayout);


            //Location
            StackLayout locationStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var locationlbl = new Label { Text = "Windesheim locatie: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var location = new Label { Text = ProfileQueueController.CurrentProfile.User.School, FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            
            //Add to Stack
            locationStackLayout.Add(locationlbl);
            locationStackLayout.Add(location);
            infoStackLayout.Add(locationStackLayout);


            //Education
            StackLayout educationStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var educationlbl = new Label { Text = "Opleiding: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var education = new Label { Text = ProfileQueueController.CurrentProfile.User.Major, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to stack
            educationStackLayout.Add(educationlbl);
            educationStackLayout.Add(education);
            infoStackLayout.Add(educationStackLayout);

            //Bio
            StackLayout bioStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var biolbl = new Label { Text = "Bio: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var bio = new Label { Text = ProfileQueueController.CurrentProfile.User.Bio ,FontSize = 20, HorizontalOptions = LayoutOptions.Start };


            //Add to stack
            bioStackLayout.Add(biolbl);
            bioStackLayout.Add(bio);
            infoStackLayout.Add(bioStackLayout);
            
            //Interests
            StackLayout InterestsStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var interestslbl = new Label { Text = "Interesses: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            InterestsStackLayout.Add(interestslbl);

            for (int i = 0; i < ProfileQueueController.CurrentProfile.User.Interests.Length; i++) {
                if (i != 0) {
                    var spacecommavar = new Label { FontSize = 20, HorizontalOptions = LayoutOptions.Start, Text = ", " };
                    InterestsStackLayout.Add(spacecommavar);
                }
                
                var interestvar = new Label { Text = ProfileQueueController.CurrentProfile.User.Interests[i], FontSize = 20, HorizontalOptions = LayoutOptions.Start };
                InterestsStackLayout.Add(interestvar);

            }

            infoStackLayout.Add(InterestsStackLayout);

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
            verticalStackLayout.GestureRecognizers.Add(rightSwipe);
            verticalStackLayout.GestureRecognizers.Add(leftSwipe);
        }

        //Add the different stacklayouts
        verticalStackLayout.Add(imageLayout);
        verticalStackLayout.Add(buttonStackLayout);

        verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        Content = verticalStackLayout;

        
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
                if (swipes % 2 == 0) {
                    OnLike(sender, e);
                }
                break;
            case SwipeDirection.Left:
                if (swipes % 2 == 0) {
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
        if (MatchModel.CheckMatch(Authentication.CurrentUser.Email, ProfileQueueController.CurrentProfile.User.Email, Database.ReleaseConnection)) {
            MatchPopup();
        }
        ProfileQueueController.OnLike(Database.ReleaseConnection);
        SelectedImage = 0;
        Initialize();
    }

    /// <summary>
    /// The dislike button is clicked
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void OnDislike(object sender, EventArgs e) {
        ProfileQueueController.OnDislike(Database.ReleaseConnection);
        SelectedImage = 0;
        Initialize();
    }
}