using Controller;
using NUnit.Framework;

namespace Unittest.DataModelTests;

public class EmailMessageTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}