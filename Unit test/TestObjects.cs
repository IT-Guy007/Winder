using System.Drawing;

namespace Unit_test;
using DataModel;
public class TestObjects {

    [Test]
    public void TestUserCreation() {
        try {
            User user = new User(0,"Padawan","Jeroen","den","Otter",DateTime.Now,"Female","s1165707@student.windesheim.nl","MysecretPassword1@","Male", new Bitmap(0,0));
        }
        catch (Exception e) {
            Assert.Fail(e.Message);
        }
    }
    
    [Test]
    public void TestInterest() {
        try {
            Interest interest = new Interest(0, "sport");
            Assert.Pass();
        }
        catch (Exception e) {
            Assert.Fail(e.Message);
        }
    }
    
    [Test]
    public void TestMatch() {

        try {
            User person1 = new User(0,"Padawan","Jeroen","den","Otter",DateTime.Now,"Female","s1165707@student.windesheim.nl","MysecretPassword1@","Male", new Bitmap(0,0));
            User person2 = new User(0,"Padawan","Wessel","","Koopman",DateTime.Now,"Male","s1168751@student.windesheim.nl","MysecretPassword1@","Male", new Bitmap(0,0));

            Match match = new Match(0,person1, person2,DateTime.Now);
            Assert.Pass();
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }
}