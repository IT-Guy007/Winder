using System.Drawing;
using System.Numerics;

namespace Unit_test;
using DataModel;

public class TestAuthentication {
    private Authentication _authentication;
    private Database _database;

    [SetUp]
    public void Setup() {
        _authentication = new Authentication();
        _database = new Database();

    }

    
}