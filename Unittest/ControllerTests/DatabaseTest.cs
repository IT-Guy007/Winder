using DataModel;
using NUnit.Framework;

namespace Unittest.ControllerTests;

public class DatabaseTest {

    [Test]
    public void TestReleaseConnection() {
        Database.InitializeDebugConnection();
        Database.InitializeReleaseConnection();
        Assert.Pass();
    }
}