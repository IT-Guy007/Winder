using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataModel
{
    public class RegisterForm
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string GenderPreference { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Bio { get; set; }
        public string Major { get; set; }
        public bool ProfileActivation { get; set; }
        public List<string> ChosenInterests { get; set; }

       
    }
}
