namespace DataModel;
using System.Drawing;

public class Authentication {
    
    private User currentUser { get; set; }
    private LoggedState _loggedState { get; set; }
    private AccountState _accountState { get; set; }
    

    public Authentication() {
        currentUser  = new User(0,"Padawan","Jeroen","den","Otter",DateTime.Now,"Female","s1165707@student.windesheim.nl","MysecretPassword1@","Male", new Bitmap(0,0));
        _loggedState = new LoggedState();
        _accountState = new  AccountState();

        _loggedState = LoggedState.signedOut;
        _accountState = AccountState.inactive;
    }
    
    
    //Defining state
    enum LoggedState {
        signedIn,
        signedOut,
    }
    
    enum  AccountState {
        active,
        inactive,
    }
}