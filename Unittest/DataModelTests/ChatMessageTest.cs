using Controller;
using NUnit.Framework;

namespace Unittest.DataModelTests;

public class ChatMessageTest {
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}