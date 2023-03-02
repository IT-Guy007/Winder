using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Winder.Repositories;
using Winder.Repositories.Interfaces;

namespace Unittest.Repositories;

public class ChatMessageTest
{
    private ChatMessageRepository _chatMessageRepository;

    [SetUp]
    public void Setup()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _chatMessageRepository = new ChatMessageRepository(configuration);
    }
    
    [TestCase(ExpectedResult = true)]
    public bool TestSendingMessage()
    {
        return _chatMessageRepository.SendMessage("Test", "s1178208@student.windesheim.nl", "s1178208@student.windesheim.nl");
    }

    [Test]
    public void TestGetMessages()
    {
        Assert.IsNotNull(_chatMessageRepository.GetChatMessages("s1178208@student.windesheim.nl", "s1178208@student.windesheim.nl"));
    }

    [Test]
    public void SetRead()
    {
        Assert.IsTrue(_chatMessageRepository.SetRead("s1178208@student.windesheim.nl", "s1178208@student.windesheim.nl"));
    }
}


