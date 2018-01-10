using System;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GLAA.Services.Admin
{
    public class AdminUserViewModelBuilder : IAdminUserViewModelBuilder
    {
        private readonly UserManager<GLAAUser> um;
        private readonly IMapper mapper;
        private readonly IRoleRepository roleRepository;

        public AdminUserViewModelBuilder(IServiceProvider serviceProvider, IMapper mp, IRoleRepository rr)
        {
            um = serviceProvider.GetRequiredService<UserManager<GLAAUser>>();
            mapper = mp;
            roleRepository = rr;
        }

        public AdminUserViewModel New()
        {
            return new AdminUserViewModel();
        }

        public AdminUserViewModel Build(string id)
        {
            var user = um.FindByIdAsync(id).GetAwaiter().GetResult();

            var model = mapper.Map(user, New());
            var role = um.GetRolesAsync(user).GetAwaiter().GetResult().Single();
            model.Role = roleRepository.GetByName(role).ReadableName;

            return model;
        }
    }
}
