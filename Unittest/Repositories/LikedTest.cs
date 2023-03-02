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


}