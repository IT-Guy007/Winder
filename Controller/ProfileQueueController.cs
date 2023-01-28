using System.Data.SqlClient;

namespace DataModel;

public class ProfileQueueController {
    
    private Queue<Profile> ProfileQueue { get; }
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
        ProfileQueue = new Queue<Profile>();
        User = user;
        CheckIfQueueNeedsMoreProfiles(connection);
    }
    
    /// <summary>
    /// Gets the next profile in the queue
    /// </summary>
    /// <returns>Profile</returns>
    public void GetNextProfile() {
        try {
            CurrentProfile = ProfileQueue.Dequeue();
        } catch {
            Console.WriteLine("No more profiles to swipe at this moment");
        }
    }
    
    /// <summary>
    /// Empties the queue
    /// </summary>
    public void ClearQueue() {
        ProfileQueue.Clear();
    }
    
    public void ClearCurrentProfile() {
        CurrentProfile = null;
    }
    
    /// <summary>
    /// Gets the amount of profiles in the queue
    /// </summary>
    /// <returns>Integer of the amount of profiles</returns>
    public int GetQueueCount() {
        return ProfileQueue.Count;
    }
    
    /// <summary>
    /// Adds a profile to the queue
    /// </summary>
    /// <param name="profile">Given profile</param>
    private void AddProfile(Profile profile) {
        ProfileQueue.Enqueue(profile);
    }
    
    /// <summary>
    /// Check's if a queue needs more profiles
    /// </summary>
    public async void CheckIfQueueNeedsMoreProfiles(SqlConnection connection) {
        if (ProfileQueue.Count < AmountOfProfilesInQueue && !IsGettingProfiles) {
            IsGettingProfiles = true;
            await GetProfilesTask(connection);
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
                AddProfile(profile);
            }
        }
    }
    
    /// <summary>
    /// Gets the profiles from the database async
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private Profile[] GetProfiles(SqlConnection connection) {
        
        //The users(Email) to get
        List<string> usersToRetrief = new List<string>();

        usersToRetrief = AlgorithmForSwiping(connection);

        //Results
        Profile[] profiles = new Profile[usersToRetrief.Count()];

        //Retrieving
        for (int i = 0; i < usersToRetrief.Count(); i++) {

            //Get the user
            User user = new User().GetUserFromDatabase(usersToRetrief[i],Database2.ReleaseConnection);

            //Get the images of the user
            byte[][] images = user.GetPicturesFromDatabase(Database2.ReleaseConnection);
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

        string query = "select top " + AmountOfProfilesInQueue * 2 + " Email " +
                       "from winder.[User] " +
                       "where Email != @Email " + //Not themself
                       "and active = 1 " + //Is active
                       "and Email not in (select person from Winder.Winder.Liked where likedPerson = @Email and liked = 1) " + //Not disliked by other person
                       "and Email not in (select likedPerson from winder.winder.Liked where person = @Email) " + //Not already a person that you liked or disliked
                       "and Email not in (select winder.winder.Match.person1 from Winder.Winder.Match where person2 = @Email) " + //Not matched
                       "and Email not in (select winder.winder.Match.person2 from Winder.Winder.Match where person1 = @Email) " + //Not matched
                       "And location = (select location from winder.winder.[User] where Email = @Email) "; // location check

        if (AgeAlgorithm) {
            query = query + " and birthday >= '" + formattedMax + "' and birthday <= '" + formattedMin + "'  "; //In age range
        }

        if (PreferenceAlgorithm)
        {
            query = query + " and Gender = (select Preference from winder.winder.[User] where Email = @Email) "; //Gender check
            query = query + " and Preference = (select Gender from winder.winder.[User] where Email = @Email) "; //Preference check
        }

        if (InterestsAlgorithm) {
            if (User.Interests.Length > 0) {
                //Add interests
                query = query + " AND Email in (select UID from winder.winder.UserHasInterest where interest = " + "'" + User.Interests[0] + "' ";
                for (int i = 1; i < User.Interests.Length; i++)
                {
                    query = query + " or interest =" + " '" + User.Interests[i] + "' ";
                }
                query = query + ")";
            }
        }
        
        //Randomize the result
        query = query + " ORDER BY NEWID()";
        
        SqlCommand command = new SqlCommand(query, connection);
        //Get the users emails
        try {
            SqlDataReader reader = command.ExecuteReader(); // execute het command
            if (reader.HasRows) {
                while (reader.Read()) {
                    string user = reader["Email"] as string ?? "";
                    usersToSwipe.Enqueue(user);
                }
            } else {
                Console.WriteLine("No new profiles found to swipe");
            }
        } catch(SqlException e) {
            Console.WriteLine("Error retrieving the users for the algorithm");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
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

        SqlCommand command = new SqlCommand("SELECT top " + UsersInQueueWhoLikedYou + " person FROM Winder.Winder.Liked " +
                                            "WHERE likedPerson = @Email AND liked = 1 " + // selects the users that have liked the given user
                                            "AND person not in (select likedPerson from Winder.Winder.Liked where person = @Email) " + // except the ones that the given user has already disliked or liked
                                            "order by NEWID()", connection); 
        command.Parameters.AddWithValue("@Email", User.Email);

        try {
            SqlDataReader reader = command.ExecuteReader(); // execute het command

            while (reader.Read()) {
                string person = reader["person"] as string ?? "Unknown";   
                users.Enqueue(person);   // zet elk persoon in de users 
            }


        } catch (SqlException se) {
            Console.WriteLine("Error retrieving users who liked you from database");
            Console.WriteLine(se.ToString());
            Console.WriteLine(se.StackTrace);


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
                IEnumerable<Profile> alreadyExists = ProfileQueue.Where(i => i.user.Email == userToAdd);
                if (!alreadyExists.Any() && !usersToSwipe.Contains(userToAdd) && !string.IsNullOrEmpty(userToAdd)) {
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
    
}