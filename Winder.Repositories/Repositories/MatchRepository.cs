using DataModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly IConfiguration _configuration;

        public MatchRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
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
