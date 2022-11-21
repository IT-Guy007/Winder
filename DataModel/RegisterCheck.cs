using System.Linq;

namespace DataModel {
    public class RegisterCheck {
        
        public bool CheckPassword(string password) {
            if (PasswordLength(password) && PasswordContainsNumber(password) && PasswordContainsCapitalLetter(password)) {
                return true;
            }
            return false;
        }

        public bool CheckEmail(string email) {
            if (email.EndsWith("student.windesheim.nl") && email.StartsWith("s")) {
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
}