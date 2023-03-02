using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winder.Repositories.Interfaces
{
    internal interface IInterestsRepository
    {
        public List<string> GetInterests();
        public List<string> GetInterestsFromUser(string email);
    }
}
