using Controller;
using DataModel;
using NUnit.Framework;

namespace Unittest.ControllerTests;

public class AuthenticationTest {
    
    
    [Test]
    public void SetterCurrentUser() {
        var user = new User();
        User.CurrentUser = user;
        Assert.AreEqual(user, User.CurrentUser);
    }
}