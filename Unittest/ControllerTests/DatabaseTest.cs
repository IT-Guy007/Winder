using DataModel;
using NUnit.Framework;

namespace Unittest.ControllerTests;

public class DatabaseTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}