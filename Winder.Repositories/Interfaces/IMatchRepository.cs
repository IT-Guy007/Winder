using DataModel;

namespace Winder.Repositories.Interfaces
{
    internal interface IMatchRepository
    {
        public List<Match> GetMatchedStudentsFromUser(User currentUser);
        public bool AddMatch(string emailLikedPerson, string emailCurrentUser);
    }
}