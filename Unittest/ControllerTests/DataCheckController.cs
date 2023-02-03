using DataModel;
using NUnit.Framework;

namespace Unittest.ControllerTests
{
    public class DataCheckController
    {
        DataCheckController TestDataCheckController;

        [SetUp]

        public void Setup()
        {
            TestDataCheckController = new DataCheckController();
        }

        //[Test]
        //public void TestTextWithLettersAndSpacesReturnsTrue()
        //{
        //    string text = "hello world";
        //    bool result = TestDataCheckController.CheckIfTextIsOnlyLettersAndSpaces(text);
        //    Assert.IsTrue(result);
        //}


    }
}
