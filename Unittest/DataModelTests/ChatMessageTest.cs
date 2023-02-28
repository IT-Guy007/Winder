using Controller;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Data.SqlClient;
using Winder.Repositories;
namespace Unittest.DataModelTests;

public class ChatMessageTest
{
    ChatMessageRepository chatMessageRepository;

    [SetUp]
    public void Setup() {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("configdatabase.test.json")
                .Build();
        chatMessageRepository = new ChatMessageRepository(configuration);
    }
    [TestCase(ExpectedResult = true)]
    public bool TestSendingMessage()
    {
        return chatMessageRepository.SendMessage("Test", "s1178208@student.windesheim.nl", "s1178208@student.windesheim.nl");
    }
}