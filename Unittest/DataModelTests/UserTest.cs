using Controller;
using DataModel;
using NUnit.Framework;


namespace Unittest.DataModelTests;

public class UserTest {
    User user;

    [SetUp]
    public void Setup() {
        user = new User();
        Database.InitializeDebugConnection();
    }
    [Test]
    public void Registration_ShouldInsertUserIntoDatabase()
    {
        // Arrange
        string firstName = "John";
        string middleName = "Doe";
        string lastName = "Smith";
        string email = "jdoe@example.com";
        string preference = "male";
        DateTime birthday = new DateTime(1995, 12, 24);
        string gender = "male";
        string bio = "Hello, I'm John Doe";
        string password = "password";

        string imgLocation = "C:/Users/wesse/source/repos/Winder/Winder/Resources/Images/achtergrond.png";

        FileStream fileStream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
        BinaryReader binary = new BinaryReader(fileStream);
        byte[] imageArr = binary.ReadBytes((int)fileStream.Length);
        byte[] profilePicture = imageArr;

        bool active = true;
        string school = "Stanford University";
        string major = "Computer Science";
        // Act
        user.Registration(firstName, middleName, lastName, email, preference, birthday, gender, bio, password, profilePicture, active, school, major, Database.DebugConnection);
        
    }
}
    
