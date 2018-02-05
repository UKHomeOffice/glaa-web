using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.PublicRegister;

namespace GLAA.Services.PublicRegister
{
    public class PublicRegisterViewModelBuilder : IPublicRegisterViewModelBuilder
    {
        private readonly IEntityFrameworkRepository repository;
        private readonly ILicenceRepository licenceRepository;
        private readonly IMapper mapper;

        public PublicRegisterViewModelBuilder(
            IMapper mapper,
            IEntityFrameworkRepository repository,
            ILicenceRepository licenceRepository)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.licenceRepository = licenceRepository;
        }

        public PublicRegisterLicenceListViewModel BuildAllLicences()
        {
            return new PublicRegisterLicenceListViewModel
            {
                Title = "Public Register",
                //We only want licences with status that are allowed to be shown using the "ShowInPublicRegister" field.
                Licences = licenceRepository.GetAllLicences()
                .Where(x => GetLatestStatus(x).Status.ShowInPublicRegister)
                    .Select(BuildSummary)
            };
        }

        public PublicRegisterLicenceListViewModel BuildSearchForLicences(PublicRegisterSearchViewModel publicRegisterSearchViewModel)
        {
            return new PublicRegisterLicenceListViewModel
            {
                Title = "Public Register",
                //We only want licences with status that are allowed to be shown using the "ShowInPublicRegister" field.
                Licences = licenceRepository.GetAllLicences()
                    .Where(x => GetLatestStatus(x).Status.ShowInPublicRegister
                                && (x.OrganisationName.Contains(publicRegisterSearchViewModel.OrganisationName) ||
                                    string.IsNullOrWhiteSpace(publicRegisterSearchViewModel.OrganisationName))
                                && (x.OperatingCountries.Any(y =>
                                        y.Country.Name.Contains(publicRegisterSearchViewModel.LicenceCountry)) ||
                                    string.IsNullOrWhiteSpace(publicRegisterSearchViewModel.LicenceCountry))
                                && (x.Address.Country.Contains(publicRegisterSearchViewModel.OrganisationCountry) ||
                                    string.IsNullOrWhiteSpace(publicRegisterSearchViewModel.OrganisationCountry)))
                    .Select(BuildSummary)
            };
        }

        public PublicRegisterLicenceSummaryViewModel BuildLicence(int id)
        {
            var licence = licenceRepository.GetById(id);

            return GetLatestStatus(licence).Status.IsLicence ? BuildSummary(licence) : null;
        }

        private PublicRegisterLicenceSummaryViewModel BuildSummary(Licence licence)
        {
            return mapper.Map<PublicRegisterLicenceSummaryViewModel>(licence);
        }

        public static LicenceStatusChange GetLatestStatus(Licence licence)
        {
            return licence.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).First();
        }
    }
}
