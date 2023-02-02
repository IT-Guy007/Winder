using DataModel;
using System.Data.SqlClient;

namespace Controller;

public class ProfileQueueController {
    public ProfileQueue ProfileQueue;
    public SwipeController SwipeController;
    
    public Profile CurrentProfile { get; private set; }
    private bool IsGettingProfiles { get; set; }
    
    private const int AmountOfProfilesInQueue = 5;
    private const int UsersInQueueWhoLikedYou = 1;
    
    private User User { get; set; }
    
    //Algorithms to use
    private const bool AgeAlgorithm = true;
    private const bool PreferenceAlgorithm = true;
    private const bool InterestsAlgorithm = true;
    
    public ProfileQueueController(User user, SqlConnection connection) {
        ProfileQueue = new ProfileQueue();
        SwipeController = new SwipeController();
        User = user;
        CheckIfQueueNeedsMoreProfiles(connection);
    }

    /// <summary>
    /// Check's if a queue needs more profiles
    /// </summary>
    public async void CheckIfQueueNeedsMoreProfiles(SqlConnection connection) {
        if (ProfileQueue.GetCount() < AmountOfProfilesInQueue && !IsGettingProfiles) {
            IsGettingProfiles = true;
            GetProfilesTask(connection);
            IsGettingProfiles = false;
        }
    }
    
    /// <summary>
    /// The task to get the profiles
    /// </summary>
    /// <param name="user">The user object to retrief it from</param>
    /// <param name="connection">The database connection</param>
    private async Task GetProfilesTask(SqlConnection connection) {
        Profile[] profiles = GetProfiles(connection);
        foreach (var profile in profiles) {
            if (profile != null) {
                ProfileQueue.Add(profile);
            }
        }
    }
    
    /// <summary>
    /// Gets the profiles from the database async
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Array of profiles</returns>
    private Profile[] GetProfiles(SqlConnection connection) {
        
        //The users(Email) to get
        List<string> usersToRetrief = new List<string>();

        usersToRetrief = AlgorithmForSwiping(connection);

        //Results
        Profile[] profiles = new Profile[usersToRetrief.Count()];

        //Retrieving
        for (int i = 0; i < usersToRetrief.Count(); i++) {

            //Get the user
            User user = new User().GetUserFromDatabase(usersToRetrief[i],connection);

            //Get the images of the user
            byte[][] images = user.GetPicturesFromDatabase(connection);
            var profile = new Profile(user, images);

            profiles[i] = profile;
        }

        return profiles;
    }
    
    /// <summary>
    /// The algorithm for swiping
    /// </summary>
    /// <param name="connection">The database connection</param>
    /// <returns></returns>
    private List<string> AlgorithmForSwiping(SqlConnection connection) {
        
        Queue<string> usersToSwipe = new Queue<string>();

        DateTime minDate = DateTime.Now.AddYears(0 - User.MinAge);
        DateTime maxDate = DateTime.Now.AddYears(0 - User.MaxAge);
        var formattedMin = minDate.ToString("yyyy-MM-dd HH:mm:ss");
        var formattedMax = maxDate.ToString("yyyy-MM-dd HH:mm:ss");

        string query = "SELECT TOP " + AmountOfProfilesInQueue * 2 + " Email " +
                       "FROM winder.[User] " +
                       "WHERE Email != @Email " + //Not themself
                       "AND active = 1 " + //Is active
                       "AND Email NOT IN (SELECT person FROM Winder.Winder.Liked WHERE likedPerson = @Email AND liked = 1) " + //Not disliked by other person
                       "AND Email NOT IN (SELECT likedPerson FROM winder.winder.Liked WHERE person = @Email) " + //Not already a person that you liked or disliked
                       "AND Email NOT IN (SELECT winder.winder.Match.person1 FROM Winder.Winder.Match WHERE person2 = @Email) " + //Not matched
                       "AND Email NOT IN (SELECT winder.winder.Match.person2 FROM Winder.Winder.Match WHERE person1 = @Email) " + //Not matched
                       "AND location = (SELECT location FROM winder.winder.[User] WHERE Email = @Email) "; // location check

        if (AgeAlgorithm) {
            query = query + " AND birthday >= '" + formattedMax + "' AND birthday <= '" + formattedMin + "'  "; //In age range
        }

        if (PreferenceAlgorithm) {
            query = query + " AND Gender = (SELECT Preference FROM winder.winder.[User] WHERE Email = @Email) "; //Gender check
            query = query + " AND Preference = (SELECT Gender FROM winder.winder.[User] WHERE Email = @Email) "; //Preference check
        }

        if (InterestsAlgorithm) {
            if (User.Interests.Length > 0) {
                //Add interests
                query = query + " AND Email IN (SELECT UID FROM winder.winder.UserHasInterest WHERE interest = " + "'" + User.Interests[0] + "' ";
                for (int i = 1; i < User.Interests.Length; i++)
                {
                    query = query + " OR interest =" + " '" + User.Interests[i] + "' ";
                }
                query = query + ")";
            }
        }
        
        //Randomize the result
        query = query + " ORDER BY NEWID()";
        
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", User.Email);
        
        //Get the users emails
        SqlDataReader reader = null;
        try {
             reader = command.ExecuteReader(); // execute het command
            if (reader.HasRows) {
                while (reader.Read()) {
                    string user = reader["Email"] as string ?? "";
                    usersToSwipe.Enqueue(user);
                }
            } else {
                Console.WriteLine("No new profiles found to swipe");
            }
            
        } catch (SqlException e) {
            Console.WriteLine("Error retrieving the users for the algorithm");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        } finally  {
            if (reader != null) reader.Close();
        }
        
        

        //Get 5 users who likes you and add to qu
        Queue<string> usersWhoLikedYou = GetUsersWhoLikedYou(connection);
        Queue<string> likedQueue = new Queue<string>();
        foreach (string user in usersWhoLikedYou) {
            likedQueue.Enqueue(user);
        }

        //Loop though users and check if in currentQueue
        List<string> result = CheckCurrentQueue(usersToSwipe,likedQueue);


        return result;
    }
    
