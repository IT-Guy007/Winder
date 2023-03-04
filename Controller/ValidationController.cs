using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
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


    }
}
