using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.Admin
{
    public class AdminLicenceViewModel : PleaseSelectViewModel, INeedCountries, INeedCounties, INeedStandards
    {
        public AdminLicenceViewModel()
        {
            Licence = new LicenceApplicationViewModel();
            OrganisationDetails = new OrganisationDetailsViewModel();
            PrincipalAuthority = new PrincipalAuthorityViewModel();
            AlternativeBusinessRepresentatives = new AlternativeBusinessRepresentativeCollectionViewModel();
            DirectorsOrPartners = new DirectorOrPartnerCollectionViewModel();
            NamedIndividuals = new NamedIndividualCollectionViewModel();
            Organisation = new OrganisationViewModel();
        }

        public LicenceApplicationViewModel Licence { get; set; }
        public OrganisationDetailsViewModel OrganisationDetails { get; set; }
        public PrincipalAuthorityViewModel PrincipalAuthority { get; set; }
        public AlternativeBusinessRepresentativeCollectionViewModel AlternativeBusinessRepresentatives { get; set; }
        public DirectorOrPartnerCollectionViewModel DirectorsOrPartners { get; set; }
        public NamedIndividualCollectionViewModel NamedIndividuals { get; set; }
        public OrganisationViewModel Organisation { get; set; }

        public ICollection<LicenceStatusViewModel> LicenceStatusHistory { get; set; }

        public LicenceStatusViewModel LatestStatus => LicenceStatusHistory?.OrderByDescending(l => l.DateCreated).First();

        public IEnumerable<SelectListItem> NextStatusDropDown => LatestStatus != null ? PleaseSelectItem.Concat(LatestStatus.NextStatuses?.Select(s => s.DropDownItem)) : null;        

        [DisplayName("New Status")]
        public int NewLicenceStatus { get; set; }

        [DisplayName("Reason")]
        public int NewStatusReason { get; set; }

        public IEnumerable<SelectListItem> Counties
        {            
            get => OrganisationDetails.Counties;
            set
            {
                OrganisationDetails.Counties = value;
                PrincipalAuthority.Counties = value;
            }
        }

        public IEnumerable<SelectListItem> Countries
        {
            get => OrganisationDetails.Countries;
            set
            {
                OrganisationDetails.Countries = value;
                PrincipalAuthority.Countries = value;
            }
        }

        public List<CheckboxListItem> Standards { get; set; }        
    }
}
