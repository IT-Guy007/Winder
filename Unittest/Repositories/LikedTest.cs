using DataModel;
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
    public bool NewLikeTest(string emailLikedPerson, string emailCurrentUser)
    {
        return _likedRepository.NewLike(emailLikedPerson, emailCurrentUser);
    }
    [TestCase("s1178208@student.windesheim.nl", "s1178208@student.windesheim.nl", ExpectedResult = true)]
    public bool CheckMatchTest(string emailLikedPerson, string emailCurrentUser)
    {
        return _likedRepository.CheckMatch(emailLikedPerson, emailCurrentUser);
    }
    [TestCase("s1178208@student.windesheim.nl", "s1178208@student.windesheim.nl", ExpectedResult = true)]
    public bool DeleteTest(string emailLikedPerson, string emailCurrentUser)
    {
        return _likedRepository.DeleteLike(emailLikedPerson, emailCurrentUser);
    }
    [TestCase("s1178208@student.windesheim.nl", "s1178208@student.windesheim.nl", ExpectedResult = true)]
    public bool NewDislikeTest(string emailLikedPerson, string emailCurrentUser)
    {
        return _likedRepository.NewDislike(emailLikedPerson, emailCurrentUser);
    }
    [TestCase("s1178208@student.windesheim.nl")]
    public void GetUsersWhoLikedYouTest(string email)
    {
        // Act
        Queue<string> likedUsers = _likedRepository.GetUsersWhoLikedYou(email);

        // Assert
        Assert.IsNotNull(likedUsers, "doesnt work");
    }
    }