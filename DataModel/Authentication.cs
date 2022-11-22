namespace DataModel;
using System.Drawing;

public class Authentication {
    
    private User currentUser { get; set; }
    private LoggedState _loggedState { get; set; }
    private AccountState _accountState { get; set; }
    

    public Authentication() {
        currentUser = new User();
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

    public bool CheckPassword(string password) {
        if (PasswordLength(password) && PasswordContainsNumber(password) && PasswordContainsCapitalLetter(password)) {
            return true;
        }
        return false;
    }

    public bool CheckEmail(string email) {
        if (email.EndsWith("@student.windesheim.nl") && email.StartsWith("s")) {
            return true;
        }
        return false;
    }

    private bool PasswordLength(string password) {
        return password.Length >= 8;
    }

    private bool PasswordContainsNumber(string password) {
        return password.Any(char.IsDigit);
    }

    private bool PasswordContainsCapitalLetter(string password) {
        return password.Any(char.IsUpper);
    }
    
}