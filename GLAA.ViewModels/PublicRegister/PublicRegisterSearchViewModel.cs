using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.PublicRegister
{
    public class PublicRegisterSearchViewModel
    {
        public PublicRegisterSearchViewModel()
        {

        }

        public PublicRegisterSearchViewModel(List<SelectListItem> countries)
        {
            AvailableCountries = countries;
        }

        public string BusinessName { get; set; }
        public SelectListItem SupplierWho { get; set; }
        public List<SelectListItem> CountriesSelected { get; set; }
        public string CountryAdded { get; set; }
        public string CountryRemoved { get; set; }
        public List<SelectListItem> AvailableSuppliersWho => new List<SelectListItem>
            {
                new SelectListItem {Value = "supply", Text = "Supply"},
                new SelectListItem {Value = "arelocated", Text = "Are Located"}
            };
        public List<SelectListItem> AvailableCountries { get; set; }
    }
}
