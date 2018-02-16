using GLAA.Domain.Models;

namespace GLAA.Services
{
    public class LicenceCountryViewModel
    {
        public virtual Licence Licence { get; set; }
        public virtual Country Country { get; set; }
    }
}
