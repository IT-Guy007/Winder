using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class LikeDislikeController
    {
        private readonly ILikedRepository _likeRepository;


        public LikeDislikeController(ILikedRepository likedRepository)
        {
            _likeRepository = likedRepository;
        }
        
        

    }
}
