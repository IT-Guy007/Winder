using Controller;
using DataModel;
using NUnit.Framework;

namespace Unittest.ControllerTests;

public class ProfileQueueControllerTest {

    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
}