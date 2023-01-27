using System.Collections.ObjectModel;
using Microsoft.Maui.Storage;

namespace DataModel;
using System.Security.Cryptography;
using System.Text;

public class Authentication {
    
    private const string ValidationCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";

    public static User CurrentUser { get; set; }
    private const int passwordLength =  8;

    public static void Initialize() {
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