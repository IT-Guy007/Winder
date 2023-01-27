using System.Collections.ObjectModel;
using Microsoft.Maui.Storage;

namespace DataModel;
using System.Security.Cryptography;
using System.Text;

public class Authentication {
    
    private const string ValidationCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";

    public static User CurrentUser { get; set; }
    public static bool IsScaled = false;

    //Match
    public static Queue<Profile> ProfileQueue;
    public static Profile CurrentProfile;
    private static bool isGettingProfiles;
    
    private const int passwordLength =  8;

    public static void Initialize() {
        ProfileQueue = new Queue<Profile>();
        CurrentUser = new User();
        
    }

    // Calculating the age by using date as parameter
    public static int CalculateAge(DateTime birthDate) {
        int age = DateTime.Now.Year - birthDate.Year;
        if (DateTime.Now.DayOfYear < birthDate.DayOfYear) {
            age--;
        }

        return age;
    }

    // checks if validated
    public bool CheckPassword(string password)
    {
        if (PasswordLength(password) && PasswordContainsNumber(password) && PasswordContainsCapitalLetter(password))
        {
            return true;
        }
        return false;
    }


    private bool PasswordLength(string password) {

        return password.Length >= passwordLength;
        
    }

    private bool PasswordContainsNumber(string password)
    {
        return password.Any(char.IsDigit);
    }

    private bool PasswordContainsCapitalLetter(string password)
    {
        return password.Any(char.IsUpper);
    }


    //User to get the profiles for the match(run async)
    public static Profile[] Get5Profiles(string email) {
        

        //The users(Email) to get
        List<string> usersToRetrief = new List<string>();

        usersToRetrief = Database.AlgorithmForSwiping(email);

        //Results
        Profile[] profiles = new Profile[usersToRetrief.Count()];

        //Retrieving
        for (int i = 0; i < usersToRetrief.Count(); i++) {

            //Get the user
            User user = new User().GetUserFromDatabase(email,Database2.ReleaseConnection);

            //Get the images of the user
            byte[][] images = Database.GetPicturesFromDatabase(usersToRetrief[i]);
            var profile = new Profile(user, images);

            profiles[i] = profile;
        }

        return profiles;
    }

    public static async Task GetProfiles() {
        Profile[] profiles = Get5Profiles(CurrentUser.Email);
        foreach (var profile in profiles) {

            if (profile != null) {
                ProfileQueue.Enqueue(profile);
            }
        }
    }

    public static async void CheckIfQueueNeedsMoreProfiles()
    {
        if (ProfileQueue.Count < 5 && !isGettingProfiles)
        {
            isGettingProfiles = true;
            await GetProfiles();
            isGettingProfiles = false;

        }
    }


    public static void SetCurrentProfile() {
        CheckIfQueueNeedsMoreProfiles();
        if (ProfileQueue.Count > 0) {
            CurrentProfile = ProfileQueue.Dequeue();
        }
    }
    
    public static void GetChatMessages(User fromUser, User toUser) {
        ChatCollection = new ObservableCollection<ChatMessage>();
        Database database = new Database();
        database.SetChatMessages(fromUser, toUser);
        
    }
    
  
    public static string RandomString(int length) {
        
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0) {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(ValidationCharacters[(int)(num % (uint)ValidationCharacters.Length)]);
            }
        }

        return res.ToString();
    }

}