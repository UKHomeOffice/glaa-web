using System.Collections.Generic;
using System.Threading.Tasks;
using GLAA.ViewModels;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services.Admin
{
    public interface IAdminHomeViewModelBuilder : IViewModelBuilder<AdminHomeViewModel>
    {
    }

    public interface IAdminLicenceListViewModelBuilder : IViewModelBuilder<AdminLicenceListViewModel>
    {
        AdminLicenceListViewModel Build(LicenceOrApplication type);
    }

    public interface IAdminLicencePostDataHandler : IPostDataHandler<AdminLicenceViewModel>
    {
        void UpdateStatus(AdminLicenceViewModel model);
    }

    public interface IAdminUserListViewModelBuilder : IViewModelBuilder<AdminUserListViewModel>
    {
        Task<AdminUserListViewModel> Build();
    }

    public interface IAdminUserViewModelBuilder : IViewModelBuilder<AdminUserViewModel>
    {
        AdminUserViewModel Build(string id);

        IEnumerable<SelectListItem> GetRoles();
    }

    public interface IAdminStatusRecordsViewModelBuilder
    {
        AdminStatusDashboardViewModel Build();
        AdminStatusLicencesViewModel Build(int id);
    }
}
