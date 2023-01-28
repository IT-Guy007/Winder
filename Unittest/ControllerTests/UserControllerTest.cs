using DataModel;
using NUnit.Framework;

namespace Unittest.ControllerTests;

public class UserControllerTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}