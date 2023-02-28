using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories
{
    internal class MatchRepository : IMatchRepository
    {
        public bool AddMatch(string emailLikedPerson, string emailCurrentUser)
        {
            throw new NotImplementedException();
        }

        public List<Match> GetMatchedStudentsFromUser(string email)
        {
            throw new NotImplementedException();
        }
    }
}
