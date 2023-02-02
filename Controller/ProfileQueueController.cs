using DataModel;
using System.Data.SqlClient;

namespace Controller;

public class ProfileQueueController {
    
    private ProfileQueue ProfileQueue { get;}
    private MatchModel MatchModel { get;}

    public Profile CurrentProfile { get; private set; }
    private bool IsGettingProfiles { get; set; }

    private User User { get; }

    public ProfileQueueController(User user, SqlConnection connection) {
        ProfileQueue = new ProfileQueue();
        MatchModel = new MatchModel(user.GetMatchedStudentsFromUser(connection));
        User = user;
        CheckIfQueueNeedsMoreProfiles(connection);
    }

    /// <summary>
    /// Check's if a queue needs more profiles
    /// </summary>
    private void CheckIfQueueNeedsMoreProfiles(SqlConnection connection) {
        if (ProfileQueue.GetCount() < AlgorithmModel.AmountOfProfilesInQueue && !IsGettingProfiles) {
            IsGettingProfiles = true;
            GetProfilesTask(Authentication.CurrentUser,connection);
            IsGettingProfiles = false;
        }
    }
    
    /// <summary>
    /// The task to get the profiles
    /// </summary>
    /// <param name="user">The user object to retrief it from</param>
    /// <param name="connection">The database connection</param>
    private async Task GetProfilesTask(User user,SqlConnection connection) {
        var profiles = new AlgorithmModel().GetProfiles(user, connection);
        foreach (var profile in profiles) {
            if (profile != null) {
                ProfileQueue.Add(profile);
            }
        }
    }

    public void NextProfile(SqlConnection connection) {

       CheckIfQueueNeedsMoreProfiles(connection);
        if (ProfileQueue.GetCount() != 0) {
            CurrentProfile = ProfileQueue.GetNextProfile();

        } else {
            ProfileQueue.Clear();
            CurrentProfile = null;
        }

    }
    
    public void OnLike(SqlConnection connection) {
        if(MatchModel.CheckMatch(Authentication.CurrentUser.Email, CurrentProfile.User.Email, connection)) {
            MatchModel.NewMatch(Authentication.CurrentUser.Email, CurrentProfile.User.Email, connection);
            MatchModel.DeleteLike(Authentication.CurrentUser.Email, CurrentProfile.User.Email, connection);
        } else {
            MatchModel.NewLike(Authentication.CurrentUser.Email, CurrentProfile.User.Email, connection);
        }
        NextProfile(connection);
    }

    public void OnDislike(SqlConnection connection) {
        MatchModel.NewDislike(Authentication.CurrentUser.Email, CurrentProfile.User.Email, connection);
        NextProfile(connection);
    }
    
    
    
}