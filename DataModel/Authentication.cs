using System.Net;
using System.Net.Mail;

namespace DataModel;
using System.Security.Cryptography;
using System.Text;

public class Authentication
{

    public static User _currentUser { get; set; }
    
    //Match
    public static Queue<Profile> _profileQueue;
    public static Profile _currentProfile;
    public static int selectedImage;
    
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
    public string HashPassword(string password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        }
        return null;
    }

    // Calculating the age by using date as parameter
    public int CalculateAge(DateTime birthDate)
    {
        int age = DateTime.Now.Year - birthDate.Year;
        if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
        {
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
        if (email.EndsWith("@student.windesheim.nl") && email.StartsWith("s"))
        {
            return true;
        }
        return false;
    }

    private bool PasswordLength(string password)
    {
        return password.Length >= 8;
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
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return res.ToString();
    }


    //verstuurd de mail
    public void SendEmail(string email, string body, string subject)
    {
        //zet de client op
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("thewinderapp@gmail.com", "xltbqbsyderpqsxp"),
            EnableSsl = true,

        };

        // maakt de mail aan
        MailMessage mailMessage = new MailMessage
        {
            From = new MailAddress("thewinderapp@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);
        // verstuurd de mail
        smtpClient.Send(mailMessage);
    }
    // changes condition on scaleimage function
    public static bool isscaled = false;
    
}