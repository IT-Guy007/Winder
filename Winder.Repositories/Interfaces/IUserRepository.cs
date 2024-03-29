﻿using DataModel;

namespace Winder.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public User Registration(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major);
        public User CheckLogin(string email, string password);
        public bool UpdateUserData(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, byte[] profilePicture, string major);
        public User GetUserFromDatabase(string email);
        public bool DeleteUser(string email);
        public bool IsEmailUnique(string email);
        public bool SetInterest(string email, string interest);
        public bool DeleteInterest(string email, string interest);
        public List<string> GetConditionBasedUsers(User user);
        public List<string> GetInterestsFromUser(string email);
        public bool UpdatePassword(string email, string password);
        public string GetSchool(string email);
        public bool SetMinAge(int minAge, string email);
        public bool SetSchool(string school, string email);
        public bool SetMaxAge(int maxAge, string email);
    }
}
