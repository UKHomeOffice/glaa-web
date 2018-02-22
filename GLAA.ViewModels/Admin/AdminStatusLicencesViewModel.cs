using System.Collections.Generic;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
