using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winder.Repositories.Interfaces;

namespace Winder.Repositories
{
    public class InterestsRepository : IInterestsRepository
    {
        private readonly IConfiguration _configuration;
        public InterestsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<string> GetInterests()
        {
            throw new NotImplementedException();
        }

        public List<string> GetInterestsFromUser(string email)
        {
            throw new NotImplementedException();
        }
    }
}
