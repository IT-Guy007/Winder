using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using DataModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Storage;
using Winder.Repositories;
using Winder.Repositories.Interfaces;

namespace Controller
{

    public class UserController
    {
        private readonly IUserRepository _userRepository;

        private readonly string EmailStartsWith = "s";
        private readonly string EmailEndsWith = "@student.windesheim.nl";

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Adds all interests to users list of interests
        /// </summary>
        /// <param name="interests"></param>
        public void RegisterInterestsInDatabase(List<string> interests)
        {
            foreach (var interest in interests)
            {
                Authentication.CurrentUser.SetInterestInDatabase(interest, Database.ReleaseConnection);
            }
        }


        /// <summary>
        /// Gets the picker data for the age picker
        /// </summary>
        /// <returns></returns>
        public int[] GetPickerData()
        {
            int[] leeftijd = new int[82];
            for (int i = 0; i < leeftijd.Length; i++)
            {
                leeftijd[i] = i + 18;

            }

            return leeftijd;

        }

        public void SetPreference(int minAge, int maxAge, string school)
        {
            Authentication.CurrentUser.SetMinAge(minAge, Database.ReleaseConnection);
            Authentication.CurrentUser.SetMaxAge(maxAge, Database.ReleaseConnection);
            Authentication.CurrentUser.SetSchool(school, Database.ReleaseConnection);
        }

        public void DeleteAccount()
        {
            //Authentication.CurrentUser.DeleteUser(Database.ReleaseConnection);
            Authentication.CurrentUser = null;
            SecureStorage.Default.Remove("Email");
            SecureStorage.Remove("Email");
            SecureStorage.RemoveAll();
        }

        public void Logout()
        {
            Authentication.CurrentUser = null;
            SecureStorage.Default.Remove("Email");
            SecureStorage.Remove("Email");
            SecureStorage.RemoveAll();

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
        /// Man of vrouw converter naar integer
        /// </summary>
        /// <returns>Integer of man of vrouw</returns>
        public int GetPreferenceFromUser(string preference)
        {
            return preference == "Man" ? 1 : 2;
        }


        /// <summary>
        /// Sets the login email in the secure storage
        /// </summary>
        /// <param name="email">The email to set</param>
        public async Task SetLoginEmail(string email)
        {
            Console.WriteLine("Setting login Email");
            await SecureStorage.SetAsync("Email", email);

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
            string hashed = new User().HashPassword(password);

            if (!email.EndsWith(EmailEndsWith))
            {
                email = email + EmailEndsWith;
            }

            if (!email.StartsWith(EmailStartsWith))
            {
                email = EmailStartsWith + email;
            }

            User user = _userRepository.CheckLogin(email, hashed);

            SetLoginEmail(email);

            return user;
        }

        public void UpdatePassword(string email, string password)
        {
            // checken of Email in de database staat
            if (EmailIsUnique(email) == false)
            {
                string hashedPassword = new User().HashPassword(password);
                _userRepository.UpdatePassword(email, hashedPassword);

            }

        }

        public bool EmailIsUnique(string email)
        {
            return _userRepository.IsEmailUnique(email);
        }
    }
}