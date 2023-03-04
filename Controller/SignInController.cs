using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class SignInController
    {
        private readonly IUserRepository _userRepository;

        public SignInController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

    }
}
