using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class ValidationController
    {
        private readonly string EmailStartsWith = "s";
        private readonly string EmailEndsWith = "@student.windesheim.nl";

        private const int RequiredMinimumPasswordLength = 8;

        private const string ValidationCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
        private static int EmailVerificationCodeCharacters = 6;

        private readonly IUserRepository _userRepository;

        public ValidationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool EmailIsUnique(string email)
        {
            return _userRepository.IsEmailUnique(email);
        }

        /// <summary>
        /// Checks if the email is from Windesheim student
        /// </summary>
        /// <param name="email">The given email</param>
        /// <returns></returns>
        public bool CheckEmail(string email)
        {
            return email.EndsWith(EmailEndsWith) && email.StartsWith(EmailStartsWith);
        }

        public User CheckLogin(string email, string password)
        {
            Console.WriteLine("Check login");
            string hashed = HashPassword(password);

            if (!email.EndsWith(EmailEndsWith))
            {
                email = email + EmailEndsWith;
            }

            if (!email.StartsWith(EmailStartsWith))
            {
                email = EmailStartsWith + email;
            }

            User user = _userRepository.CheckLogin(email, hashed);

            new SettingsController(_userRepository).SetLoginEmail(email);

            return user;
        }

        public byte[] ScaleImage(byte[] bytes, int width, int height)
        {
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
        private bool PasswordLength(string password)
        {

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
        public string RandomString()
        {

            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (EmailVerificationCodeCharacters-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(ValidationCharacters[(int)(num % (uint)ValidationCharacters.Length)]);
                }
            }

            return res.ToString();
        }

        /// <summary>
        /// Calculates the age of a date
        /// </summary>
        /// <param name="birthDate">The birthday</param>
        /// <returns>Integer of the age</returns>
        public int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Hashed a plain string to a hashed string
        /// </summary>
        /// <param name="password">The plain string to hash</param>
        /// <returns>The hashed string</returns>
        public string HashPassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                string result = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
                return result;
            }
            return "";
        }
    }
}
