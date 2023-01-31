using DataModel;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;
using Image = Microsoft.Maui.Controls.Image;

namespace Winder;

public partial class MatchPage
{

    private SwipeController SwipeController;
    public string OriginPage;
    private const string PageName = "matchpage";
    private const string BackbuttonImage = "backbutton.png";
    public bool BackButtonVisible;
    
    private StackLayout verticalStackLayout;

    private int SelectedImage = 0;
    private readonly int swipes = 0;

    private ProfileQueueController ProfileQueueController;



    public MatchPage() {
        //Gets the controller
        ProfileQueueController = new ProfileQueueController(Authentication.CurrentUser,Database.ReleaseConnection);
        SwipeController = new SwipeController();
        //Set first profile
        if (ProfileQueueController.ProfileQueue.GetCount() > 0) {
            try {
                ProfileQueueController.ProfileQueue.GetNextProfile();

            } catch (Exception e) {
                
                //No profiles found
                Console.WriteLine("Error dequeuing profile: " + e);
                Console.WriteLine(e.StackTrace);
            }
        } else {
            ProfileQueueController.ProfileQueue.Clear();
        }
        
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
            if (Authentication.CurrentUser.ProfilePicture.IsEmpty) {

                var profileImage = new Image {
                    Source = Authentication.CurrentUser.ProfilePicture,
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

        }
        else {
            StackLayout infoStackLayout = new StackLayout { Orientation = StackOrientation.Vertical };


            //Image carousel
            var currentImage = new ImageButton();

            currentImage.WidthRequest = 600;
            currentImage.HeightRequest = 600;

            currentImage.Clicked += (_, _) => {
                if (SelectedImage < ProfileQueueController.CurrentProfile.profileImages.Count(x => x != null) - 1) {
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
            var name = new Label { Text = ProfileQueueController.CurrentProfile.user.FirstName, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to stack
            nameStackLayout.Add(namelbl);
            nameStackLayout.Add(name);
            infoStackLayout.Add(nameStackLayout);


            //Gender
            StackLayout genderStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var genderlbl = new Label { Text = "Geslacht: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var gender = new Label { Text = ProfileQueueController.CurrentProfile.user.Gender, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            genderStackLayout.Add(genderlbl);
            genderStackLayout.Add(gender);
            infoStackLayout.Add(genderStackLayout);


            //Age
            StackLayout ageStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var agelbl = new Label { Text = "Leeftijd: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var birthday = new UserController().CalculateAge(ProfileQueueController.CurrentProfile.user.BirthDay);
            var age = new Label { Text = birthday.ToString(), FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            ageStackLayout.Add(agelbl);
            ageStackLayout.Add(age);
            infoStackLayout.Add(ageStackLayout);


            //Location
            StackLayout locationStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var locationlbl = new Label { Text = "Windesheim locatie: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var location = new Label { Text = ProfileQueueController.CurrentProfile.user.School, FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            
            //Add to Stack
            locationStackLayout.Add(locationlbl);
            locationStackLayout.Add(location);
            infoStackLayout.Add(locationStackLayout);


            //Education
            StackLayout educationStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            
            var educationlbl = new Label { Text = "Opleiding: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var education = new Label { Text = ProfileQueueController.CurrentProfile.user.Major, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to stack
            educationStackLayout.Add(educationlbl);
            educationStackLayout.Add(education);
            infoStackLayout.Add(educationStackLayout);

            //Bio
            StackLayout bioStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var biolbl = new Label { Text = "Bio: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var bio = new Label { Text = ProfileQueueController.CurrentProfile.user.Bio ,FontSize = 20, HorizontalOptions = LayoutOptions.Start };


            //Add to stack
            bioStackLayout.Add(biolbl);
            bioStackLayout.Add(bio);
            infoStackLayout.Add(bioStackLayout);
            
            //Interests
            StackLayout InterestsStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var interestslbl = new Label { Text = "Interesses: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            InterestsStackLayout.Add(interestslbl);

            for (int i = 0; i < ProfileQueueController.CurrentProfile.user.Interests.Length; i++) {
                if (i != 0) {
                    var spacecommavar = new Label { FontSize = 20, HorizontalOptions = LayoutOptions.Start, Text = ", " };
                    InterestsStackLayout.Add(spacecommavar);
                }
                
                var interestvar = new Label { Text = ProfileQueueController.CurrentProfile.user.Interests[i], FontSize = 20, HorizontalOptions = LayoutOptions.Start };
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
        switch (OriginPage)
        {
            case "profilepage":
                Navigation.PushAsync(new ProfileChange());
                break;
            case "settingspage":
                Navigation.PushAsync(new SettingsPage());
                break;
        }

    }

    // my profile button clicked
    private void MyProfile_Clicked(object sender, EventArgs e)
    {
        //declares origin page, in the my profile page
        ProfileChange myProfile = new ProfileChange();
        BackButtonVisible = true;
        myProfile.OriginPage = PageName;
        Navigation.PushAsync(myProfile);
    }
    
    // matches button clicked
    private void ChatButton_Clicked(object obj, EventArgs e) {
        
        ChatsViewPage chatsViews = new ChatsViewPage();
        BackButtonVisible = true;
        //declares origin page, in the matches page
        chatsViews.OriginPage = PageName;
        Navigation.PushAsync(chatsViews);
    }

    private void Settings_Clicked(object sender, EventArgs e)
    {
        SettingsPage settingsPage = new SettingsPage();
        BackButtonVisible = true;
        settingsPage.OriginPage = PageName;
        Navigation.PushAsync(settingsPage);
    }
    
    private void NextProfile() {

        ProfileQueueController.CheckIfQueueNeedsMoreProfiles(Database.ReleaseConnection);
        if (ProfileQueueController.ProfileQueue.GetCount() != 0) {
            ProfileQueueController.ProfileQueue.GetNextProfile();

            SelectedImage = 0;
            Initialize();

        } else {
            ProfileQueueController.ProfileQueue.Clear();
            Initialize();
        }

        if (ProfileQueueController.ProfileQueue.GetCount() <= 0)
        {
            Navigation.PushAsync(new MatchPage());
        }
    }

    //Give match popup
    private async void MatchPopup()
    {
        await DisplayAlert("Match", "You have a match", "OK");
    }
    
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

    private void OnLike(object sender, EventArgs e)
    {
        string emailCurrentUser = Authentication.CurrentUser.Email;
        string emailLikedUser = ProfileQueueController.CurrentProfile.user.Email;

        if(SwipeController.CheckMatch(emailCurrentUser, emailLikedUser, Database.ReleaseConnection)) {
            SwipeController.NewMatch(emailCurrentUser, emailLikedUser, Database.ReleaseConnection);
            SwipeController.DeleteLike(emailCurrentUser, emailLikedUser, Database.ReleaseConnection);
            MatchPopup();
        } else {
            SwipeController.NewLike(emailCurrentUser, emailLikedUser, Database.ReleaseConnection);
        }
        ProfileQueueController.CheckIfQueueNeedsMoreProfiles(Database.ReleaseConnection);
        NextProfile();
    }

    private void OnDislike(object sender, EventArgs e) {
        string emailCurrentUser = Authentication.CurrentUser.Email;
        string emaildDislikedUser = ProfileQueueController.CurrentProfile.user.Email;

        SwipeController.NewDislike(emailCurrentUser, emaildDislikedUser, Database.ReleaseConnection);
        ProfileQueueController.CheckIfQueueNeedsMoreProfiles(Database.ReleaseConnection);
        NextProfile();
    }
}