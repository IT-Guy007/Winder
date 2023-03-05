using DataModel;
using Winder.Repositories.Interfaces;

namespace Controller;

public class MatchmakingController
{
    private readonly IUserRepository _userRepository;
    private readonly ILikedRepository _likeRepository;
    private readonly IPhotosRepository _photosRepository;

    private ProfileQueue _profileQueue;

    public Profile CurrentProfile { get; private set; }
    private bool IsGettingProfiles { get; set; }


    public MatchmakingController(IUserRepository userRepository, ILikedRepository likedRepository, IPhotosRepository photosRepository)
    {
        _userRepository = userRepository;
        _likeRepository = likedRepository;
        _photosRepository = photosRepository;
        _profileQueue = new ProfileQueue();
        
        CheckIfQueueNeedsMoreProfiles();
    }

    /// <summary>
    /// Check's if a queue needs more profiles
    /// </summary>
    private void CheckIfQueueNeedsMoreProfiles()
    {

        if (GetCount() < 5 && !IsGettingProfiles)
        {
            IsGettingProfiles = true;
            GetProfilesTask(Authentication.CurrentUser);
            IsGettingProfiles = false;
        }

    }
    
    /// <summary>
    /// Get profile to validate
    /// </summary>
    /// <param name="user">The current user</param>
    /// <returns>Array of profiles</returns>
    private Profile[] GetProfiles(User user)
    {
        //The users(Email) to get
        List<string> usersToRetrief = new List<string>();

        usersToRetrief = _userRepository.GetConditionBasedUsers(user);

        //Results
        Profile[] profiles = new Profile[usersToRetrief.Count()];

        //Retrieving
        for (int i = 0; i < usersToRetrief.Count(); i++) {

            //Get the user
            User userItem = _userRepository.GetUserFromDatabase(usersToRetrief[i]);

            //Get the images of the user
            byte[][] images = _photosRepository.GetPhotos(userItem.Email);
            var profile = new Profile(userItem, images);

            profiles[i] = profile;
        }

        return profiles;
    }

    /// <summary>
    /// The task to get the profiles
    /// </summary>
    /// <param name="user">The user object to retrieve it from</param>
    private Task GetProfilesTask(User user)
    {
        var profiles = GetProfiles(user);
        foreach (var profile in profiles)
        {
            if (profile != null)
            {
                Add(profile);
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the next profile in the queue
    /// </summary>
    public void NextProfile()
    {

        CheckIfQueueNeedsMoreProfiles();
        if (GetCount() != 0)
        {
            CurrentProfile = GetNextProfile();

        } 
        else
        {
            CurrentProfile = null;
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public void OnLike()
    {

        if (_likeRepository.CheckMatch(Authentication.CurrentUser.Email, CurrentProfile.User.Email))
        {
            _likeRepository.CreateMatch(Authentication.CurrentUser.Email, CurrentProfile.User.Email);
            _likeRepository.DeleteLike(Authentication.CurrentUser.Email, CurrentProfile.User.Email);
        }
        else
        {
            _likeRepository.NewLike(Authentication.CurrentUser.Email, CurrentProfile.User.Email);
        }

        NextProfile();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="emailFirstPerson"></param>
    /// <param name="emailSecondPerson"></param>
    /// <returns></returns>
    public bool CheckMatch(string emailFirstPerson, string emailSecondPerson)
    {
        return _likeRepository.CheckMatch(emailFirstPerson, emailSecondPerson);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnDislike()
    {

        _likeRepository.NewDislike(Authentication.CurrentUser.Email, CurrentProfile.User.Email);
        NextProfile();
        
    }
    
    /// <summary>
    /// Add profile to queue
    /// </summary>
    /// <param name="profile">The profile to add</param>
    private void Add(Profile profile) {
        _profileQueue.ProfileItems.Enqueue(profile);
    }
    
    /// <summary>
    /// Get the next profile in the queue
    /// </summary>
    /// <returns>Profile or null</returns>
    private Profile GetNextProfile() {
        try {
            return _profileQueue.ProfileItems.Dequeue();
        } catch(Exception) {
            Console.WriteLine("No more profiles in queue");
            return null;
        }
    }
    
    /// <summary>
    /// Checks if the queue is empty
    /// </summary>
    /// <returns>Boolean if the queue is empty</returns>
    public bool IsEmpty() {
        return _profileQueue.ProfileItems.Count == 0;
    }
    
    /// <summary>
    /// Get the count of the queue
    /// </summary>
    /// <returns>Integer with amount of items</returns>
    private int GetCount() {
        return _profileQueue.ProfileItems.Count;
    }


    /// <summary>
    /// Checks if the given email is in the profilequeue
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Bool if true</returns>
    public bool CurrentQueueContainsTheGivenEmail(string email) {
        return _profileQueue.ProfileItems.Any(p => p.User.Email == email);
    }

}

