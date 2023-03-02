using System.Data.SqlClient;

namespace DataModel;

public class AlgorithmModel {
    
    //Algorithms to use
    private const bool AgeAlgorithm = true;
    private const bool PreferenceAlgorithm = true;
    private const bool InterestsAlgorithm = true;
    public const int AmountOfProfilesInQueue = 5;
    private const int UsersInQueueWhoLikedYou = 1;
    
    /// <summary>
    /// The algorithm for swiping
    /// </summary>
    /// <param name="connection">The database connection</param>
    /// <returns></returns>
    public List<string> AlgorithmForSwiping(User user, SqlConnection connection) {
        
        Queue<string> usersToSwipe = new Queue<string>();

        DateTime minDate = DateTime.Now.AddYears(0 - user.MinAge);
        DateTime maxDate = DateTime.Now.AddYears(0 - user.MaxAge);
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
            if (user.Interests.Length > 0) {
                //Add interests
                query = query + " AND Email IN (SELECT UID FROM winder.winder.UserHasInterest WHERE interest = " + "'" + user.Interests[0] + "' ";
                for (int i = 1; i < user.Interests.Length; i++)
                {
                    query = query + " OR interest =" + " '" + user.Interests[i] + "' ";
                }
                query = query + ")";
            }
        }
        
        //Randomize the result
        query = query + " ORDER BY NEWID()";
        
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", user.Email);
        
        //Get the users emails
        SqlDataReader reader = null;
        try {
             reader = command.ExecuteReader(); // execute het command
            if (reader.HasRows) {
                while (reader.Read()) {
                    string userEmail = reader["Email"] as string ?? "";
                    usersToSwipe.Enqueue(userEmail);
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
        Queue<string> usersWhoLikedYou = GetUsersWhoLikedYou(user,connection);
        Queue<string> likedQueue = new Queue<string>();
        foreach (string userWhoLikedYou in usersWhoLikedYou) {
            likedQueue.Enqueue(userWhoLikedYou);
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
    private Queue<string> GetUsersWhoLikedYou(User user,SqlConnection connection) {

        Queue<string> users = new Queue<string>();

        SqlCommand command = new SqlCommand("SELECT TOP " + UsersInQueueWhoLikedYou + " person FROM Winder.Winder.Liked " +
                                            "WHERE likedPerson = @Email AND liked = 1 " + // selects the users that have liked the given user
                                            "AND person NOT IN (SELECT likedPerson FROM Winder.Winder.Liked WHERE person = @Email) " + // except the ones that the given user has already disliked or liked
                                            "ORDER BY NEWID()", connection); 
        command.Parameters.AddWithValue("@Email", user.Email);

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
                if (!new ProfileQueue().Contains(userToAdd) && !usersToSwipe.Contains(userToAdd) && !string.IsNullOrEmpty(userToAdd)) {
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
    /// <summary>
    /// Gets the profiles from the database async
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Array of profiles</returns>
    public Profile[] GetProfiles(User user, SqlConnection connection) {
        
        //The users(Email) to get
        List<string> usersToRetrief = new List<string>();

        usersToRetrief = new AlgorithmModel().AlgorithmForSwiping(user,connection);

        //Results
        Profile[] profiles = new Profile[usersToRetrief.Count()];

        //Retrieving
        for (int i = 0; i < usersToRetrief.Count(); i++) {

            //Get the user
            User userItem = new User().GetUserFromDatabase(usersToRetrief[i],connection);

            //Get the images of the user
            byte[][] images = userItem.GetPicturesFromDatabase(connection);
            var profile = new Profile(userItem, images);

            profiles[i] = profile;
        }

        return profiles;
    }
    
}