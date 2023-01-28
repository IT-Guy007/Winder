using DataModel;
using NUnit.Framework;

namespace Unittest.DataModelTests;

public class ChatModelTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}