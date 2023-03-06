using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Winder.Repositories;
using DataModel;
namespace Unittest.Repositories;

public class UserTest
{
    private UserRepository _userRepository;

    [SetUp]
    public void Setup()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _userRepository = new UserRepository(configuration);
    }

    [TestCase("s1000@student.windesheim.nl", "hoi", ExpectedResult = false)]
    [TestCase("s1000@student.windesheim.nl", "Qwerty1@", ExpectedResult = true)]
    [TestCase("", "hallo", ExpectedResult = false)]
    public bool CheckLoginTest(string email, string password)
    {
        if (_userRepository.CheckLogin(email, password) != null)
        {
            return true;
        }
        return false;
    }

    [TestCase("s2000@student.windesheim.nl", ExpectedResult = true)] // in database
    [TestCase("s30000@student.windesheim.nl", ExpectedResult = false)] // not in database
    [TestCase("ja", ExpectedResult = false)]
    public bool DeleteUserTest(string email)
    {
        _userRepository.Registration("Testdel", "", "Test", "s2000@student.windesheim.nl", "Man", DateTime.Now, "Vrouw", "a", "a", new byte[0], true, "a", "a");

        bool b = _userRepository.DeleteUser(email);
        _userRepository.DeleteUser("s2000@student.windesheim.nl");
        return b;
    }


    [TestCase("s1000@student.windesheim.nl", ExpectedResult = "s1001@student.windesheim.nl")]
    [TestCase("s1001@student.windesheim.nl", ExpectedResult = "s1000@student.windesheim.nl")]
    [TestCase("", ExpectedResult = null)]
    public string GetConditionBasedUsersTest(string email)
    {
        User currentUser = _userRepository.GetUserFromDatabase(email);
        
        List<string> users = _userRepository.GetConditionBasedUsers(currentUser);

        if (users.Count > 0)
        {
            return users[0];
        }
        return null;
        
    }




    [TestCase("s1000@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("sstaatnietindatabase@student.windesheim.nl", ExpectedResult = false)]
    [TestCase("", ExpectedResult = false)]
    public bool GetUserFromDatabaseTest(string email)
    {
        if ((_userRepository.GetUserFromDatabase(email).FirstName ?? "a") != "a")
        {
            return true;
        }
        return false;
    }



    [TestCase("s1000@student.windesheim.nl", ExpectedResult = false)]
    [TestCase("sstaatnietindatabase@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("ja", ExpectedResult = true)]
    public bool IsEmailUniqueTest(string email)
    {
        return _userRepository.IsEmailUnique(email);
    }



    [TestCase("Testreg", "", "Test", "s90000@student.windesheim.nl", "Man", "Vrouw", "a", "a", new byte[0], true, "a", "a", ExpectedResult = true)]
    public bool Registration(string firstName, string middleName, string lastName, string email, string preference, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major)
    {
        _userRepository.Registration(firstName, middleName, lastName, email, preference, new DateTime(1925, 01, 01, 0, 0, 0, 0), gender, bio, password, profilePicture, active, school, major);

        if (_userRepository.IsEmailUnique(email))
        {
            return false;
        }
        else
        {
            _userRepository.DeleteUser(email);
            return true;
        }
    }


    [TestCase("s1000@student.windesheim.nl", "Astrologie", ExpectedResult = true)]
    [TestCase("s1000@student.windesheim.nl", "Lezen", ExpectedResult = true)]
    [TestCase("s1000@student.windesheim.nl", "eadefasdf", ExpectedResult = false)]
    [TestCase("sstaatnietindatabase@student.windesheim.nl", "eadefasdf", ExpectedResult = false)]
    public bool SetInterestTest(string email, string interest)
    {
        var result = _userRepository.SetInterest(email, interest);
        return result;
    }
    [TestCase("Testup", "", "Testa", "s1000@student.windesheim.nl", "Man", "Vrouw", "ab", "ab", new byte[0], false, "ab", "ab", ExpectedResult = true)]
    [TestCase("Testup", "", "Testa", "sstaatnietindatabase@student.windesheim.nl", "Man", "Vrouw", "ab", "ab", new byte[0], false, "ab", "ab", ExpectedResult = false)]
    public bool UpdateUserDataTest(string firstName, string middleName, string lastName, string email, string preference, string gender, string bio, string password, byte[] profilePicture, bool active, string school, string major)
    {
        return _userRepository.UpdateUserData(firstName, middleName, lastName, email, preference, new DateTime(1925, 01, 01, 0, 0, 0, 0), gender, bio, profilePicture, major);
    }



    [TestCase("s1000@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("staatnietindatabase@student.windesheim.nl", ExpectedResult = false)]
    [TestCase("", ExpectedResult = false)]
    public bool GetInterestsFromUserTest(string email)
    {
        List<string> interests = _userRepository.GetInterestsFromUser(email);
        if (interests.Count > 1)
        {
            return true;
        }
        return false;
    }

    [TestCase("s1000@student.windesheim.nl", ExpectedResult = "Zwolle")]
    [TestCase("s1001@student.windesheim.nl", ExpectedResult = "Zwolle")]
    public string GetSchoolTest(string email)
    {
        return _userRepository.GetSchool(email);
    }
    [TestCase("s1000@student.windesheim.nl", "Lezen", ExpectedResult = true)]
    public bool DeleteInterestTest(string email, string interest)
    {
        return _userRepository.DeleteInterest(email, interest);
    }
    [TestCase("s1000@student.windesheim.nl", "Qwerty12@@@@", ExpectedResult = true)]
    [TestCase("s1000@student.windesheim.nl", "Qwerty1@", ExpectedResult = true)]
    public bool UpdatePasswordTest(string email, string password)
    {
        return _userRepository.UpdatePassword(email, password);
    }
    [TestCase(20, "s1000@student.windesheim.nl", ExpectedResult = true)]
    public bool SetMinAgeTest(int minAge, string email)
    {
        return _userRepository.SetMinAge(minAge, email);
    }
    [TestCase(25, "s1000@student.windesheim.nl", ExpectedResult = true)]
    public bool SetMaxAgeTest(int maxAge, string email)
    {
        return _userRepository.SetMinAge(maxAge, email);
    }
    [TestCase("Almere", "s1000@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("Zwolle", "s1000@student.windesheim.nl", ExpectedResult = true)]
    public bool SetSchoolTest(string school, string email)
    {
        return _userRepository.SetSchool(school, email);
    }
    
}