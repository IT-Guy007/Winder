namespace DataModel;

public class Match {
    public User Person1 { get; private set; } //This user
    public User Person2 { get; private set; } //The other user


    public Match(User person1, User person2) {
        Person1 = person1;
        Person2 = person2;
    }

}