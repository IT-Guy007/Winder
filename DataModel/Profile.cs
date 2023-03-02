using System.Data.SqlClient;

namespace DataModel;

public class Profile {
    public User User { get; set; }
    public byte[][] ProfileImages { get; set; }
    
    public Profile(User user, byte[][] profileImages) {
        User = user;
        ProfileImages = profileImages;
    }

}