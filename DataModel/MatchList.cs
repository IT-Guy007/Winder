namespace DataModel;

public class MatchList
{
    public List<Match> Matches { get; private set; }

    public MatchList(List<Match> matches)
    {
        Matches = matches;
    }

    public List<User> GetUsers()
    {
        return Matches.Select(x => x.Person2).ToList();
    }
}