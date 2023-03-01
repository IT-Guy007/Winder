﻿using DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winder.Repositories.Interfaces
{
    internal interface IUserRepository
    {
        public User Registration(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major);
        public User CheckLogin(string email, string password);
        public bool UpdateUserData(string firstName, string middleName, string lastName, string email, string preference, DateTime birthday, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major);
        public User GetUserFromDatabase(string email);
        public bool DeleteUser(string email);
        public bool IsEmailUnique(string email);
        public bool SetInterest(string email);
        public List<string> GetConditionBasedUsers(User user);
        }
    }