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

        }

    }
}
