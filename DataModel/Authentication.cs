namespace DataModel;

public class Authentication {
    
    private User currentUser { get; set; }
    private LoggedState _loggedState { get; set; }
    private AccountState _accountState { get; set; }
    

    public Authentication()
    {
        currentUser = new User();
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