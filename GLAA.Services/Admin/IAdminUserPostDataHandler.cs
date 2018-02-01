using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Services.Admin
{
    public interface IAdminUserPostDataHandler
    {
        string Insert(AdminUserViewModel model, IUrlHelper url, string scheme);

        void Update(AdminUserViewModel model);

        bool Exists(AdminUserViewModel model);
    }
}
