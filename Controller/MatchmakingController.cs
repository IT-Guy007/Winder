using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class MatchmakingController
    {
        private readonly IUserRepository _userRepository; 
        private readonly ILikedRepository _likeRepository;


        public MatchmakingController(IUserRepository userRepository, ILikedRepository likedRepository)
        {
            _userRepository = userRepository;
            _likeRepository = likedRepository;
        }



    }
}
