namespace DataModel;

public class ProfileModel {
    public Queue<Profile> ProfilesQueue { get; set; }
    
    public ProfileModel() {
        ProfilesQueue = new Queue<Profile>();
    }
    
    
    
    
}