using Controller;
using DataModel;
using NUnit.Framework;
using System.Net.Mail;

namespace Unittest.DataModelTests;


public class EmailMessageTest {
    
    
    [SetUp]
    public void Setup() {
        Database.InitializeDebugConnection();
    }
    
   
}