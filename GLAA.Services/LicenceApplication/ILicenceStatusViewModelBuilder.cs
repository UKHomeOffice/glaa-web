using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.LicenceApplication
{
    public interface ILicenceStatusViewModelBuilder : IViewModelBuilder<LicenceStatusViewModel>
    {
        LicenceStatusViewModel BuildRandomStatus();

        LicenceStatusViewModel BuildLatestStatus(int licenceId);
    }
}
