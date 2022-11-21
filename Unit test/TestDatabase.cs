namespace Unit_test;
using DataModel;
public class TestDatabase {
    
    [Test]
    public void TestDatabaseConnection() {
        
        Database database = new Database();
        try {
            database.generateConnection();
            database.openConnection();
            database.closeConnection();
            Assert.Pass();
        } catch(Exception e) {

            Assert.Fail(e.Message);
        }
        
    }
}