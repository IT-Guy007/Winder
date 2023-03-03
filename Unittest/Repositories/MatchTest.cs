using DataModel;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Winder.Repositories;

namespace Unittest.Repositories;

public class MatchTest
{
    private MatchRepository _matchRepository;

    [SetUp]
    public void SetUp()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _matchRepository = new MatchRepository(configuration);
        
    }
    
    [TestCase("s1165707@student.windesheim.nl","", ExpectedResult = true)]
    public bool AddMatchTest(string emailLikedPerson, string emailCurrentUser)
    {
        
        return _matchRepository.AddMatch(emailLikedPerson, emailCurrentUser);
    }

    [Test]
    public void GetMatchedStudentsFromUserTest()
    {
        User testUser = new User();
        testUser.Email = "s1165707@student.windesheim.nl";
        Assert.IsNotEmpty(_matchRepository.GetMatchedStudentsFromUser(testUser));
    }
}