    /// <summary>
    /// Retrieves 5 users who liked you
    /// </summary>
    /// <param name="connection">The database connection</param>
    /// <returns>Queue with liked emails</returns>
    private Queue<string> GetUsersWhoLikedYou(SqlConnection connection) {

        Queue<string> users = new Queue<string>();

        SqlCommand command = new SqlCommand("SELECT TOP " + UsersInQueueWhoLikedYou + " person FROM Winder.Winder.Liked " +
                                            "WHERE likedPerson = @Email AND liked = 1 " + // selects the users that have liked the given user
                                            "AND person NOT IN (SELECT likedPerson FROM Winder.Winder.Liked WHERE person = @Email) " + // except the ones that the given user has already disliked or liked
                                            "ORDER BY NEWID()", connection); 
        command.Parameters.AddWithValue("@Email", User.Email);

        SqlDataReader reader = null;
        try {
            reader = command.ExecuteReader(); // execute het command

            while (reader.Read()) {
                string person = reader["person"] as string ?? "Unknown";   
                users.Enqueue(person);   // zet elk persoon in de users 
            }

            reader.Close();
        } catch (SqlException se) {
            Console.WriteLine("Error retrieving users who liked you from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);

        } finally  {
            if (reader != null) reader.Close();
        }
        
        return users;
    }
    
    /// <summary>
    /// Checks if the current queue contains the users that are retrieved and creates a list
    /// </summary>
    /// <param name="result"></param>
    /// <param name="usersToSwipe"></param>
    /// <param name="likedQueue"></param>
    private List<string> CheckCurrentQueue(Queue<string> usersToSwipe, Queue<string> likedQueue) {
        List<string> result = new List<string>();
        
        //Place for the users who liked you
        Random random = new Random();
        int place = random.Next(0, AmountOfProfilesInQueue);
        
        
        while (result.Count != AmountOfProfilesInQueue && (usersToSwipe.Count > 0 || likedQueue.Count > 0)) {

            var userToAdd = "";

            try {
                if (place == result.Count) {
                    //For the liked user place else random user
                    if (likedQueue.Count > 0) {
                        userToAdd = likedQueue.Dequeue(); 
                    } else {
                        userToAdd = usersToSwipe.Dequeue();
                    }
                } else {
                    if (usersToSwipe.Count > 0) {
                        userToAdd = usersToSwipe.Dequeue();
                    }
                    else {
                        userToAdd = likedQueue.Dequeue();
                    }
                }

                //Check if already exists in current Queue
                if (!ProfileQueue.Contains(userToAdd) && !usersToSwipe.Contains(userToAdd) && !string.IsNullOrEmpty(userToAdd)) {
                    Console.WriteLine("Adding user");
                    result.Add(userToAdd);
                }
            } catch (Exception e) {
                Console.WriteLine("Error adding user to queue");
                Console.WriteLine(e.ToString());
                Console.WriteLine(e.StackTrace);
            }

        }

        return result;
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
        if(SwipeController.CheckMatch(Authentication.CurrentUser.Email, CurrentProfile.user.Email, connection)) {
            SwipeController.NewMatch(Authentication.CurrentUser.Email, CurrentProfile.user.Email, connection);
            SwipeController.DeleteLike(Authentication.CurrentUser.Email, CurrentProfile.user.Email, connection);
        } else {
            SwipeController.NewLike(Authentication.CurrentUser.Email, CurrentProfile.user.Email, connection);
        }
        NextProfile(connection);
    }

    public void OnDislike(SqlConnection connection) {
        SwipeController.NewDislike(Authentication.CurrentUser.Email, CurrentProfile.user.Email, connection);
        NextProfile(connection);
    }
    
    
    
}