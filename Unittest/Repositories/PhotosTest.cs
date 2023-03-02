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

    [TestCase(new byte[0],"s11657077@student.windesheim.nl", ExpectedResult = true)]
    public bool AddPhotoTest(byte[] image,string email) {
        return _photosRepository.AddPhoto(image, email);
    }

    [TestCase("s11657077@student.windesheim.nl", ExpectedResult = true)]
    public bool DeleteAllPhotosTest(string email) {
        return _photosRepository.DeleteAllPhotos(email);
    }

    [TestCase("s11657077@student.windesheim.nl")]
    public void GetPhotosTest(string email) {
        Assert.IsNotNull(_photosRepository.GetPhotos(email));
    }
}