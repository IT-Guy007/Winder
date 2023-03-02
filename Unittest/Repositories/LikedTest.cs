using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Winder.Repositories;
namespace Unittest.Repositories;

public class LikedTest
{
    private LikedRepository _likedRepository;

    [SetUp]
    public void Setup()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _likedRepository = new LikedRepository(configuration);
    }
    [TestCase("s1178208@student.windesheim.nl", "s1178208@student.windesheim.nl", ExpectedResult = true)]
    public bool CheckMatchTest(string emailLikedPerson, string emailCurrentUser) {
        return _likedRepository.CheckMatch(emailLikedPerson, emailCurrentUser);
    }
}