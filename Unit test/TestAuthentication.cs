namespace Unit_test;
using DataModel;

public class TestAuthentication {
    Authentication authentication;
    
    [SetUp]
    public void Setup() {
        authentication = new Authentication();
    }
    
    [Test]
    public void AuthenticationInitialize() {
        Assert.IsNotNull(authentication);
    }

    [TestCase("", ExpectedResult = false)]
    [TestCase("langwachtwoordzonderhoofdletters", ExpectedResult = false)]
    [TestCase("LangWachtwoordMetHoofdLetters", ExpectedResult = false)]
    [TestCase("LangWachtwoordMetHoofdLettersEn123", ExpectedResult = true)]
    public bool AuthenticationPasswordCheck(string password) {
        return authentication.CheckPassword(password);
    }

    [TestCase("", ExpectedResult = false)]
    [TestCase("randomemail@gmail.com", ExpectedResult = false)]
    [TestCase("NietEensEenEmail", ExpectedResult = false)]
    [TestCase("sDITHOEVENGEENCIJFERSTEZIJN@student.windesheim.nl", ExpectedResult = true)]
    [TestCase("student.windesheim.nl", ExpectedResult = false)]
    public bool AuthenticationEmailCheck(string email) {
        return authentication.CheckEmail(email);
    }

}