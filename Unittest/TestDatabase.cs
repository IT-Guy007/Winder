using System.Data.SqlClient;
using DataModel;
using NUnit.Framework;

namespace Unittest;

public class TestDatabase {


    private Database database;
    private Authentication authentication;
    
    [SetUp]
    public void Setup() {
        database = new Database();
        authentication = new Authentication();
        database.UpdateLocalUserFromDatabase("s1165707@student.windesheim.nl");
        Authentication.Initialize();
        Database.Initialize();
    }

    [Test]
    public void TestDatabaseConnection() {
        try {
            Database.OpenConnection();
            Assert.Pass();

            Database.CloseConnection();
        } catch (SqlException e) {
            Assert.Fail(e.Message);
        }

    }

    [Test]
    public void DatabaseGetEmailForDatabase() {
        Database database = new Database();
        Assert.IsNotEmpty(database.GetEmailFromDataBase());
    }

    [TestCase("Jeroen", "1234", ExpectedResult = false)]
    [TestCase("s1165707@student.windesheim.nl", "Qwerty1@", ExpectedResult = true)]
    public bool LoginTest(string email, string password) {
        return database.CheckLogin(email, password);

    }


    [TestCase("Test", "", "Account", "male",
        "01-01-2000", "male", "Ik haat men leven want ik haal netwerken nooit", "Kaolozooi_ik_haal_netwerkenniet01@", "", false, "Zwolle", "opleiding",
        ExpectedResult = true)]

    public bool RegisterTest(string firstname, string middlename, string lastname,
        string preference, DateTime birthday, string gender, string bio, string password, string profilePicture, bool active, string locatie, string opleiding)
    {
        Random random = new Random();
        var email1 = random.Next(0, 999999);
        string email2 = "s" + email1 + "@student.windesheim.nl";
        try {
            database.RegistrationFunction(firstname, middlename, lastname, email2, preference, birthday, gender, bio,
                password, new byte[] { 0x20, 0x20 }, active, locatie, opleiding);
            return true;
        } catch
        {
            return false;
        }
    }

    [TestCase("s1165707@student.windesheim.nl", false, ExpectedResult = true)]
    [TestCase("1707@student.windesheim.nl", false, ExpectedResult = false)]
    [TestCase("s1165707@student.windesheim.nl", true, ExpectedResult = true)]
    public bool ToggleActivationTest(string email, bool activation) {
        return database.ToggleActivation(email, activation);
    }


