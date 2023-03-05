namespace DataModel;

public class InterestsModel
{

    public static List<string> InterestsList { get; private set; } = new List<string>();

    public InterestsModel(List<string> interestsItems)
    {
        InterestsList = interestsItems;
    }


}