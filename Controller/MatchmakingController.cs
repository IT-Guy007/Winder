using DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class MatchmakingController
    {
        private ProfileQueue ProfileQueue { get; }
        private MatchModel MatchModel { get; }

        public Profile CurrentProfile { get; private set; }


        private readonly IMatchRepository _matchRepository;

        public MatchmakingController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;

            ProfileQueue = new ProfileQueue();
            MatchModel = new MatchModel(_matchRepository.GetMatchedStudentsFromUser(Authentication.CurrentUser));
        }

        public List<Match> GetMatchedStudentsFromUser(User user)
        {
            return _matchRepository.GetMatchedStudentsFromUser(user);
        }


    }
}
