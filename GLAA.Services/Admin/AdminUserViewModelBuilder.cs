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
        private readonly IMapper mapper;
        private readonly IRoleRepository roleRepository;

        public AdminUserViewModelBuilder(UserManager<GLAAUser> um, IMapper mp, IRoleRepository rr)
        {
            userManager = um;
            mapper = mp;
            roleRepository = rr;
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
            model.Role = roleRepository.GetByName(role).ReadableName;
            model.AvailableRoles = roleRepository.GetAll<RoleDescription>().Select(r =>
                new SelectListItem {Value = r.Name, Text = r.ReadableName, Selected = r.Name == role});

            return model;
        }
    }
}
