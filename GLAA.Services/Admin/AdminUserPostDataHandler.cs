using System;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

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
            var existing = userManager.FindByEmailAsync(model.Email).GetAwaiter().GetResult();
            
            if (existing != null)
            {
                return null;
            }

            var user = mapper.Map<GLAAUser>(model);
            userManager.CreateAsync(user).GetAwaiter().GetResult();

            userManager.AddToRoleAsync(user, model.Role).GetAwaiter().GetResult();

            return user.Id;
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

        public bool Exists(AdminUserViewModel model)
        {
            return userManager.FindByEmailAsync(model.Email) != null;
        }
    }
}
