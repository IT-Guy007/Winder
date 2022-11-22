namespace Unit_test;
using DataModel;

public class TestAuthentication
{
    private Authentication _authentication;
    private Database _database;
    
    [SetUp]
    public void Setup()
    {
        _authentication = new Authentication();
        _database = new Database();
        
    }

    [TestCase("Jeroen", "1234",ExpectedResult = false)]
    [TestCase("jeroen.denotter@icloud.com", "Qwerty1@",ExpectedResult = true)]
    public bool LoginTest(string email, string password) {
        return _database.checkLogin(email, password);

    }

}