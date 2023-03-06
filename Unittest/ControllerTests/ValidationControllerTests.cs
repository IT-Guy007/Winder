using Controller;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using Winder.Repositories;
using Winder.Repositories.Interfaces;

namespace Unittest.ControllerTests
{
    public class ValidationControllerTests
    {
        private IUserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("configdatabase.test.json")
                .Build();
            _userRepository = new UserRepository(configuration);
        }

        private ValidationController CreateValidationController()
        {
            return new ValidationController(
                this._userRepository);
        }

        [TestCase(" ", ExpectedResult = false)]
        [TestCase("s", ExpectedResult = false)]
        [TestCase("@student.windesheim.nl", ExpectedResult = false)]
        [TestCase("sThereCanBeTextHere@student.windesheim.nl", ExpectedResult = true)]
        [TestCase("s1@student.windesheim.nl", ExpectedResult = true)]
        [TestCase("s0123456789@student.windesheim.nl", ExpectedResult = true)]
        public bool TestCheckEmail(string email)
        {
            // Arrange
            var validationController = CreateValidationController();

            // Act
            var result = validationController.CheckEmail(email);

            // Assert
            return result;
        }

        [TestCase("Qwerty1@", ExpectedResult = true)]
        [TestCase("Qwerty1A", ExpectedResult = true)]
        [TestCase("qwerty1@", ExpectedResult = false)]
        [TestCase("QwertyU@", ExpectedResult = false)]
        [TestCase("Qwert1@", ExpectedResult = false)]
        public bool TestCheckPassword(string password)
        {
            // Arrange
            var validationController = CreateValidationController();

            // Act
            var result = validationController.CheckPassword(
                password);

            // Assert
            return result;
        }

        // must be edited each year
        [TestCase("Jan 1, 2009", ExpectedResult = 14)]
        [TestCase("Jan 1, 2003", ExpectedResult = 20)]
        [TestCase("Jan 1, 2001", ExpectedResult = 22)]
        [TestCase("Jan 1, 1968", ExpectedResult = 55)]
        public int TestCalculateAge(string birthday)
        {
            var parsedDate = DateTime.Parse(birthday);

            // Arrange
            var validationController = CreateValidationController();

            // Act
            var result = validationController.CalculateAge(parsedDate);

            // Assert
            return result;
        }

        // hashes in the ExpectedResult are calculated using WinHasher
        [TestCase("Qwerty1@", ExpectedResult = "457D1FA123F6F942CA95C00B2C77602569EC41AC10F68674C18BF0E698BF9120")]
        [TestCase("Hello", ExpectedResult = "185F8DB32271FE25F561A6FC938B2E264306EC304EDA518007D1764826381969")]
        [TestCase("ABCDEFGH", ExpectedResult = "9AC2197D9258257B1AE8463E4214E4CD0A578BC1517F2415928B91BE4283FC48")]
        [TestCase("AlsoLongerPasswordAreHashed", ExpectedResult = "C8F04DB534F1C43104D6CA5C2C23FA9BE55F81F39B2658386EDC62B63EF2FCCF")]
        public string TestHashPassword(string password)
        {
            // Arrange
            var validationController = CreateValidationController();

            // Act
            var result = validationController.HashPassword(password);

            // Assert
            return result;
        }

        [TestCase("This is a text with only letters and spaces", ExpectedResult = true)]
        [TestCase("This is a text with --- letters and spaces", ExpectedResult = true)]
        [TestCase("This is a text with , letters spaces", ExpectedResult = false)] // you can't use any special characters except '-'
        [TestCase("This is a text with --- letters spaces \n here the text goes on", ExpectedResult = true)]
        [TestCase("This is a text with --- letters spaces \r here the text goes on", ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        public bool TestCheckBioFormat(string bio)
        {
            // Arrange
            var validationController = CreateValidationController();

            // Act
            var result = validationController.CheckBioFormat(bio);

            // Assert
            return result;
        }

        [TestCase("", ExpectedResult = true)]
        [TestCase("OnlyTextIsFoundHere", ExpectedResult = true)]
        [TestCase("Spaces are added", ExpectedResult = false)]
        [TestCase("NumbersAreAThing1234567890", ExpectedResult = false)]
        [TestCase("SoAreSpecialCharacters!@#$%^&*()", ExpectedResult = false)]
        public bool TestCheckIfTextIsOnlyLetters(string text)
        {
            // Arrange
            var validationController = CreateValidationController();

            // Act
            var result = validationController.CheckIfTextIsOnlyLetters(text);

            // Assert
            return result;
        }
    }
}
