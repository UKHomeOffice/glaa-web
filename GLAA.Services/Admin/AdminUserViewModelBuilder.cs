using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services.Admin
{
    public class AdminUserViewModelBuilder : IAdminUserViewModelBuilder
    {
        private readonly UserManager<GLAAUser> userManager;
        private readonly RoleManager<GLAARole> roleManager;
        private readonly IMapper mapper;

        public AdminUserViewModelBuilder(UserManager<GLAAUser> um, RoleManager<GLAARole> rm, IMapper mp)
        {
            userManager = um;
            roleManager = rm;
            mapper = mp;
        }

        public AdminUserViewModel New()
        {
            return new AdminUserViewModel();
        }

        public AdminUserViewModel Build(string id)
        {
            var user = userManager.FindByIdAsync(id).GetAwaiter().GetResult();

            var model = mapper.Map(user, New());
            var role = userManager.GetRolesAsync(user).GetAwaiter().GetResult().Single();
            model.AvailableRoles = roleManager.Roles.Select(r =>
                new SelectListItem {Value = r.Name, Text = r.Name, Selected = r.Name == role});

            return model;
        }
    }
}
