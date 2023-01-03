using DataModel;
using NUnit.Framework;

namespace Unittest;

public class TestAuthentication {

    Authentication authentication;
    private Database database;

    [SetUp]
    public void Setup() {
        authentication = new Authentication();
        database = new Database();
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

    [TestCase(" ", ExpectedResult = "36A9E7F1C95B82FFB99743E0C5C4CE95D83C9A430AAC59F84EF3CBFAB6145068")]
    [TestCase("Password123", ExpectedResult = "008C70392E3ABFBD0FA47BBC2ED96AA99BD49E159727FCBA0F2E6ABEB3A9D601")]
    [TestCase("lullo", ExpectedResult = "A6AC7840B2BAA721A972CC7DD15BC85C9CABDEDA808725C8B0645B9A92E07ADA")]
    [TestCase("jeroenhaaltnetwerkenookniet", ExpectedResult = "DFEB03C43C589CE4F0C742C0C5EB153DEC47370A61A03CFF691806E1EC27082F")]
    public string AuthenticationHashPassword(string password) {
        return authentication.HashPassword(password);
    }

    [TestCase("2000-01-01", ExpectedResult = 23)]
    [TestCase("2001-11-23", ExpectedResult = 21)]
    [TestCase("2003-12-10", ExpectedResult = 19)]
    [TestCase("2001-01-11", ExpectedResult = 21)]
    [TestCase("1999-11-14", ExpectedResult = 23)]
    public int AuthenticationCalculateAge(DateTime birthDate) {
        return authentication.CalculateAge(birthDate);
    }
    
    [TestCase("s1165707@student.windesheim.nl", ExpectedResult = false)]
    [TestCase("ditisechteenkutemail@ikhaalnetwerkenniet.com", ExpectedResult = true)]
    public bool AuthenticationEmailIsUnique(string email) {
        return authentication.EmailIsUnique(email);
    }
    
    [TestCase(4, ExpectedResult = "4")]
    [TestCase(0, ExpectedResult = "0")]
    [TestCase(9, ExpectedResult = "9")]
    public string AuthenticationRandomString(int length)
    {
        return Authentication.RandomString(length).Length.ToString();

    }

    [TestCase("1174004@student.widnesheim.nl", "hoi", "hoi", ExpectedResult = true)]
    [TestCase("1174004@student.widnesheim.nl", "hoi", "", ExpectedResult = true)]
    [TestCase("1174004@student.widnesheim.nl", "", "hoi", ExpectedResult = true)]
    [TestCase("", "hoi", "hoi", ExpectedResult = false)]
    [TestCase("1174004", "hoi", "hoi", ExpectedResult = false)]
    public bool AuthenticationSendEmail(string email, string subject, string body) {
        try {
            authentication.SendEmail(email, subject, body);
            return true;
        } catch {
            return false;
        }
    }
}