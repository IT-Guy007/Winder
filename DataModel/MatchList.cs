namespace DataModel;

public class MatchList {
    public List<Match> Matches { get; private set; }
    
    public MatchList(List<Match> matches) {
        Matches = matches;
    }
    
    public void Add(Match match) {
        Matches.Add(match);
    }
    
    public void Remove(Match match) {
        Matches.Remove(match);
    }
    
    public List<User> GetUsers() {
        return Matches.Select(x => x.Person2).ToList();
    }
}