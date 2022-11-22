using System.Data.SqlClient;

namespace Unit_test;
using DataModel;
public class TestDatabase {
    
    [Test]
    public void TestDatabaseConnection() {
        
        Database database = new Database();
        try {
            database.generateConnection();
            Assert.Pass();
        } catch(SqlException e) {
            Assert.Fail(e.Message);
        }
        
    }
    
    [Test]
    public void TestOpenDatabaseConnection() {
        
        Database database = new Database();
        try {
            database.openConnection();
            Assert.Pass();
        } catch(SqlException e) {
            Assert.Fail(e.Message);
        }
        
    }
}