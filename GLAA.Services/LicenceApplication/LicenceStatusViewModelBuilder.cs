using System.Linq;
using AutoMapper;
using GLAA.Repository;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.LicenceApplication
{
    public class LicenceStatusViewModelBuilder : ILicenceStatusViewModelBuilder
    {
        private readonly ILicenceRepository licenceRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IMapper mapper;

        public LicenceStatusViewModelBuilder(ILicenceRepository licenceRepository, IStatusRepository statusRepository,
            IMapper mapper)
        {
            this.licenceRepository = licenceRepository;
            this.statusRepository = statusRepository;
            this.mapper = mapper;
        }
        
        public LicenceStatusViewModel New()
        {
            return new LicenceStatusViewModel();
        }

        public LicenceStatusViewModel BuildRandomStatus()
        {
            var status = statusRepository.GetRandomStatus();
            return mapper.Map<LicenceStatusViewModel>(status);
        }

        public LicenceStatusViewModel BuildLatestStatus(int licenceId)
        {
            var licence = licenceRepository.GetById(licenceId);

            var latestStatus = licence.CurrentStatusChange.Status;

            var model = mapper.Map<LicenceStatusViewModel>(latestStatus);

            return model;
        }
    }
}
