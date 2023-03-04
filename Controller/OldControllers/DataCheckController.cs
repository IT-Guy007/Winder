

namespace Controller
{
    public class DataCheckController
    {
        //Checks if input has spaces, letters or dashes
        public bool CheckIfTextIsOnlyLettersAndSpaces(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-' && c != '\n' && c != '\r')
                {
                    return false;
                }
            }
            return true;
        }
        
        //Check if input only consists of letters
        public bool CheckIfTextIsOnlyLetters(string text)
        {
            if (text.All(char.IsLetter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if age is 18 or older
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns>returns the age</returns>
        public int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
                age--;

            return age;
        }

        
    }
}
