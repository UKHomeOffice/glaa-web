using System;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GLAA.Services.Admin
{
    public class AdminUserViewModelBuilder : IAdminUserViewModelBuilder
    {
        private readonly UserManager<GLAAUser> um;
        private readonly IMapper mapper;

        public AdminUserViewModelBuilder(IServiceProvider serviceProvider, IMapper mp)
        {
            um = serviceProvider.GetRequiredService<UserManager<GLAAUser>>();
            mapper = mp;
        }

        public AdminUserViewModel New()
        {
            return new AdminUserViewModel();
        }

        public AdminUserViewModel Build(string id)
        {
            var user = um.FindByIdAsync(id).GetAwaiter().GetResult();

            var model = mapper.Map(user, New());
            model.Roles = um.GetRolesAsync(user).GetAwaiter().GetResult();

            return model;
        }
    }
}
