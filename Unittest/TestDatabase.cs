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
        
        Database database = new Database();
        try {
            database.GenerateConnection();
            Assert.Pass();
        } catch (SqlException e) {
            Assert.Fail(e.Message);
        }

    }

    [Test]

    public void TestDatabaseConnection() {
        
        Database database = new Database();
        try {
            database.OpenConnection();
            Assert.Pass();

            database.CloseConnection();
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
        return _database.Register(firstname, middlename, lastname, email2, preference, birthday, gender, bio,
            password,profilePicture, active, locatie, opleiding);
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
        return _database.LoadInterestsFromDatabaseInListInteresses(email).Count > 0;
    }

    [TestCase("s1416890@student.windesheim.nl", "Lezen", ExpectedResult = true)]
    [TestCase("s1416890@student.windesheim.nl", "bestaat niet", ExpectedResult = false)]
    public bool removeInterestOutOfuserHasInterestTableDatabaseTest(string email, string interest)
    {
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
            _database.GetUsersWhoLikedYou(email);
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
    public bool DatabaseAlgorithmForSwiping(string email)
    {
        try
        {
            _database.AlgorithmForSwiping(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}