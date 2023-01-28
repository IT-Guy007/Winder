using DataModel;
using NUnit.Framework;

namespace Unittest.ControllerTests;

public class SwipeControllerTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}