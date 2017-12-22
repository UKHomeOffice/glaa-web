using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GLAA.ViewModels.Core;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.Admin
{
    public class AdminLicenceViewModel : PleaseSelectViewModel
    {
        public LicenceApplicationViewModel Licence { get; set; }

        public OrganisationDetailsViewModel OrganisationDetails { get; set; }
        public PrincipalAuthorityViewModel PrincipalAuthority { get; set; }
        public AlternativeBusinessRepresentativeCollectionViewModel AlternativeBusinessRepresentatives { get; set; }
        public DirectorOrPartnerCollectionViewModel DirectorsOrPartners { get; set; }
        public NamedIndividualCollectionViewModel NamedIndividuals { get; set; }
        public OrganisationViewModel Organisation { get; set; }

        public ICollection<LicenceStatusViewModel> LicenceStatusHistory { get; set; }

        public LicenceStatusViewModel LatestStatus => LicenceStatusHistory.OrderByDescending(l => l.DateCreated).First();

        public IEnumerable<SelectListItem> NextStatusDropDown => PleaseSelectItem.Concat(LatestStatus.NextStatuses.Select(s => s.DropDownItem));

        public List<CheckboxListItem> StandardCheckboxes { get; set; }

        [DisplayName("New Status")]
        public int NewLicenceStatus { get; set; }

        [DisplayName("Reason")]
        public int NewStatusReason { get; set; }
    }
}
