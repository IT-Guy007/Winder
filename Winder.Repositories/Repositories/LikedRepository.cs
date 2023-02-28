using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories
{
    internal class LikedRepository : ILikedRepository
    {
        public bool CheckMatch(string emailLikedPerson, string emailCurrentUser)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLike(string emailLikedPerson, string emailCurrentUser)
        {
            throw new NotImplementedException();
        }

        public List<string> GetLikes(string emailCurrentUser)
        {
            throw new NotImplementedException();
        }

        public bool NewDislike(string emailLikedPerson, string emailCurrentUser)
        {
            throw new NotImplementedException();
        }

        public bool NewLike(string emailLikedPerson, string emailCurrentUser)
        {
            throw new NotImplementedException();
        }
    }
}
