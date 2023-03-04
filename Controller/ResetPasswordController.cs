using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class ResetPasswordController
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

    }
}
