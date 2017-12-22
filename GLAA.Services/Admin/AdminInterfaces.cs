using GLAA.ViewModels;
using GLAA.ViewModels.Admin;

namespace GLAA.Services.Admin
{
    public interface IAdminHomeViewModelBuilder : IViewModelBuilder<AdminHomeViewModel>
    {
    }

    public interface IAdminLicenceListViewModelBuilder : IViewModelBuilder<AdminLicenceListViewModel>
    {
        AdminLicenceListViewModel Build(LicenceOrApplication type);
    }

    public interface IAdminLicenceViewModelBuilder : IViewModelBuilder<AdminLicenceViewModel>
    {
        AdminLicenceViewModel Build(int id);
    }

    public interface IAdminLicencePostDataHandler : IPostDataHandler<AdminLicenceViewModel>
    {
        void UpdateStatus(AdminLicenceViewModel model);
    }
}
