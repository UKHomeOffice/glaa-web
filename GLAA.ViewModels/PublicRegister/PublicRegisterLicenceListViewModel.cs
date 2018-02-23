using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.PublicRegister
{
    public class PublicRegisterLicenceListViewModel : INeedCounties, INeedCountries
    {
        public PublicRegisterLicenceListViewModel()
        {
            PublicRegisterSearchCriteria = new PublicRegisterSearchCriteria {SearchActive = false};
        }

        public PublicRegisterLicenceListViewModel(List<SelectListItem> countries)
        {
            AvailableCountries = countries;
        }

        public IEnumerable<PublicRegisterLicenceSummaryViewModel> Licences { get; set; }
        public PublicRegisterSearchCriteria PublicRegisterSearchCriteria { get; set; }
        public List<PublicRegisterListItem<SupplierWho>> AvailableSuppliersWho => new List<PublicRegisterListItem<SupplierWho>>
        {
            new PublicRegisterListItem<SupplierWho> {Value = SupplierWho.Supply.ToString(), Text = "Supply", EnumMappedTo = SupplierWho.Supply},
            new PublicRegisterListItem<SupplierWho> {Value = SupplierWho.AreLocated.ToString(), Text = "Are Located", EnumMappedTo = SupplierWho.AreLocated}
        };
        public List<SelectListItem> AvailableCountries { get; set; }
        public IEnumerable<SelectListItem> Counties { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
