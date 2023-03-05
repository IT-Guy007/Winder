namespace DataModel;

public class ProfileQueue
{
    public Queue<Profile> ProfileItems { get; set; }

    public ProfileQueue()
    {
        ProfileItems = new Queue<Profile>();
    }

}