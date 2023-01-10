namespace DataModel;

public class Profile
{
    public User user { get; set; }
    public byte[][] profileImages { get; set; }
    
    
    public Profile(User user, byte[][] profileImages)
    {
        this.user = user;
        this.profileImages = profileImages;
    }
    
}