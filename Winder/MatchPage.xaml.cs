using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input; 

using DataModel;
using Microsoft.Maui;

using Microsoft.Maui.Controls;
namespace MAUI;

public partial class MatchPage : ContentPage {
    private Authentication _authentication = new Authentication();
    private Database _database = new Database();
    private Queue<Profile> _profileQueue = new Queue<Profile>();
    private Profile _currentProfile;
    private Image[] _images = new Image[10];
    private int selectedImage = 0;
    
    public MatchPage() {

        //Get profiles to swipe
        CheckIfQueueNeedsMoreProfiles();

        Title = "Make your match now!";

        //Main stacklayout
        StackLayout verticalStackLayout = new StackLayout {Orientation = StackOrientation.Vertical,VerticalOptions = LayoutOptions.Fill};
        verticalStackLayout.Spacing = 10;
        
        //The stack with left the image and right the info.
        StackLayout imageLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
        imageLayout.Spacing = 10;

        //Button StackLayout
        StackLayout buttonStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal,HorizontalOptions = LayoutOptions.Center};
        buttonStackLayout.Spacing = 10;
        

        try {
            _currentProfile = _profileQueue.Dequeue();
        } catch (Exception e) {
            //No profiles found
            Console.WriteLine("Couldn't find a new profile");
            Console.WriteLine(e.StackTrace);
        }

