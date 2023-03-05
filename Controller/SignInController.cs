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
