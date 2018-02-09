using System.Collections.Generic;

namespace GLAA.ViewModels.PublicRegister
{
    public class PublicRegisterSearchCriteria
    {
        public string BusinessName { get; set; }
        public string SupplierWho { get; set; }
        public List<string> CountriesSelected { get; set; }
        public List<byte> CountriesSelectedIds { get; set; }
        public string CountryAdded { get; set; }
        public string CountryRemoved { get; set; }
        public bool SearchActive { get; set; }
    }
}
