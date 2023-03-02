using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Winder.Repositories;
namespace Unittest.Repositories;

public class InterestsTest
{
    private InterestsRepository _interestsRepository;

    [SetUp]
    public void SetUp()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _interestsRepository = new InterestsRepository(configuration);
    }

    [Test]
    public void GetInterestsTest() {
        Assert.IsNotNull(_interestsRepository.GetInterests());
    }
}