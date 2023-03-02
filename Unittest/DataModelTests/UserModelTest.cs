using Controller;
using NUnit.Framework;

namespace Unittest.DataModelTests;

public class UserModelTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}