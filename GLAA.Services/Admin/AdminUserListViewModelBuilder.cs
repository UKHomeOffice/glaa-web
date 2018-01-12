using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

namespace GLAA.Services.Admin
{
    public class AdminUserListViewModelBuilder : IAdminUserListViewModelBuilder
    {
        private readonly UserManager<GLAAUser> userManager;
        private readonly RoleManager<GLAARole> roleManager;
        private readonly IMapper mapper;

        public AdminUserListViewModelBuilder(UserManager<GLAAUser> um, RoleManager<GLAARole> rm, IMapper mp)
        {
            userManager = um;
            roleManager = rm;
            mapper = mp;
        }

        public AdminUserListViewModel New()
        {
            return new AdminUserListViewModel();
        }

        public async Task<AdminUserListViewModel> Build()
        {
            var result = New();

            var roles = roleManager.Roles.Select(r => r.Name).OrderBy(role => role);

            foreach (var role in roles)
            {
                var users = await userManager.GetUsersInRoleAsync(role);
                result.Users.Add(role, mapper.Map(users.OrderBy(u => u.FullName), new List<AdminUserViewModel>()));
            }

            return result;
        }
    }
}
