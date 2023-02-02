using Controller;
using NUnit.Framework;

namespace Unittest.ControllerTests;

public class UserControllerTest {
    
    private UserController UserController;

    [SetUp]
    public void Setup() {
        UserController = new UserController();
    }
    
    [TestCase("Man", ExpectedResult = 1)]
    [TestCase("Vrouw", ExpectedResult = 2)]
    [TestCase("Other", ExpectedResult = 2)]
    [TestCase("", ExpectedResult = 2)]
    public int GetPreferenceFromUserTest(string preference) {
        return UserController.GetPreferenceFromUser(preference);
    }
    
    
}