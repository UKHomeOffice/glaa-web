using GLAA.Domain.Models;

namespace GLAA.ViewModels
{
    public class LicenceCountryViewModel
    {
        public int LicenceId { get; set; }
        public virtual Licence Licence { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
