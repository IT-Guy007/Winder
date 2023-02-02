using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class DataCheckController
    {
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
    }
}
