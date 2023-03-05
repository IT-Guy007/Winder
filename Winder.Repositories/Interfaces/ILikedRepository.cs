using DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winder.Repositories.Interfaces
{
    public interface ILikedRepository
    {
        public bool CheckMatch(string emailLikedPerson, string emailCurrentUser);
        public bool NewLike(string emailLikedPerson, string emailCurrentUser);
        public bool NewDislike(string emailLikedPerson, string emailCurrentUser);
        public bool DeleteLike(string emailLikedPerson, string emailCurrentUser);
        public Queue<string> GetUsersWhoLikedYou(string email);
        public bool CreateMatch(string emailFirstPerson, string emailSecondPerson);
    }
}
