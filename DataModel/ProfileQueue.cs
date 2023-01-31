namespace DataModel;

public class ProfileQueue {
    private Queue<Profile> ProfileItems { get; set; }
    
    public ProfileQueue() {
        ProfileItems = new Queue<Profile>();
    }
    
    /// <summary>
    /// Add profile to queue
    /// </summary>
    /// <param name="profile">The profile to add</param>
    public void Add(Profile profile) {
        ProfileItems.Enqueue(profile);
    }
    
    /// <summary>
    /// Get the next profile in the queue
    /// </summary>
    /// <returns>Profile or null</returns>
    public Profile GetNextProfile() {
        try {
            return ProfileItems.Dequeue();
        } catch(Exception e) {
            Console.WriteLine("No more profiles in queue");
            return null;
        }
    }
    
    /// <summary>
    /// Checks if the queue is empty
    /// </summary>
    /// <returns>Boolean if the queue is empty</returns>
    public bool IsEmpty() {
        return ProfileItems.Count == 0;
    }
    
    /// <summary>
    /// Get the count of the queue
    /// </summary>
    /// <returns>Integer with amount of items</returns>
    public int GetCount() {
        return ProfileItems.Count;
    }
    
    /// <summary>
    /// Clear the queue
    /// </summary>
    public void Clear() {
        ProfileItems.Clear();
    }
    
    /// <summary>
    /// Remove specific profile from queue
    /// </summary>
    /// <param name="profile">The profile to remove from the queue</param>
    public void Remove(Profile profile) {
        ProfileItems = new Queue<Profile>(ProfileItems.Where(p => p.user.Email != profile.user.Email));
    }
    
    public Boolean Contains(string email) {
        return ProfileItems.Any(p => p.user.Email == email);
    }

}