    [TestCase("jannieandes@gmail.com", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com", ExpectedResult = true)]
    public bool DatabaseDeleteUser(string email) {
       database.DeleteUser(email);
       return authentication.EmailIsUnique(email); 
    }

    public bool UpdateUserInDatabaseWithNewUserProfileTest(string firstname, string middlename, string lastname,
    string preference, DateTime birthday, string gender, string bio, string email,string major, byte[] profilePicture)
    {
        
        User testUser = new User();
        testUser.firstName = firstname;
        testUser.middleName = middlename;
        testUser.lastName = lastname;
        testUser.preference = preference;
        testUser.birthDay = birthday;
        testUser.gender = gender;
        testUser.bio = bio;
        testUser.email = email;
        testUser.major = major;
        testUser.profilePicture = profilePicture;
        return database.UpdateUserInDatabaseWithNewUserData(testUser);
    }

    
    [TestCase("@student.windesheim.nl", ExpectedResult = false)]
    [TestCase("s1test@student.windesheim.nl", ExpectedResult = true)]
    public bool LoadInterestsFromDatabaseInListInteressesTest(string email)
    {
        return Database.LoadInterestsFromDatabaseInListInteresses(email).Count > 0;
    }

    [TestCase("s1416890@student.windesheim.nl", "Lezen", ExpectedResult = true)]
    [TestCase("s1416890@student.windesheim.nl", "bestaat niet", ExpectedResult = false)]
    public bool RemoveInterestOutOfuserHasInterestTableDatabaseTest(string email, string interest) {
        return database.RemoveInterestOfUser(email, interest);
    }

    
    [TestCase("s1416890@student.windesheim.nl", "Roeien", ExpectedResult = false)]
    [TestCase("s00@student.windesheim.nl", "Astrologie", ExpectedResult = true)]
    [TestCase("s1416890@student.windesheim.nl", "Sportschool", ExpectedResult = false)]
    [TestCase("NietBestaandeGebruiker@student.windesheim.nl", "Lezen", ExpectedResult = false)]
    public bool AddInterestToUserInterestsTest(string email, string interest)
    {
        
        bool boolean = database.RegisterInterestInDatabase(email, interest);
        database.RemoveInterestOfUser(email, interest);
        return boolean;
    }
    [TestCase(ExpectedResult = true)]
    public bool GetInterestsFromDataBaseTest()
    {
        return database.GetInterestsFromDataBase().Count > 0;
    }
    [TestCase("s1173231@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com",  ExpectedResult = false)]
    [TestCase(" ", ExpectedResult = false)]
    public bool DatabaseGetUsersWhoLikedYou(string email) {

        var data = Database.GetUsersWhoLikedYou(email);
        return data.Length > 0;
       
    }
    
    [TestCase("jannieandes@gmail.com", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com", ExpectedResult = false)]
    [TestCase(" ", ExpectedResult = false)]
    public bool GetUsersWithCommonInterest(string email) {
        try {
        database.GetUsersWithCommonInterest(email);
            return true;
        } catch {
            return false;
        }
       
    }
    

    [TestCase("s1165707@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165700@student.windesheim.nl", ExpectedResult = false)]
    public bool UpdateLocalUserFromDatabase(string email) {
        database.UpdateLocalUserFromDatabase(email);
        
        if(Authentication._currentUser.email == email) {
            return true;
        }
        return false;
        
    }
    
    [Test]
    public void GetEmailFromDataBase() {
        
        Assert.IsNotEmpty(database.GetEmailFromDataBase());
    }

    [TestCase("s1173231@student.windesheim.nl","Qwerty2@", ExpectedResult = true)]
    [TestCase("s1173231@student.windesheim.nl","Qwerty1@", ExpectedResult = true)]
    public bool UpdatePassword(string email, string password) {
        try {
            database.UpdatePassword(email, password);
            return true;
        } catch(Exception e) {
            Console.WriteLine(e);
            Console.WriteLine(e.StackTrace);
            return false;
        }
    }
    
    [TestCase("s1111111@student.windesheim.nl",ExpectedResult = true)]
    public bool DeleteUser(string email) {
        try {
            database.DeleteUser(email);
            return true;
        } catch {
            return false;
        }
    }
    
    [Test]
    public void GetInterestFromDatabase() {
        Assert.IsNotEmpty(database.GetInterestsFromDataBase());
    }
    
    [TestCase("s1165707@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1164000@student.windesheim.nl", ExpectedResult = true)]
    public bool GetUserFromDatabase(string email) {
        try {
            Database.GetUserFromDatabase(email);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl","Bier drinken", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","Duikenn", ExpectedResult = false)]
    public bool RegisterInterestInDatabase(string email, string interest) {
        try {
            database.RegisterInterestInDatabase(email, interest);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl","Bier drinken", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","zuipert", ExpectedResult = true)]
    public bool RemoveInterestOfUser(string email, string interest) {
        try {
            database.RemoveInterestOfUser(email, interest);
            return true;
        } catch {
            return false;
        }
    }
    
  
    
    [TestCase("s1165707@student.windesheim.nl","Female", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","Male", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","Else", ExpectedResult = true)]
    public bool SetPreference(string email, string preference) {
        try {
            database.InsertPreference(email, preference);
            return true;
        } catch {
            return false;
        }
    }

    [TestCase("s1165707@student.windesheim.nl","Almere", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","Zwolle", ExpectedResult = true)]
    public bool SetLocation(string email, string location) {
        try {
            database.InsertLocation(email, location);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetPreference(string email) {
        try {
            var preferences = database.GetPreference(email);
            if (preferences != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }

    [TestCase("s1165707@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl", ExpectedResult = false)]
    public bool GetLocation(string email) {

        var location = database.GetLocation(email);
        return location.Length > 0;
        
    }

    [TestCase("s1165707@student.windesheim.nl",29,ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",18,ExpectedResult = true)]
    public bool SetAge(string email, int age) {
        try {
            database.SetMinAge(email, age);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165400@student.windesheim.nl", 99,ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",60,ExpectedResult = true)]
    public bool SetMaxAge(string email, int age) {
        try {
            database.SetMaxAge(email, age);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetMinAge(string email) {
        try {
            var minAge = database.GetMinAge(email);
            if (minAge != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetMaxAge(string email) {
        try {
            var maxAge = database.GetMaxAge(email);
            if (maxAge != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s168742@student.windesheim.nl", "s1167487@student.windesheim.nl", ExpectedResult = false)]
    [TestCase("s1test@student.windesheim.nl", "s1test@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","s1168742@student.windesheim.nl", ExpectedResult = false)]
    public bool CheckMatch(string email, string email2) {
        try {
            return database.CheckMatch(email, email2);
        } catch {
            return false;
        }
    }
    
    [TestCase("s168742@student.windesheim.nl", "s1167487@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","s1165400@student.windesheim.nl", ExpectedResult = false)]
    public bool NewLike(string email, string email2) {
        try {
            database.NewLike(email, email2);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl","s168742@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","s1165400@student.windesheim.nl", ExpectedResult = false)]
    public bool NewDislike(string email, string email2) {
        try {
            database.NewDislike(email, email2);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetPictureFromDatabase(string email) {
        try {
            var picture = Database.GetPicturesFromDatabase(email);
            if (picture != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }

    [TestCase("s1173231@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool AlgorithmForSwiping(string email) {
        var users = Database.AlgorithmForSwiping(email);
        return users.Count > 0;
    } 

    
    [TestCase("s1test@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool Get5Profiles(string email) {
        var users = Authentication.Get5Profiles(email);
        return users.Length > 0;

    }
    
    [TestCase("s00@student.windesheim.nl", "s1@student.windesheim.nl", "Vahe Test", ExpectedResult = true)]
    public bool SendMessageTest(string personFrom, string personTo, string message) {
        try {
            return Database.SendMessage(personFrom, personTo, message);
        } catch {
            return false;
        }
    }




}   