        //Images
        if (_currentProfile == null) {
            
            if(Authentication._currentUser.profilePicture != null ) {

                MemoryStream ms = new MemoryStream(Authentication._currentUser.profilePicture);

                var profileImage = new Image {

                    Source = ImageSource.FromStream(() => ms),
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 800,
                    HeightRequest = 800,
                    BackgroundColor = Color.FromArgb("#ffffff")
                };
                verticalStackLayout.Add(profileImage);

            }

            var label = new Label { Text = "No more profiles to match with for now", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            verticalStackLayout.Add(label);

        }
        else
        {
            StackLayout infoStackLayout = new StackLayout { Orientation = StackOrientation.Vertical };
            if (_currentProfile == null)
            {
                _currentProfile = _profileQueue.Dequeue();
            }

            //Image carousel

            //placeholder als geen image
            var currentImage = new ImageButton();

            if (_currentProfile == null) {
                currentImage.Source = ImageSource.FromFile("noprofile.jpg");

            } else {
                MemoryStream ms = new MemoryStream(_currentProfile.profile_images[0]);
                currentImage.Source = ImageSource.FromStream(() => ms);

                //Data binding
                currentImage.SetBinding(ImageButton.SourceProperty, new Binding() {Source = ImageSource.FromStream(() => ms)});

            }
            currentImage.WidthRequest = 600;
            currentImage.HeightRequest = 600;

            currentImage.Clicked += (sender, args) => {
                if(_currentProfile != null) {
                    if (selectedImage < _currentProfile.profile_images.Length) {
                        selectedImage++;
                    } else {
                        selectedImage = 0;
                    }
                }
            };

            imageLayout.Add(currentImage);


            //Name
            StackLayout nameStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var namelbl = new Label { Text = "Naam: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start};

            //Binding
            var name = new Label {Text = _currentProfile.user.firstName, FontSize = 20, HorizontalOptions = LayoutOptions.Start};

            name.SetBinding(Label.TextProperty, new Binding() { Source = _currentProfile.user.firstName });
            
            name.FontSize = 20;
            name.HorizontalOptions = LayoutOptions.Start;

            //Add to stack
            nameStackLayout.Add(namelbl);
            nameStackLayout.Add(name);
            infoStackLayout.Add(nameStackLayout);
            

            //Gender
            StackLayout genderStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var genderlbl = new Label { Text = "Geslacht: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var gender = new Label {Text = _currentProfile.user.gender, FontSize = 20, HorizontalOptions = LayoutOptions.Start};
            
            //Data binding
            var genderBinding = new Binding() {Source = _currentProfile.user.gender};
            gender.SetBinding(Label.TextProperty,genderBinding);

            //Add to Stack

            genderStackLayout.Add(genderlbl);
            genderStackLayout.Add(gender);
            infoStackLayout.Add(genderStackLayout);
            
            
            //Age
            StackLayout ageStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var agelbl = new Label { Text = "Leeftijd: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
            var birthday = int.Parse(_currentProfile.user.birthDay.ToString("yyyyMMdd"));
            var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            var age = new Label { Text = ((today - birthday) /1000).ToString(), FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Add to Stack
            var ageBinding = new Binding() {Source = ((today - birthday) / 1000).ToString()};
            age.SetBinding(Label.TextProperty, ageBinding);
            
            
            //Add to Stack
            ageStackLayout.Add(agelbl);
            ageStackLayout.Add(age);
            infoStackLayout.Add(ageStackLayout);
            
            
            //Location
            StackLayout locationStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var locationlbl = new Label { Text = "Windesheim locatie: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
            var location = new Label { Text = _currentProfile.user.school , FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Data binding
            var locationBinding = new Binding() { Source = _currentProfile.user.school };
            gender.SetBinding(Label.TextProperty, locationBinding);

            //Add to Stack

            locationStackLayout.Add(locationlbl);
            locationStackLayout.Add(location);
            infoStackLayout.Add(locationStackLayout);
            

            //Education
            StackLayout educationStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var educationlbl = new Label { Text = "Opleiding: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
            var education = new Label { Text = _currentProfile.user.major , FontSize = 20, HorizontalOptions = LayoutOptions.Start };

            //Data binding
            var educationbinding = new Binding() { Source = _currentProfile.user.major };
            gender.SetBinding(Label.TextProperty, educationbinding);

            //Add to stack
            educationStackLayout.Add(educationlbl);
            educationStackLayout.Add(education);
            infoStackLayout.Add(educationStackLayout);

            //Data binding
            var bioBinding = new Binding() { Source = _currentProfile.user.bio };

            //Bio
            StackLayout bioStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var biolbl = new Label { Text = "Bio: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
            var bio = new Label {FontSize = 20, HorizontalOptions = LayoutOptions.Start };
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

    private async Task UpdateQueue()
    {
        Task gettingProfiles = GetProfiles();
        await gettingProfiles;
    }

    private async Task GetProfiles() {
        Profile[] profiles =  _database.Get5Profiles(Authentication._currentUser.email);
        foreach (var profile in profiles) {
            _profileQueue.Enqueue(profile);
        }
    }
    
    private async void CheckIfQueueNeedsMoreProfiles() {
        if(_profileQueue.Count < 5) {
            await GetProfiles();
        }
        if (_currentProfile != null)
        {
            _currentProfile = _profileQueue.Dequeue();
        }
    }

    private void NextProfile() {
        if(_profileQueue.Count != 0) {
            _currentProfile = _profileQueue.Dequeue();
        } else {
            _currentProfile = null;
        }
    }

    //Give match popup
    private async void MatchPopup() {
        await DisplayAlert("Match", "You have a match", "OK");
    }

    int swipes = 0;
    private void OnSwipe(object sender, SwipedEventArgs e)
    {
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
    
    private void OnLike(object sender, EventArgs e)
    {
        CheckIfQueueNeedsMoreProfiles();
        string emailCurrentUser = Authentication._currentUser.email;
        string emailLikedUser = _currentProfile.user.email;
        if (_database.CheckMatch(emailCurrentUser, emailLikedUser))
        {
            _database.NewMatch(emailLikedUser, emailCurrentUser);
            _database.deleteLikeOnMatch(emailCurrentUser, emailLikedUser);
        }
        else
        {
            _database.NewLike(emailCurrentUser, emailLikedUser);
        }

        MatchPopup()

        NextProfile();
    }

    private void OnDislike(object sender, EventArgs e)
    {
        CheckIfQueueNeedsMoreProfiles();
        string emailCurrentUser = Authentication._currentUser.email;
        string emaildDislikedUser = _currentProfile.user.email;

        _database.NewDislike(emailCurrentUser, emaildDislikedUser);

        NextProfile();
    }
}