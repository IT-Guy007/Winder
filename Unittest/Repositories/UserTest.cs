using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Winder.Repositories;
namespace Unittest.Repositories;

public class UserTest
{
    private UserRepository _userRepository;

    [SetUp]
    public void Setup()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _userRepository = new UserRepository(configuration);
        
        
    }
    
    
}