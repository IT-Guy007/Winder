namespace DataModel;

public class Profile
{
    public User user { get; set; }
    public byte[][] profileImages { get; set; }
    
    
    public Profile(User user, byte[][] profile_images)
    {
        this.user = user;
        this.profileImages = profile_images;
    }
    
}