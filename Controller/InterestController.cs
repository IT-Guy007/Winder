using DataModel;
using Winder.Repositories.Interfaces;

namespace Controller
{
    public class InterestController
    {
        private readonly IInterestsRepository _interestsRepository;

        public InterestController(IInterestsRepository interestsRepository)
        {
            _interestsRepository = interestsRepository;
        }

        public List<string> GetInterests()
        {
            InterestsModel interestsListModel = new InterestsModel(_interestsRepository.GetInterests());
            return InterestsModel.InterestsList;
        }
    }
}
