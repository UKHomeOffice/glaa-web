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

        public AdminStatusRecordsViewModelBuilder(
            IMapper mapper,
            IEntityFrameworkRepository repository,
            ILicenceRepository licenceRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _licenceRepository = licenceRepository;
        }

        public AdminStatusDashboardViewModel Build()
        {
            var statuses = _repository.GetAll<LicenceStatus>();
            var adminStatusRecordsLicenceViewModel = new AdminStatusDashboardViewModel();

            //var licenceStatusRecords = _repository.GetAll<LicenceStatusChange>().GroupBy(x => x.Status.Id, (key, group) => group.OrderBy(y => y.DateCreated).First());
            //var licenceStatusRecords = _repository.GetAll<Licence>().GroupBy(x => x.CurrentStatusChange.Status.Id, (key, group));

            var licenceStatuses = statuses.GroupJoin(_licenceRepository.GetAllEntriesWithStatusesAndAddress(), ls => ls.Id,
                l => l.CurrentStatusChange.Status.Id, (ls, licences) => new { ls, licences });

            foreach (var licenceStatus in licenceStatuses)
            {
                //var licencesByStatus = GetLicencesByStatus(licenceStatus.ls.Id)?.Count() ?? 0;

                adminStatusRecordsLicenceViewModel.AdminStatusCountViewModels.Add(
                    new AdminStatusCountViewModel
                    {
                        LicenceStatusViewModel = _mapper.Map<LicenceStatusViewModel>(licenceStatus.ls),
                        LicenceApplicationCount = licenceStatus.licences?.Count() ?? 0 //licencesByStatus?.Count() ?? 0
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
                LicenceStatusViewModel = _mapper.Map<LicenceStatusViewModel>(_repository.GetById<LicenceStatus>(id))
            };
        }

        private IEnumerable<LicenceApplicationViewModel> GetLicencesByStatus(int statusId)
        {
            return _licenceRepository.GetAllEntriesWithStatusesAndAddress().Where(x => x.CurrentStatusChange.Status.Id == statusId)
                .Select(_mapper.Map<LicenceApplicationViewModel>);
        }
    }
}
