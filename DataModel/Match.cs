namespace DataModel;

public class Match {
    private User person1 { get; set; }
    private User person2 { get; set; }
    private DateTime matchDate { get; set; }


    public Match(User person1, User person2, DateTime matchDate) {
        this.person1 = person1;
        this.person2 = person2;
        this.matchDate = matchDate;
    }

}