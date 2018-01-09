using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GLAA.Services.Admin
{
    public class AdminUserListViewModelBuilder : IAdminUserListViewModelBuilder
    {
        private readonly UserManager<GLAAUser> um;
        private readonly RoleManager<IdentityRole> rm;
        private readonly IMapper mapper;

        public AdminUserListViewModelBuilder(IServiceProvider serviceProvider, IMapper mp)
        {
            um = serviceProvider.GetRequiredService<UserManager<GLAAUser>>();
            rm = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            mapper = mp;
        }

        public AdminUserListViewModel New()
        {
            return new AdminUserListViewModel();
        }

        public async Task<AdminUserListViewModel> Build()
        {
            var result = New();

            var roles = rm.Roles.Select(r => r.Name);

            foreach (var role in roles)
            {
                var users = await um.GetUsersInRoleAsync(role);
                result.Users.Add(role, mapper.Map(users.OrderBy(u => u.FullName), new List<AdminUserViewModel>()));
            }

            return result;
        }
    }
}
