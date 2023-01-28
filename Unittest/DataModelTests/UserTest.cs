using DataModel;
using NUnit.Framework;

namespace Unittest.DataModelTests;

public class UserTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}