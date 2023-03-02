using DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winder.Repositories.Interfaces
{
    internal interface IMatchRepository
    {
        public List<Match> GetMatchedStudentsFromUser(User currentUser);
        public bool AddMatch(string emailLikedPerson, string emailCurrentUser);
    }
}
