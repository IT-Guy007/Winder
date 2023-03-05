using DataModel;
using Winder.Repositories.Interfaces;


namespace Controller
{
    public class ChatController
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IUserRepository _userRepository;

        public ChatController(IChatMessageRepository chatMessageRepository, IMatchRepository matchRepository, IUserRepository userRepository)
        {
            _chatMessageRepository = chatMessageRepository;
            _matchRepository = matchRepository;
            _userRepository = userRepository;
        }

        public void SendMessage(string message, string emailTo, string emailFrom)
        {
            _chatMessageRepository.SendMessage(message, emailTo, emailFrom);
        }

        public List<ChatMessage> GetChatMessages(string emailTo, string emailFrom)
        {
            return _chatMessageRepository.GetChatMessages(emailTo, emailFrom);
        }

        public void SetRead(string emailTo, string emailFrom) {
            _chatMessageRepository.SetRead(emailTo, emailFrom);
        }

        public List<Match> GetMatches(string email)
        {
            User GottenUser = _userRepository.GetUserFromDatabase(email);

            return _matchRepository.GetMatchedStudentsFromUser(GottenUser);
        }
    }
}
