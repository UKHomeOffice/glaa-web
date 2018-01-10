using System;
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
    public class AdminUserPostDataHandler : IAdminUserPostDataHandler
    {
        private readonly UserManager<GLAAUser> userManager;
        private readonly IMapper mapper;

        public AdminUserPostDataHandler(UserManager<GLAAUser> um, IMapper mp)
        {
            userManager = um;
            mapper = mp;
        }

        public string Insert(AdminUserViewModel model)
        {
            throw new NotImplementedException();
        }

        //TODO Make this async?
        public void Update(AdminUserViewModel model)
        {
            var user = userManager.FindByIdAsync(model.Id).GetAwaiter().GetResult();

            user = mapper.Map(model, user);
            var result = userManager.UpdateAsync(user).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                var currentRole = userManager.GetRolesAsync(user).GetAwaiter().GetResult();

                if (!currentRole.Single().Equals(model.Role))
                {
                    userManager.RemoveFromRoleAsync(user, currentRole.Single()).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(user, model.Role).GetAwaiter().GetResult();
                }
            }
        }
    }
}
