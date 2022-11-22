namespace Unit_test;
using DataModel;

public class TestAuthentication {
    
    [Test]
    public void AuthenticationInitialize() {
        var auth = new Authentication();
        Assert.IsNotNull(auth);
    }

}