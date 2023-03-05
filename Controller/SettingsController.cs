using DataModel;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class SettingsController
    {
        private readonly IUserRepository _userRepository;
        private ValidationController validationController;


        public SettingsController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            validationController = new ValidationController(_userRepository);
        }

        public void UpdatePassword(string email, string password)
        {
            // checken of Email in de database staat
            if (validationController.EmailIsUnique(email) == false)
            {
                string hashedPassword = validationController.HashPassword(password);
                _userRepository.UpdatePassword(email, hashedPassword);

            }

        }

        public string GetSchool(string email)
        {
            return _userRepository.GetSchool(email);
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
        /// Man of vrouw converter naar integer
        /// </summary>
        /// <returns>Integer of man of vrouw</returns>
        public int GetPreferenceFromUser(string preference)
        {
            return preference == "Man" ? 1 : 2;
        }



        /// <summary>
        /// Adds all interests to users list of interests
        /// </summary>
        /// <param name="interests"></param>
        public void RegisterInterestsInDatabase(string email, List<string> interests)
        {
            foreach (var interest in interests)
            {
                _userRepository.SetInterest(email, interest);
            }
        }

        public void DeleteAccount()
        {
            
            _userRepository.DeleteUser(User.CurrentUser.Email);
            User.CurrentUser = null;
          
            SecureStorage.Remove("Email");
            
        }

        public void Logout()
        {
            User.CurrentUser = null;
           
            SecureStorage.Remove("Email");
        

        }

        public void SetPreference(int minAge, int maxAge, string school)
        {
            _userRepository.SetMinAge(minAge, User.CurrentUser.Email);
            _userRepository.SetMaxAge(maxAge, User.CurrentUser.Email);
            _userRepository.SetSchool(school, User.CurrentUser.Email);
           
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

    }
}
