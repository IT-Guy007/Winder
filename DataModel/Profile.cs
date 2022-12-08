using Microsoft.Maui.Controls;

namespace DataModel;

public class Profile
{
    public User user { get; set; }
    public byte[][] profile_images { get; set; }
    
    
    public Profile(User user, byte[][] profile_images)
    {
        this.user = user;
        this.profile_images = profile_images;
    }
    
}