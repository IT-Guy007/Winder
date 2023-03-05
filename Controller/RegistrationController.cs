using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class RegistrationController
    {
        private readonly IUserRepository _userRepository;

        public RegistrationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void RegisterUser(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major)
        {
            _userRepository.Registration(firstName,middleName,lastName,email,preference,birthday,gender,bio,password,profilePicture,active,school,major);
        }

    }
}
