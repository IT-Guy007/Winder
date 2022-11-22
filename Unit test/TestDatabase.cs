using System.Data.SqlClient;

namespace Unit_test;
using DataModel;
public class TestDatabase {
    
    [Test]
    public void TestCreateDatabaseConnection() {
        
        Database database = new Database();
        try {
            database.generateConnection();
            Assert.Pass();
        } catch(SqlException e) {
            Assert.Fail(e.Message);
        }
        
    }
    
    [Test]
    public void TestDatabaseConnection() {
        
        Database database = new Database();
        try {
            database.openConnection();
            Assert.Pass();
            database.closeConnection();
        } catch(SqlException e) {
            Assert.Fail(e.Message);
        }
        
    }
}