using System.Data.SqlClient;

namespace Unit_test;
using DataModel;
public class TestDatabase {

    
    private Authentication _authentication { get; set; }
    private Database _database { get; set; }
    private DateTime today { get; set; }
    
    [SetUp]
    public void Setup() {
        _authentication = new Authentication();
        _database = new Database();
        today = new DateTime();
        

    }
    
    [Test]
    public void TestCreateDatabaseConnection() {
        try {
            Database.GenerateConnection();
            Assert.Pass();
        } catch (SqlException e) {
            Assert.Fail(e.Message);
        }

    }

    [Test]
    public void TestDatabaseConnection() {
        
        try {
            Database.OpenConnection();
            Assert.Pass();

            Database.CloseConnection();
        } catch(SqlException e) {
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
        return _database.CheckLogin(email, password);

    }
    
    [TestCase("Test", "", "Account", "male",
        "01-01-2000", "male", "Ik haat men leven want ik haal netwerken nooit", "Kaolozooi_ik_haal_netwerkenniet01@","",false, "Zwolle", "opleiding",
        ExpectedResult = true)]

    public bool RegisterTest(string firstname, string middlename, string lastname,
        string preference, DateTime birthday, string gender, string bio, string password,string profilePicture, bool active, string locatie, string opleiding)
    {
        Random random = new Random();
        var email1 = random.Next(0, 999999);
        string email2 = "s" + email1 + "@student.windesheim.nl";
        try {
            _database.RegistrationFunction(firstname, middlename, lastname, email2, preference, birthday, gender, bio,
                password, new byte[] { 0x20, 0x20 }, active, locatie, opleiding);
            return true;
        } catch
        {
            return false;
        }
    }

    [TestCase("s1165707@student.windesheim.nl",false, ExpectedResult = true)]
    [TestCase("1707@student.windesheim.nl",false, ExpectedResult = false)]
    [TestCase("s1165707@student.windesheim.nl",true, ExpectedResult = true)]
    public bool ToggleActivationTest(string email, bool activation) {
        return _database.ToggleActivation(email, activation);
    }


    [TestCase("jannieandes@gmail.com", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com", ExpectedResult = false)]
    public bool DatabaseDeleteUser(string email)
    {
        try
        {
            
            _database.DeleteUser(email);
            return true;

        }
        catch
        {
            return false;
        }
    }

    [TestCase("jannieandes@gmail.com", "asdf", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com", "asedf",  ExpectedResult = false)]
    public bool DatabaseUpdatePassword(string email, string password)
    {
        try
        {
            _database.UpdatePassword(email, password);
            return true;
        }
        catch
        {
            return false;
        }
    }
    [TestCase("Peter", "van", "Huizkes", "Vrouw", "1998/01/01", "Man", "bio info", "s1416890@student.windesheim.nl", ExpectedResult = true)]
    public bool updateUserInDatabaseWithNewUserProfileTest(string firstname, string middlename, string lastname,
    string preference, DateTime birthday, string gender, string bio, string email)
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
        return _database.UpdateUserInDatabaseWithNewUserData(testUser);
    }

    [TestCase("@student.windesheim.nl", ExpectedResult = false)]
    [TestCase("s1416890@student.windesheim.nl", ExpectedResult = true)]
    public bool LoadInterestsFromDatabaseInListInteressesTest(string email)
    {
        return Database.LoadInterestsFromDatabaseInListInteresses(email).Count > 0;
    }

    [TestCase("s1416890@student.windesheim.nl", "Lezen", ExpectedResult = true)]
    [TestCase("s1416890@student.windesheim.nl", "bestaat niet", ExpectedResult = false)]
    public bool removeInterestOutOfuserHasInterestTableDatabaseTest(string email, string interest) {
        return _database.RemoveInterestOfUser(email, interest);
    }

    [TestCase("s1416890@student.windesheim.nl", "Roeien", ExpectedResult = false)]
    [TestCase("s1416890@student.windesheim.nl", "Astrologie", ExpectedResult = true)]
    [TestCase("s1416890@student.windesheim.nl", "Sportschool", ExpectedResult = false)]
    [TestCase("NietBestaandeGebruiker@student.windesheim.nl", "Lezen", ExpectedResult = false)]
    public bool addInterestToUserInterestsTest(string email, string interest)
    {
        return _database.RegisterInterestInDatabase(email, interest);
    }
    [TestCase(ExpectedResult = true)]
    public bool GetInterestsFromDataBaseTest()
    {
        return _database.GetInterestsFromDataBase().Count > 0;
    }
    [TestCase("jannieandes@gmail.com", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com",  ExpectedResult = false)]
    [TestCase(" ", ExpectedResult = false)]
    public bool DatabaseGetUsersWhoLikedYou(string email)
    {
        try
        {
            Database.GetUsersWhoLikedYou(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    [TestCase("jannieandes@gmail.com", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com", ExpectedResult = false)]
    [TestCase(" ", ExpectedResult = false)]
    public bool GetUsersWithCommonInterest(string email)
    {
        try
        {
            _database.GetUsersWithCommonInterest(email);
            return true;
        }
        catch
        {
            return false;
        }
       
    }

    [TestCase("jannieandes@gmail.com", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com", ExpectedResult = false)]
    [TestCase(" ", ExpectedResult = false)]
    public bool DatabaseGetRestOfUsers(string email)
    {
        try
        {
            _database.GetRestOfUsers(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    [TestCase("jannieandes@gmail.com", ExpectedResult = true)]
    [TestCase("japiejaap@gmail.com", ExpectedResult = false)]
    [TestCase(" ", ExpectedResult = false)]
    public bool DatabaseAlgorithmForSwiping(string email) {
        try {
            Database.AlgorithmForSwiping(email);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165700@student.windesheim.nl", ExpectedResult = false)]
    public bool updateLocalUserFromDatabase(string email) {
        try {
            _database.UpdateLocalUserFromDatabase(email);
            return true;
        } catch {
            return false;
        }
    }
    
    [Test]
    public void GetEmailFromDataBase() {
        
        Assert.IsNotEmpty(_database.GetEmailFromDataBase());
    }
    
    
    
    [TestCase("s1165707@student.windesheim.nl",false, ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl",false, ExpectedResult = false)]
    [TestCase("s1165707@student.windesheim.nl",true, ExpectedResult = true)]
    
    public bool ToggleActivation(string email, bool activation) {
        try {
            _database.ToggleActivation(email, activation);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl","Qwerty2@", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","Qwerty1@", ExpectedResult = true)]
    public bool UpdatePassword(string email, string password) {
        try {
            _database.UpdatePassword(email, password);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1111111@student.windesheim.nl",ExpectedResult = true)]
    public bool DeleteUser(string email) {
        try {
            _database.DeleteUser(email);
            return true;
        } catch {
            return false;
        }
    }
    
    [Test]
    public void GetInterestFromDatabase() {
        Assert.IsNotEmpty(_database.GetInterestsFromDataBase());
    }
    
    [TestCase("s1165707@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1164000@student.windesheim.nl", ExpectedResult = true)]
    public bool GetUserFromDatabase(string email) {
        try {
            _database.GetUserFromDatabase(email);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl","Bier drinken", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","Duikenn", ExpectedResult = false)]
    public bool RegisterInterestInDatabase(string email, string interest) {
        try {
            _database.RegisterInterestInDatabase(email, interest);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl","Bier drinken", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","zuipert", ExpectedResult = true)]
    public bool RemoveInterestOfUser(string email, string interest) {
        try {
            _database.RemoveInterestOfUser(email, interest);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s116400@student.windesheim.nl", ExpectedResult = false)]
    public bool LoadInterestfromdatabase(string email) {
        try {
            Database.LoadInterestsFromDatabaseInListInteresses(email);
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
            _database.InsertPreference(email, preference);
            return true;
        } catch {
            return false;
        }
    }

    [TestCase("s1165707@student.windesheim.nl","Almere", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","Zwolle", ExpectedResult = true)]
    public bool SetLocation(string email, string location) {
        try {
            _database.InsertLocation(email, location);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetPreference(string email) {
        try {
            var preferences = _database.GetPreference(email);
            if (preferences != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetLocation(string email) {
        try {
            var location = _database.GetLocation(email);
            if (location != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s1165707@student.windesheim.nl",17,ExpectedResult = false)]
    [TestCase("s1165400@student.windesheim.nl",18,ExpectedResult = true)]
    public bool SetAge(string email, int age) {
        try {
            _database.SetMinAge(email, age);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl",99,ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",100,ExpectedResult = false)]
    public bool SetMaxAge(string email, int age) {
        try {
            _database.SetMaxAge(email, age);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetMinAge(string email) {
        try {
            var minAge = _database.GetMinAge(email);
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
            var maxAge = _database.GetMaxAge(email);
            if (maxAge != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s1165707@student.windesheim.nl","s168742@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","s1168742@student.windesheim.nl", ExpectedResult = false)]
    public bool CheckMatch(string email, string email2) {
        try {
            return _database.CheckMatch(email, email2);
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl","s168742@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","s1165400@student.windesheim.nl", ExpectedResult = false)]
    public bool NewLike(string email, string email2) {
        try {
            _database.NewLike(email, email2);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl","s168742@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("s1165707@student.windesheim.nl","s1165400@student.windesheim.nl", ExpectedResult = false)]
    public bool NewDislike(string email, string email2) {
        try {
            _database.NewDislike(email, email2);
            return true;
        } catch {
            return false;
        }
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetPictureFromDatabase(string email) {
        try {
            var picture = _database.GetPicturesFromDatabase(email);
            if (picture != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetUsersWhoLikedYou(string email) {
        try {
            string[] likes = Database.GetUsersWhoLikedYou(email);
            if (likes != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetRestOfUsers(string email) {
        try {
            var users = _database.GetRestOfUsers(email);
            if (users.Count >= 0 && users != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool GetUsersWithCommonInterests(string email) {
        try {
            var users = _database.GetUsersWithCommonInterest(email);
            if (users.Length >= 0 && users != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool AlgAlgorithmForSwipingorithm(string email) {
        try {
            List<string> users = Database.AlgorithmForSwiping(email);
            if (users.Count >= 0 && users != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    
    
    [TestCase("s1165707@student.windesheim.nl",ExpectedResult = true)]
    [TestCase("s1165400@student.windesheim.nl",ExpectedResult = false)]
    public bool Get5Profiles(string email) {
        try {
            var users = _database.Get5Profiles(email);
            if (users.Length >= 0 && users != null) {
                return true;
            }
        } catch {
            return false;
        }

        return false;
    }
    [TestCase("s1167488@student.windesheim.nl", "s1178208@student.windesheim.nl", "Vahe Test", ExpectedResult = true)]
    public bool SendMessageTest(string personFrom, string personTo, string message)
    {
        try
        {
            return _database.SendMessage(personFrom, personTo, message);
        }
        catch
        {
            return false;
        }
    }




}   

