namespace DataModel;
using System.Drawing;

public class Authentication {
    
    public static User _currentUser { get; set; }
    private LoggedState _loggedState { get; set; }
    private AccountState _accountState { get; set; }
    

    public Authentication() {
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