using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mail;

namespace DataModel;
using System.Security.Cryptography;
using System.Text;

public class Authentication {

    public static User _currentUser { get; set; }
    private const string winderEmail = "thewinderapp@gmail.com";
    private const string emailCredential = "xltbqbsyderpqsxp";
    private const string validationCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
    private const string emailEndsWith = "@student.windesheim.nl";
    private const string emailStartsWith = "s";
    private const int passwordLength =  8;
    private const string smtpClientGmail = "smtp.gmail.com";
    private const int portEmail = 587;
    public static bool isScaled = false;

    //Match
    public static Queue<Profile> _profileQueue;
    public static Profile _currentProfile;
    public static int selectedImage;
    private static bool isGettingProfiles;
    
    //Chat
    public static ObservableCollection<ChatMessage> ChatCollection;

    public static void Initialize() {
        _profileQueue = new Queue<Profile>();
        selectedImage = 0;
        _currentUser = new User();
        
    }


    // checking if email is already in database, returns true if unique
    public bool EmailIsUnique(string email)
    {
        Database db = new Database();
        List<string> emails = db.GetEmailFromDataBase();
        if (emails.Contains(email))
        {
            return false;
        }
        return true;
    }

    // Hashing the password
    public string HashPassword(string password) {
        if (!string.IsNullOrEmpty(password)) {
            String result = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
            Console.WriteLine(result);
            return result;
        }
        return "";
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

    // checks if email belongs to Windesheim, returns true if so
    public bool CheckEmail(string email)
    {
        if (email.EndsWith(emailEndsWith) && email.StartsWith(emailStartsWith))
        {
            return true;
        }
        return false;
    }

    private bool PasswordLength(string password)
    {
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


    //maakt de authenticatiecode aan
    public static string RandomString(int length)
    {
        
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(validationCharacters[(int)(num % (uint)validationCharacters.Length)]);
            }
        }

        return res.ToString();
    }


    //verstuurd de mail
    public void SendEmail(string email, string body, string subject)
    {
        //zet de client op
        SmtpClient smtpClient = new SmtpClient(smtpClientGmail)
        {
            Port = portEmail,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(winderEmail, emailCredential),
            EnableSsl = true,

        };

        // maakt de mail aan
        MailMessage mailMessage = new MailMessage
        {
            From = new MailAddress(winderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);
        // verstuurd de mail
        smtpClient.Send(mailMessage);
    }

    //User to get the profiles for the match(run async)
    public static Profile[] Get5Profiles(string email) {
        

        //The users(email) to get
        List<string> usersToRetrief = new List<string>();

        usersToRetrief = Database.AlgorithmForSwiping(email);

        //Results
        Profile[] profiles = new Profile[usersToRetrief.Count()];

        //Retrieving
        for (int i = 0; i < usersToRetrief.Count(); i++) {

            //Get the user
            User user = Database.GetUserFromDatabase(usersToRetrief[i]);

            //Get the interests of the user
            user.interests = Database.LoadInterestsFromDatabaseInListInteresses(usersToRetrief[i]).ToArray();

            //Get the images of the user
            byte[][] images = Database.GetPicturesFromDatabase(usersToRetrief[i]);
            var profile = new Profile(user, images);

            profiles[i] = profile;
        }

        return profiles;
    }

    public static async Task GetProfiles() {
        Profile[] profiles = Get5Profiles(_currentUser.email);
        foreach (var profile in profiles) {

            if (profile != null) {
                _profileQueue.Enqueue(profile);
            }
        }
    }

    public static async void CheckIfQueueNeedsMoreProfiles()
    {
        if (_profileQueue.Count < 5 && !isGettingProfiles)
        {
            isGettingProfiles = true;
            await GetProfiles();
            isGettingProfiles = false;

        }
    }


    public static void SetCurrentProfile() {
        CheckIfQueueNeedsMoreProfiles();
        if (_profileQueue.Count > 0) {
            _currentProfile = _profileQueue.Dequeue();
        }
    }
    
    public static void GetChatMessages(User fromUser, User toUser) {
        ChatCollection = new ObservableCollection<ChatMessage>();
        Database database = new Database();
        database.SetChatMessages(fromUser, toUser);
        
    }
}