using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Storage;
using System.Drawing;

namespace DataModel;

public class UserController {
    
    private const int RequiredMinimumPasswordLength =  8;
    private const string ValidationCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
    private static int EmailVerificationCodeCharacters = 6;
    
    /// <summary>
    /// Calculates the age of a date
    /// </summary>
    /// <param name="birthDate">The birthday</param>
    /// <returns>Integer of the age</returns>
    public int CalculateAge(DateTime birthDate) {
        int age = DateTime.Now.Year - birthDate.Year;
        if (DateTime.Now.DayOfYear < birthDate.DayOfYear) {
            age--;
        }

        return age;
    }

    /// <summary>
    /// Checks if password is compliant with the requirements
    /// </summary>
    /// <param name="password">Plain string password</param>
    /// <returns>Boolean if password meets requirements</returns>
    public bool CheckPassword(string password)
    {
        if (PasswordLength(password) && PasswordContainsNumber(password) && PasswordContainsCapitalLetter(password))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Checks the password length
    /// </summary>
    /// <param name="password">The password in plain string</param>
    /// <returns>Boolean if password meets length requirement</returns>
    private bool PasswordLength(string password) {

        return password.Length >= RequiredMinimumPasswordLength;
        
    }

    /// <summary>
    /// Checks the password for numbers
    /// </summary>
    /// <param name="password">The password in plain string</param>
    /// <returns>Boolean if password the password contains numbers</returns>
    private bool PasswordContainsNumber(string password)
    {
        return password.Any(char.IsDigit);
    }

    /// <summary>
    /// Checks the password for capital letters
    /// </summary>
    /// <param name="password">The password in plain string</param>
    /// <returns>Boolean if password contains capital letters</returns>
    private bool PasswordContainsCapitalLetter(string password)
    {
        return password.Any(char.IsUpper);
    }

    /// <summary>
    /// Creates a random string with the given length
    /// </summary>
    /// <param name="length">Required length</param>
    /// <returns></returns>
    public string RandomString() {
        
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (EmailVerificationCodeCharacters-- > 0) {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(ValidationCharacters[(int)(num % (uint)ValidationCharacters.Length)]);
            }
        }

        return res.ToString();
    }
    

    /// <summary>
    /// Gets the picker data for the age picker
    /// </summary>
    /// <returns></returns>
    public int[] GetPickerData() {
        int[] leeftijd = new int[82];
        for (int i = 0; i < leeftijd.Length; i++) {
            leeftijd[i] = i + 18;

        }

        return leeftijd;

    }

    public void SetPreference(int minAge, int maxAge, string school) {
        Authentication.CurrentUser.SetMinAge(minAge, Database.ReleaseConnection);
        Authentication.CurrentUser.SetMaxAge(maxAge, Database.ReleaseConnection);
        Authentication.CurrentUser.SetSchool(school, Database.ReleaseConnection);
    }

    public void DeleteAccount() {
        Authentication.CurrentUser.DeleteUser(Database.ReleaseConnection);
        Authentication.CurrentUser = null;
        SecureStorage.Default.Remove("Email");
        SecureStorage.Remove("Email");
        SecureStorage.RemoveAll();
    }

    public void Logout() {
        Authentication.CurrentUser = null;
        SecureStorage.Default.Remove("Email");
        SecureStorage.Remove("Email");
        SecureStorage.RemoveAll();
        
    }
    
    public byte[] ScaleImage(byte[] bytes, int width, int height) {
#if WINDOWS
    using (MemoryStream ms = new MemoryStream(bytes)) {
        using (Bitmap image = new Bitmap(ms)) {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, 0, 0, width, height);
            }

            using (MemoryStream output = new MemoryStream())
            {
                resizedImage.Save(output, image.RawFormat);
                return output.ToArray();
            }
        }
    }
#else
        return bytes;
#endif
    }
    
    /// <summary>
    /// Man of vrouw converter naar integer
    /// </summary>
    /// <returns>Integer of man of vrouw</returns>
    public int GetPreferenceFromUser(string preference) {
        return preference == "Man" ? 1 : 2;
    }

}