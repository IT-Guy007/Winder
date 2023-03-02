using DataModel;
using Microsoft.Extensions.Configuration;
using Winder.Repositories.Interfaces;
namespace Winder.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public User CheckLogin(string email, string password)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string email)
        {
            throw new NotImplementedException();
        }

        public List<string> GetConditionBasedUsers(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUserFromDatabase(string email)
        {
            throw new NotImplementedException();
        }

        public bool IsEmailUnique(string email)
        {
            throw new NotImplementedException();
        }

        public User Registration(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major)
        {
            throw new NotImplementedException();
        }

        public bool SetInterest(string email)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserData(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major)
        {
            throw new NotImplementedException();
        }
    }
}