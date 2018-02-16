using System.Collections.Generic;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.Admin
{
    public class AdminStatusDashboardViewModel
    {
        public AdminStatusDashboardViewModel()
        {
            AdminStatusCountViewModels = new List<AdminStatusCountViewModel>();
        }

        public List<AdminStatusCountViewModel> AdminStatusCountViewModels { get; set; }
    }
}