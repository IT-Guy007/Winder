using DataModel;
using NUnit.Framework;

namespace Unittest.ControllerTests;

public class AuthenticationTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}