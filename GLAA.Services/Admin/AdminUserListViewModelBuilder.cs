using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GLAA.Services.Admin
{
    public class AdminUserListViewModelBuilder : IAdminUserListViewModelBuilder
    {
        private readonly UserManager<GLAAUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;

        public AdminUserListViewModelBuilder(UserManager<GLAAUser> um, RoleManager<IdentityRole> rm, IMapper mp,
            IRoleRepository rr)
        {
            userManager = um;
            roleManager = rm;
            mapper = mp;
            roleRepository = rr;
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
                var roleDescription = roleRepository.GetByName(role);
                var users = await userManager.GetUsersInRoleAsync(role);
                result.Users.Add(roleDescription.ReadableName, mapper.Map(users.OrderBy(u => u.FullName), new List<AdminUserViewModel>()));
            }

            return result;
        }
    }
}
