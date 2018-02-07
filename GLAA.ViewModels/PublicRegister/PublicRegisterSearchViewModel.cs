using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.PublicRegister
{
    public class PublicRegisterSearchViewModel
    {
        public PublicRegisterSearchViewModel()
        {
            SearchActive = false;
        }

        public PublicRegisterSearchViewModel(List<SelectListItem> countries)
        {
            SearchActive = false;
            AvailableCountries = countries;
        }

        public string BusinessName { get; set; }
        public string SupplierWho { get; set; }
        public List<string> CountriesSelected { get; set; }
        public string CountryAdded { get; set; }
        public string CountryRemoved { get; set; }
        public List<SelectListItem> AvailableSuppliersWho => new List<SelectListItem>
            {
                new SelectListItem {Value = "supply", Text = "Supply"},
                new SelectListItem {Value = "arelocated", Text = "Are Located"}
            };
        public List<SelectListItem> AvailableCountries { get; set; }
        public bool SearchActive { get; set; }
    }
}
