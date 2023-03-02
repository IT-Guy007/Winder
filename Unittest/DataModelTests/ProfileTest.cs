using Controller;
using NUnit.Framework;

namespace Unittest.DataModelTests;

public class ProfileTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}