using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.Admin;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Admin
{
    public class AdminStatusRecordsViewModelBuilder : IAdminStatusRecordsViewModelBuilder
    {
        private readonly IMapper _mapper;
        private readonly IEntityFrameworkRepository _repository;
        private readonly ILicenceRepository _licenceRepository;
        private readonly IReferenceDataProvider _referenceDataProvider;

        public AdminStatusRecordsViewModelBuilder(
            IMapper mapper,
            IEntityFrameworkRepository repository,
            ILicenceRepository licenceRepository,
            IReferenceDataProvider referenceDataProvider)
        {
            _mapper = mapper;
            _repository = repository;
            _licenceRepository = licenceRepository;
            _referenceDataProvider = referenceDataProvider;
        }

        public AdminStatusDashboardViewModel Build()
        {
            var statuses = _repository.GetAll<LicenceStatus>();
            var adminStatusRecordsLicenceViewModel = new AdminStatusDashboardViewModel();
            var licenceStatuses = statuses.GroupJoin(_licenceRepository.GetAllEntriesWithStatusesAndAddress(), ls => ls.Id,
                l => l.CurrentStatusChange.Status.Id, (ls, licences) => new { ls, licences });

            foreach (var licenceStatus in licenceStatuses)
            {
                adminStatusRecordsLicenceViewModel.AdminStatusCountViewModels.Add(
                    new AdminStatusCountViewModel
                    {
                        LicenceStatusViewModel = _mapper.Map<LicenceStatusViewModel>(licenceStatus.ls),
                        LicenceApplicationCount = licenceStatus.licences?.Count() ?? 0
                    }
                );
            }

            return adminStatusRecordsLicenceViewModel;
        }

        public AdminStatusLicencesViewModel Build(int id)
        {
            return new AdminStatusLicencesViewModel
            {
                LicenceApplicationViewModels = GetLicencesByStatus(id).ToList(),
                LicenceStatusViewModel = _mapper.Map<LicenceStatusViewModel>(_repository.GetById<LicenceStatus>(id)),
                Countries = _referenceDataProvider.GetCountries()
            };
        }

        private IEnumerable<LicenceApplicationViewModel> GetLicencesByStatus(int statusId)
        {
            return _licenceRepository.GetAllEntriesWithStatusesAndAddress()
                .Where(x => x.CurrentStatusChange.Status.Id == statusId)
                .Select(_mapper.Map<LicenceApplicationViewModel>);
        }
    }
}
