namespace DataModel;

public class MatchModel
{
    private List<Match> Matches { get; set; }

    public MatchModel(List<Match> matches) {
        Matches = matches;
    }

    public void AddMatch(Match match) {
        Matches.Add(match);
    }
    
    public List<User> GetUsers() {
        return Matches.Select(x => x.Person2).ToList();
    }

}