using System.Drawing;
using DataModel;
using Winder;

namespace MAUI;

public partial class MatchPage : ContentPage {

    
    private Database _database = new Database();

    public string originPage;
    public const string pageName = "matchpage";
    public const string backbuttonImage = "backbutton.png";
    public bool backButtonVisible = false;

    public MatchPage() {

        Title = "Make your match now!";
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });
        
        StackLayout verticalStackLayout = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill };
        verticalStackLayout.Spacing = 10;
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
        var backButton = new ImageButton();
        backButton.Source = backbuttonImage;
        backButton.WidthRequest = 40;
        backButton.HeightRequest = 40;
        backButton.HorizontalOptions = LayoutOptions.Start;
        backButton.CornerRadius = 50;
        backButton.Aspect = Aspect.AspectFill;
        backButton.IsVisible = false;

        if (backButtonVisible) {

            backButton.IsVisible = true;
        }
        backButton.Clicked += BackButton_Clicked;
        gridLayout.Add(backButton,0);
        
        

        //Chat button
        var ChatButton = new Button();
        ChatButton.Text = "Chats";
        ChatButton.WidthRequest = 100;
        ChatButton.HeightRequest = 50;
        ChatButton.IsVisible = true;
        ChatButton.TextColor = Microsoft.Maui.Graphics.Color.FromRgb(0, 0, 0);
        ChatButton.Clicked += ChatButton_Clicked;
        ChatButton.HorizontalOptions = LayoutOptions.End;


       

        //my profile button
        var myProfile = new Button();
        myProfile.Text = "Mijn profiel";
        myProfile.HeightRequest = 50;
        myProfile.TextColor = Microsoft.Maui.Graphics.Color.FromRgb(0, 0, 0);
        myProfile.WidthRequest = 110;
        myProfile.Clicked += MyProfile_Clicked;
        myProfile.HorizontalOptions = LayoutOptions.End;

        //settings button
        var instellingen = new Button();
        instellingen.Text = "Instellingen";
        instellingen.TextColor = Microsoft.Maui.Graphics.Color.FromRgb(0, 0, 0);
        instellingen.HeightRequest = 50;
        instellingen.WidthRequest = 115;
        instellingen.HorizontalOptions = LayoutOptions.End;
        instellingen.Clicked += Settings_Clicked;

        horizontalLayout.Children.Add(ChatButton);

        horizontalLayout.Children.Add(myProfile);
        horizontalLayout.Children.Add(instellingen);
        gridLayout.Add(horizontalLayout,2);
        verticalStackLayout.Add(gridLayout);
        
        //The stack with left the image and right the info.
        StackLayout imageLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
        imageLayout.Spacing = 10;

        //Button StackLayout
        StackLayout buttonStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center };
        buttonStackLayout.Spacing = 10;

        //Images
        if (Authentication._currentProfile == null)
        {

            if (Authentication._currentUser.profilePicture != null)
            {
                if (Authentication._currentUser.profilePicture.Length > 1000)
                {

                    MemoryStream ms = new MemoryStream(Authentication._currentUser.profilePicture);


                    var profileImage = new Microsoft.Maui.Controls.Image
                    {
                        Source = ImageSource.FromStream(() => ms),
                        Aspect = Aspect.AspectFit,
                        WidthRequest = 800,
                        HeightRequest = 800,
                        BackgroundColor = Microsoft.Maui.Graphics.Color.FromArgb("#CC415F")
                    };
                    verticalStackLayout.Add(profileImage);

                } else {

                    var profileImage = new Microsoft.Maui.Controls.Image
                    {
                        Source = "noprofile.jpg",
                        Aspect = Aspect.AspectFit,
                        WidthRequest = 800,
                        HeightRequest = 800,
                        BackgroundColor = Microsoft.Maui.Graphics.Color.FromArgb("#CC415F")
                    };
                    verticalStackLayout.Add(profileImage);
                }

            }

            var label = new Label { Text = "No more profiles to match with for now", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            verticalStackLayout.Add(label);

        }
        else
        {
            StackLayout infoStackLayout = new StackLayout { Orientation = StackOrientation.Vertical };


            //Image carousel
            var currentImage = new ImageButton();


            if (Authentication._currentProfile == null)
            {
                currentImage.Source = ImageSource.FromFile("noprofile.jpg");

            }
            else
            {
                MemoryStream ms = new MemoryStream(ScaleImage(Authentication._currentProfile.profile_images[Authentication.selectedImage]));

                currentImage.Source = ImageSource.FromStream(() => ms);

                //Data binding
                currentImage.SetBinding(ImageButton.SourceProperty, new Binding() { Source = ImageSource.FromStream(() => ms) });

            }
            currentImage.WidthRequest = 600;
            currentImage.HeightRequest = 600;

            currentImage.Clicked += (sender, args) => {

                if (Authentication._currentProfile != null)
                {
                    if (Authentication.selectedImage < Authentication._currentProfile.profile_images.Length)
                    {
                        Authentication.selectedImage++;
                    }
                    else
                    {

                        Authentication.selectedImage = 0;
                    }
                }
            };

            imageLayout.Add(currentImage);


            //Name
            StackLayout nameStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var namelbl = new Label { Text = "Naam: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Binding
            var name = new Label { Text = Authentication._currentProfile.user.firstName, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            name.SetBinding(Label.TextProperty, new Binding() { Source = Authentication._currentProfile.user.firstName });

            name.FontSize = 20;
            name.HorizontalOptions = LayoutOptions.Start;

            //Add to stack
            nameStackLayout.Add(namelbl);
            nameStackLayout.Add(name);
            infoStackLayout.Add(nameStackLayout);


            //Gender
            StackLayout genderStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var genderlbl = new Label { Text = "Geslacht: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var gender = new Label { Text = Authentication._currentProfile.user.gender, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Data binding
            var genderBinding = new Binding() { Source = Authentication._currentProfile.user.gender };
            gender.SetBinding(Label.TextProperty, genderBinding);

            //Add to Stack
            genderStackLayout.Add(genderlbl);
            genderStackLayout.Add(gender);
            infoStackLayout.Add(genderStackLayout);


            //Age
            StackLayout ageStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var agelbl = new Label { Text = "Leeftijd: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var birthday = int.Parse(Authentication._currentProfile.user.birthDay.ToString("yyyyMMdd"));
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            var age = new Label { Text = ((today - birthday) / 1000).ToString(), FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            var ageBinding = new Binding() { Source = ((today - birthday) / 1000).ToString() };
            age.SetBinding(Label.TextProperty, ageBinding);


            //Add to Stack
            ageStackLayout.Add(agelbl);
            ageStackLayout.Add(age);
            infoStackLayout.Add(ageStackLayout);


            //Location
            StackLayout locationStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var locationlbl = new Label { Text = "Windesheim locatie: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var location = new Label { Text = Authentication._currentProfile.user.school, FontSize = 20, HorizontalOptions = LayoutOptions.Start };


            //Data binding
            var locationBinding = new Binding() { Source = Authentication._currentProfile.user.school };
            location.SetBinding(Label.TextProperty, locationBinding);

            //Add to Stack
            locationStackLayout.Add(locationlbl);
            locationStackLayout.Add(location);
            infoStackLayout.Add(locationStackLayout);


            //Education
            StackLayout educationStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var educationlbl = new Label { Text = "Opleiding: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var education = new Label { Text = Authentication._currentProfile.user.major, FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Data binding
            var educationbinding = new Binding() { Source = Authentication._currentProfile.user.major };
            education.SetBinding(Label.TextProperty, educationbinding);

            //Add to stack
            educationStackLayout.Add(educationlbl);
            educationStackLayout.Add(education);
            infoStackLayout.Add(educationStackLayout);


            //Data binding
            var bioBinding = new Binding() { Source = Authentication._currentProfile.user.bio };

            //Bio
            StackLayout bioStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var biolbl = new Label { Text = "Bio: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var bio = new Label { FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            bio.SetBinding(Label.TextProperty, bioBinding);


            //Add to stack
            bioStackLayout.Add(biolbl);
            bioStackLayout.Add(bio);
            infoStackLayout.Add(bioStackLayout);


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

        verticalStackLayout.BackgroundColor = Microsoft.Maui.Graphics.Color.FromArgb("#CC415F");
        Content = verticalStackLayout;
    }
    
    

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        switch (originPage)
        {
            case "profilepage":
                Navigation.PushAsync(new ProfileChange());
                break;
            case "settingspage":
                Navigation.PushAsync(new Instellingen());
                break;
        }

    }

    // myprofile button clicked
    private void MyProfile_Clicked(object sender, EventArgs e)
    {
        //declares origin page, in the my profile page
        ProfileChange myProfile = new ProfileChange();
        backButtonVisible = true;
        myProfile.originPage = pageName;
        Navigation.PushAsync(myProfile);
    }
    // matches button clicked
    private void ChatButton_Clicked(object obj, EventArgs e)
    {
        
        ChatPage chats = new ChatPage();
        backButtonVisible = true;
        //declares origin page, in the matches page
        chats.originPage = pageName;
        Navigation.PushAsync(chats);
    }

    // matches button clicked
    private void MatchesButton_Clicked(object obj, EventArgs e)
    {

        MatchesPage chats = new MatchesPage();
        backButtonVisible = true;
        //declares origin page, in the matches page
        chats.originPage = pageName;
        Navigation.PushAsync(chats);
    }

    private void Settings_Clicked(object sender, EventArgs e)
    {
        Instellingen Instellingen = new Instellingen();
        backButtonVisible = true;
        Instellingen.originPage = pageName;
        Navigation.PushAsync(Instellingen);
    }
    
    

    public void NextProfile() {

        Authentication.CheckIfQueueNeedsMoreProfiles();
        if (Authentication._profileQueue.Count != 0) {
            try {
                Authentication._currentProfile = Authentication._profileQueue.Dequeue();

            } catch (Exception e) {
                //No profiles found
                Console.WriteLine("Couldn't find a new profile");
                Console.WriteLine(e.ToString());
                Console.WriteLine(e.StackTrace);

            }

            Authentication.selectedImage = 0;
            Navigation.PushAsync(new MatchPage());
            
            //Works better but doesn't work
            //Application.Current.Dispatcher.Dispatch(() => Authentication._currentProfile = Authentication._profileQueue.Dequeue());

        } else {
            Console.WriteLine("Couldn't find a new profile");
            Authentication._currentProfile = null;
            Navigation.PushAsync(new MatchPage());
            
        }

    }

    //Give match popup
    private async void MatchPopup()
    {
        await DisplayAlert("Match", "You have a match", "OK");
    }

    

    byte[] ScaleImage(byte[] bytes) {
        //Dont run when other then Windows
#if WINDOWS

        if (Authentication.isscaled == false) {

            Authentication.isscaled = true;

            using var memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, Convert.ToInt32(bytes.Length));
            memoryStream.Seek(0, SeekOrigin.Begin);

            using var originalImage = new Bitmap(memoryStream);
            var resized = new Bitmap(600, 600);
            using var graphics = System.Drawing.Graphics.FromImage(resized);
            graphics.DrawImage(originalImage, 0, 0, 600, 600);
            using var stream = new MemoryStream();
            resized.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }

        return bytes;
#else
        return bytes;
#endif


    }
    
    int swipes = 0;
    private void OnSwipe(object sender, SwipedEventArgs e) {
        switch (e.Direction)
        {
            case SwipeDirection.Right:
                if (swipes % 2 == 0)
                {
                    OnLike(sender, e);
                }
                break;
            case SwipeDirection.Left:
                if (swipes % 2 == 0)
                {
                    OnDislike(sender, e);
                }
                break;
        }
    }
    
    private void OnLike(object sender, EventArgs e) {
        string emailCurrentUser = Authentication._currentUser.email;
        string emailLikedUser = Authentication._currentProfile.user.email;
        if (_database.CheckMatch(emailCurrentUser, emailLikedUser)) {
            _database.NewMatch(emailLikedUser, emailCurrentUser);
            _database.DeleteLikeOnMatch(emailCurrentUser, emailLikedUser);
            MatchPopup();
        } else {
            _database.NewLike(emailCurrentUser, emailLikedUser);
        }

        NextProfile();
    }

    private void OnDislike(object sender, EventArgs e)
    {
        string emailCurrentUser = Authentication._currentUser.email;
        string emaildDislikedUser = Authentication._currentProfile.user.email;

        _database.NewDislike(emailCurrentUser, emaildDislikedUser);

        NextProfile();
    }

}