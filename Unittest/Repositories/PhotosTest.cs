using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Winder.Repositories;
namespace Unittest.Repositories;

public class PhotosTest
{
    private PhotosRepository _photosRepository;

    [SetUp]
    public void Setup()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _photosRepository = new PhotosRepository(configuration);
        
        
    }
    
    
}