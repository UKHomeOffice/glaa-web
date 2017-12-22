using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.Admin;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Admin
{
    public class AdminLicenceViewModelBuilder : IAdminLicenceViewModelBuilder
    {
        private readonly ILicenceRepository licenceRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IMapper mapper;

        public AdminLicenceViewModelBuilder(ILicenceRepository licenceRepository, IStatusRepository statusRepository, IMapper mapper)
        {
            this.licenceRepository = licenceRepository;
            this.statusRepository = statusRepository;
            this.mapper = mapper;
        }

        public AdminLicenceViewModel New()
        {
            return new AdminLicenceViewModel();
        }

        public AdminLicenceViewModel Build(int id)
        {
            var licence = licenceRepository.GetById(id);

            var licenceModel = mapper.Map<LicenceApplicationViewModel>(licence);           

            var standards = statusRepository.GetAll<LicensingStandard>();

            return new AdminLicenceViewModel
            {
                Licence = licenceModel,
                OrganisationDetails = mapper.Map<OrganisationDetailsViewModel>(licence),
                PrincipalAuthority = mapper.Map<PrincipalAuthorityViewModel>(licence.PrincipalAuthorities.FirstOrDefault()),
                AlternativeBusinessRepresentatives = mapper.Map<AlternativeBusinessRepresentativeCollectionViewModel>(licence),
                DirectorsOrPartners = mapper.Map<DirectorOrPartnerCollectionViewModel>(licence),
                NamedIndividuals = mapper.Map<NamedIndividualCollectionViewModel>(licence),
                Organisation = mapper.Map<OrganisationViewModel>(licence),
                LicenceStatusHistory = licence.LicenceStatusHistory.Select(x => mapper.Map<LicenceStatusViewModel>(x)).ToList(),
                StandardCheckboxes = standards.Select(s => new CheckboxListItem { Id = s.Id, Name = s.Name }).ToList()
            };
        }
    }
}
