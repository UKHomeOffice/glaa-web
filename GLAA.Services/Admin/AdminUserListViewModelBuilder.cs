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
        private readonly UserManager<GLAAUser> um;
        private readonly RoleManager<GLAARole> rm;
        private readonly IMapper mapper;

        public AdminUserListViewModelBuilder(IServiceProvider serviceProvider, IMapper mp, RoleManager<GLAARole> rm, UserManager<GLAAUser> um)
        {
            this.um = um;
            this.rm = rm;
            mapper = mp;
        }

        public AdminUserListViewModel New()
        {
            return new AdminUserListViewModel();
        }

        public async Task<AdminUserListViewModel> Build()
        {
            var result = New();

            var roles = rm.Roles.Select(r => r.Name).OrderBy(role => role);

            foreach (var role in roles)
            {
                var users = await um.GetUsersInRoleAsync(role);
                result.Users.Add(role, mapper.Map(users.OrderBy(u => u.FullName), new List<AdminUserViewModel>()));
            }

            return result;
        }
    }
}
