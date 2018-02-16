using System.Collections.Generic;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.Admin
{
    public class AdminStatusLicencesViewModel
    {
        public AdminStatusLicencesViewModel()
        {
            LicenceApplicationViewModels = new List<LicenceApplicationViewModel>();
        }

        public LicenceStatusViewModel LicenceStatusViewModel { get; set; }

        public List<LicenceApplicationViewModel> LicenceApplicationViewModels { get; set; }
    }
}
