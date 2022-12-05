using System.Drawing;

namespace DataModel;

public class Profile
{
    public User user { get; set; }
    public Image[] profile_images { get; set; }
    
    
    public Profile(User user, Image[] profile_images)
    {
        this.user = user;
        this.profile_images = profile_images;
    }
    
}