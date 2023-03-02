using Controller;
using DataModel;
using NUnit.Framework;

namespace Unittest.DataModelTests;

public class InterestModelTest {

    InterestsModel TestInterestModel;
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
        TestInterestModel = new InterestsModel();
    }

    [Test]
    public void GetInterests_ValidData_Returns49Interests()
    {
        //Arrange
        int totalInterests = 49;
        
        //Act
        var interests = TestInterestModel.GetInterestsFromDataBase(Database.DebugConnection);

        //Assert
        Assert.AreEqual(totalInterests, interests.Count);

    }
}