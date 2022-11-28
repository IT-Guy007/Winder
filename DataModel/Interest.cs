namespace DataModel;

public class Interest {
    private int interestID { get; set; }
    private string name { get; set; }
    
    public Interest(int interestID, string name) {
        this.interestID = interestID;
        this.name = name;
    }
}