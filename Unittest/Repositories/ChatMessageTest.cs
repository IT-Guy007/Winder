using DataModel;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Winder.Repositories;

namespace Unittest.Repositories;

public class ChatMessageTest
{
    private ChatMessageRepository _chatMessageRepository;

    private const string emailFrom = "s1178208@student.windesheim.nl";
    private const string emailTo = "s1168742@student.windesheim.nl";

    [SetUp]
    public void Setup()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("configdatabase.test.json")
            .Build();
        _chatMessageRepository = new ChatMessageRepository(configuration);
    }

    [TestCase(emailFrom, emailTo, ExpectedResult = true)]
    [TestCase(emailTo, emailFrom, ExpectedResult = true)]
    public bool TestSendingMessage(string EmailTo, string EmailFrom)
    {
        return _chatMessageRepository.SendMessage("Test", EmailFrom, EmailTo);
    }

    [Test]
    public void TestGetMessages()
    {
        List<ChatMessage> messages = _chatMessageRepository.GetChatMessages(emailTo, emailFrom);
        foreach (ChatMessage message in messages)
        {
            Assert.That(message.Message == "Test"); // There are only Messages with "Test" as text made by the test above.
        }
    }

    [Test]
    public void SetRead()
    {
        _chatMessageRepository.SetRead(emailFrom, emailTo);

        List<ChatMessage> messages = _chatMessageRepository.GetChatMessages(emailTo, emailFrom);
        foreach (ChatMessage message in messages)
        {
            if (message.FromUser == emailFrom && message.ToUser == emailTo) // Check to see if only the messages that are not send from the user are set to read for the other.
            {
                Assert.That(message.Read == true); // Messages are set to read for the other.
            }
            else
            {
                Assert.That(message.Read == false); // Messages are not set to read the other way, because you don't want to show that when you open a chat your messages are read by you.
            }
        }
    }
}


