using Controller;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;
using Winder.Repositories;

namespace Unittest.ControllerTests
{
    public class SettingsControllerTests
    {
        private IUserRepository _userRepository;
        private IPhotosRepository _photoRepository;

        [SetUp]
        public void SetUp()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("configdatabase.test.json")
                .Build();
            _userRepository = new UserRepository(configuration);
            _photoRepository = new PhotosRepository(configuration);
        }

        private ValidationController CreateValidationController()
        {
            return new ValidationController(
                this._userRepository);
        }

        [TestCase("Man", ExpectedResult = 1)]
        [TestCase("Vrouw", ExpectedResult = 2)]
        [TestCase("IetsAnders", ExpectedResult = 2)]
        public int TestGetPreferenceFromUser(string preference)
        {
            return new SettingsController(_userRepository).GetPreferenceFromUser(preference);
        }

        [Test]
        public void TestGetPickerData()
        {
            var arrayWithNumbers = new SettingsController(_userRepository).GetPickerData();

            Assert.That(arrayWithNumbers.First() == 18 && arrayWithNumbers.Last() == 99);
        }
    }
}
