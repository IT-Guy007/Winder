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
            database.generateConnection();
            Assert.Pass();
        } catch (SqlException e) {
            Assert.Fail(e.Message);
        }

    }

    [Test]

    public void TestDatabaseConnection() {
        
        Database database = new Database();
        try {
            database.openConnection();
            Assert.Pass();

            database.closeConnection();
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
        return _database.checkLogin(email, password);

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
        return _database.register(firstname, middlename, lastname, email2, preference, birthday, gender, bio,
            password,profilePicture, active, locatie, opleiding);
    }

    [TestCase("s1165707@student.windesheim.nl",false, ExpectedResult = true)]
    [TestCase("1707@student.windesheim.nl",false, ExpectedResult = false)]
    [TestCase("s1165707@student.windesheim.nl",true, ExpectedResult = true)]
    public bool ToggleActivationTest(string email, bool activation) {
        return _database.toggleActivation(email, activation);
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
}