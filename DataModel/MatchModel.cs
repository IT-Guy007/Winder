namespace DataModel;

public class MatchModel
{

    public MatchList Matches { get; }

    public MatchModel(List<Match> matches)
    {
        Matches = new MatchList(matches);
    }

}