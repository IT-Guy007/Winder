using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories;
using Winder.Repositories.Interfaces;
using DataModel;

namespace Controller
{
    public class LikeDislikeController
    {
        Profile CurrentProfile;
        private bool IsGettingProfiles { get; set; }


        private readonly ILikedRepository _likeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPhotosRepository _photosRepository;


        public LikeDislikeController(ILikedRepository likedRepository, IUserRepository userRepository, IMatchRepository matchRepository, IPhotosRepository photosRepository)
        {
            _likeRepository = likedRepository;
            _userRepository = userRepository;
            _matchRepository = matchRepository;
            _photosRepository = photosRepository;

            CheckIfQueueNeedsMoreProfiles();
        }

        public void OnLike()
        {
            if (_likeRepository.CheckMatch(Authentication.CurrentUser.Email, CurrentProfile.User.Email))
            {
                _matchRepository.AddMatch(Authentication.CurrentUser.Email, CurrentProfile.User.Email);
                _likeRepository.DeleteLike(Authentication.CurrentUser.Email, CurrentProfile.User.Email);
            }
            else
            {
                _likeRepository.NewLike(Authentication.CurrentUser.Email, CurrentProfile.User.Email);
            }
            NextProfile();
        }

        public void OnDislike()
        {
            _likeRepository.NewDislike(Authentication.CurrentUser.Email, CurrentProfile.User.Email);
            NextProfile();
        }

        public void NextProfile()
        {

            CheckIfQueueNeedsMoreProfiles();
            if (new ProfileQueue().GetCount() != 0)
            {
                CurrentProfile = new ProfileQueue().GetNextProfile();

            }
            else
            {
                new ProfileQueue().Clear();
                CurrentProfile = null;
            }

        }
        
        

        /// <summary>
        /// Check's if a queue needs more profiles
        /// </summary>
        private void CheckIfQueueNeedsMoreProfiles()
        {
            if (new ProfileQueue().GetCount() < AlgorithmModel.AmountOfProfilesInQueue && !IsGettingProfiles)
            {
                IsGettingProfiles = true;
                GetProfilesTask(Authentication.CurrentUser);
                IsGettingProfiles = false;
            }
        }

        /// <summary>
        /// Gets the profiles from the database async
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Array of profiles</returns>
        public Profile[] GetProfiles(User user)
        {

            //The users(Email) to get
            List<string> usersToRetrief = new List<string>();

            usersToRetrief = _userRepository.GetConditionBasedUsers(user);

            //Results
            Profile[] profiles = new Profile[usersToRetrief.Count()];

            //Retrieving
            for (int i = 0; i < usersToRetrief.Count(); i++)
            {

                //Get the user
                User userItem = _userRepository.GetUserFromDatabase(usersToRetrief[i]);

                //Get the images of the user
                byte[][] images = _photosRepository.GetPhotos(user.Email);
                var profile = new Profile(userItem, images);

                profiles[i] = profile;
            }

            return profiles;
        }

        /// <summary>
        /// The task to get the profiles
        /// </summary>
        /// <param name="user">The user object to retrief it from</param>
        /// <param name="connection">The database connection</param>
        private async Task GetProfilesTask(User user)
        {
            var profiles = GetProfiles(user);
            foreach (var profile in profiles)
            {
                if (profile != null)
                {
                    new ProfileQueue().Add(profile);
                }
            }
        }
    }
}
