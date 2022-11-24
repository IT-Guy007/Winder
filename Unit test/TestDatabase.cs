using System.Data.SqlClient;

namespace Unit_test;
using DataModel;
public class TestDatabase {
    
    private Authentication _authentication;
    private Database _database;
    
    [SetUp]
    public void Setup() {
        _authentication = new Authentication();
        _database = new Database();

    }
    
    [Test]
    public void TestCreateDatabaseConnection() {
        
        Database database = new Database();
        try {
            database.generateConnection();
            Assert.Pass();
        } catch(SqlException e) {
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
    
    [TestCase("Jeroen", "1234", ExpectedResult = false)]
    [TestCase("s1165707@student.windesheim.nl", "Qwerty1@", ExpectedResult = true)]
    public bool LoginTest(string email, string password) {
        return _database.checkLogin(email, password);

    }

    [TestCase("Test", "", "Account", "netwerkfailure", "male",
        "2003-12-10", "male", "Ik haat men leven want ik haal netwerken nooit", "Kaolozooi_ik_haal_netwerkenniet01@","",false,
        ExpectedResult = true)]

    public bool RegisterTest(string firstname, string middlename, string lastname, string username,
        string preference, DateTime birthday, string gender, string bio, string password,string profilePicture, bool active)
    {
        Random random = new Random();
        var email1 = random.Next(0, 999999);
        string email2 = "s" + email1 + "@student.windesheim.nl";
        return _database.register(firstname, middlename, lastname, username, email2, preference, birthday, gender, bio,
            password,profilePicture, active);
    }
}