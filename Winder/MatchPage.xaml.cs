using DataModel;

namespace MAUI;

public partial class MatchPage : ContentPage {
    private Database _database = new Database();

    private  Queue<Profile> _profileQueue = new Queue<Profile>();
    private Profile _currentProfile;
    private Image[] _images = new Image[10];
    private int selectedImage = 0;
    
    public MatchPage() {
        Title = "Make your match now!";

        StackLayout verticalStackLayout = new StackLayout {
            Orientation = StackOrientation.Vertical,
            VerticalOptions = LayoutOptions.Fill
        };
        StackLayout imageLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
        
        verticalStackLayout.Spacing = 10;
        //Images
        if (_profileQueue.Count == 0) {
            if(Authentication._currentUser.profilePicture != null ) {

                var profileImage = new Image
                {
                    Source = ImageSource.FromResource(Authentication._currentUser.profilePicture.ToString()),
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 800,
                    HeightRequest = 800
                };
                verticalStackLayout.Add(profileImage);
                
            } else {
                var profileImage = new Image {
                    Source = "noprofile.jpg",
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 200,
                    HeightRequest = 200,
                    HorizontalOptions = LayoutOptions.Center
                };
                verticalStackLayout.Add(profileImage);
                var noProfileImage = new Label {
                    Text = "You have no profilepicture yet, add one to get more matches",
                    HorizontalOptions = LayoutOptions.Center
                };
                verticalStackLayout.Add(noProfileImage);
                
            }

            
            var label = new Label { Text = "No more profiles to match with for now", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            verticalStackLayout.Add(label);
            
        } else {
            StackLayout infoStackLayout = new StackLayout{Orientation = StackOrientation.Vertical};
            if(_currentProfile == null) {
                _currentProfile = _profileQueue.Dequeue();
            }
            
            //Image carousel
            var currentImage = new Button
            {
                ImageSource = ImageSource.FromResource(_currentProfile.profile_images[selectedImage].ToString()),
                WidthRequest = 800,
                HeightRequest = 800,

            };
            currentImage.Clicked += (sender, args) => {
                if(selectedImage < _currentProfile.profile_images.Length) {
                    selectedImage++;
                } else {
                    selectedImage = 0;
                }
            };
            
            
            //fullname
            StackLayout nameStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var fullnamelbl = new Label { Text = "Naam: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
            var fullname = new Label { Text = _currentProfile.user.firstName + " " + _currentProfile.user.middleName + " "+ _currentProfile.user.lastName, FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            nameStackLayout.Add(fullnamelbl);
            nameStackLayout.Add(fullname);
            infoStackLayout.Add(nameStackLayout);
            
            //Gender
            StackLayout genderStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var genderlbl = new Label { Text = "Geslacht: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            var gender = new Label {Text = _currentProfile.user.gender, FontSize = 20, HorizontalOptions = LayoutOptions.Start};
            
            genderStackLayout.Add(genderlbl);
            genderStackLayout.Add(gender);
            infoStackLayout.Add(genderStackLayout);
            
            //Age
            if (_currentProfile.user.birthDay != null) {
                StackLayout ageStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
                var agelbl = new Label { Text = "Leeftijd: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
                
                var birthday = int.Parse(_currentProfile.user.birthDay.ToString("yyyyMMdd"));
                var today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                var age = new Label { Text = ((today - birthday) /1000).ToString(), FontSize = 20, HorizontalOptions = LayoutOptions.Start };
                ageStackLayout.Add(agelbl);
                ageStackLayout.Add(age);
                infoStackLayout.Add(ageStackLayout);
            }
            
            //Location
            StackLayout locationStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var locationlbl = new Label { Text = "Windesheim locatie: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
            var location = new Label { Text = _currentProfile.user.location , FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            locationStackLayout.Add(locationlbl);
            locationStackLayout.Add(location);
            infoStackLayout.Add(locationStackLayout);
            
            //Education
            StackLayout educationStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var educationlbl = new Label { Text = "Opleiding: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
            var education = new Label { Text = _currentProfile.user.major , FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            educationStackLayout.Add(educationlbl);
            educationStackLayout.Add(education);
            infoStackLayout.Add(educationStackLayout);

            //Bio
            StackLayout bioStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var biolbl = new Label { Text = "Bio: ", FontSize = 20, HorizontalOptions = LayoutOptions.Start  };
            var bio = new Label { Text = _currentProfile.user.bio, FontSize = 20, HorizontalOptions = LayoutOptions.Start };
            
            bioStackLayout.Add(biolbl);
            bioStackLayout.Add(bio);
            imageLayout.Add(bioStackLayout);
            
            //Buttons
            StackLayout buttonStackLayout = new StackLayout{Orientation = StackOrientation.Horizontal};
            var likeButton = new Button { Text = "Like", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            var dislikeButton = new Button { Text = "Dislike", FontSize = 20, HorizontalOptions = LayoutOptions.Center };
            
            likeButton.Clicked += (sender, args) => {
                CheckIfQueueNeedsMoreProfiles();
                string emailCurrentUser = Authentication._currentUser.email;
                string emailLikedUser = _currentProfile.user.email;
                if (_database.CheckMatch(emailCurrentUser, emailLikedUser))
                {
                    _database.NewMatch(emailLikedUser, emailCurrentUser);
                    _database.deleteLikeOnMatch(emailCurrentUser, emailLikedUser);
                }
                else {
                    _database.NewLike(emailCurrentUser, emailLikedUser);
                }

                //en krijg een pop-up dat je een match hebt

                //en daarna door naar de volgende persoon
            };
            dislikeButton.Clicked += (sender, args) =>
            {
                CheckIfQueueNeedsMoreProfiles();
                string emailCurrentUser = Authentication._currentUser.email;
                string emaildDislikedUser = _currentProfile.user.email;

                _database.NewDislike(emailCurrentUser, emaildDislikedUser);

            };
            buttonStackLayout.Add(dislikeButton);
            buttonStackLayout.Add(likeButton);

        }
        verticalStackLayout.Add(imageLayout);
        
        verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        Content = verticalStackLayout;
    }

    private async Task UpdateQueue() {
        Task gettingProfiles = GetProfiles();
        await gettingProfiles;
    }
    
    private async Task GetProfiles() {
        Profile[] profiles =  _database.Get5Profiles(Authentication._currentUser.email);
        foreach (var profile in profiles) {
            _profileQueue.Enqueue(profile);
        }
    }
    
    private void CheckIfQueueNeedsMoreProfiles() {
        if(_profileQueue.Count < 5) {
            UpdateQueue();
        }
    }
}