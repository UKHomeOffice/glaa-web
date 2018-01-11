using GLAA.ViewModels.Admin;

namespace GLAA.Services.Admin
{
    public interface IAdminUserPostDataHandler
    {
        string Insert(AdminUserViewModel model);

        void Update(AdminUserViewModel model);

        bool Exists(AdminUserViewModel model);
    }
}
