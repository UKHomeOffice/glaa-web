using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GLAA.Services.Admin
{
    public class AdminUserPostDataHandler : IAdminUserPostDataHandler
    {
        private readonly UserManager<GLAAUser> userManager;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public AdminUserPostDataHandler(UserManager<GLAAUser> um, IMapper mp, IEmailService es, IConfiguration cs)
        {
            userManager = um;
            mapper = mp;
            emailService = es;
            configuration = cs;
        }

        public string Insert(AdminUserViewModel model, IUrlHelper url, string scheme)
        {
            var existing = userManager.FindByEmailAsync(model.Email).GetAwaiter().GetResult();
            
            if (existing != null)
            {
                return null;
            }

            var user = mapper.Map<GLAAUser>(model);
            userManager.CreateAsync(user).GetAwaiter().GetResult();

            userManager.AddToRoleAsync(user, model.Role).GetAwaiter().GetResult();

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = userManager.GeneratePasswordResetTokenAsync(user).GetAwaiter().GetResult();
            var callbackUrl = url.Action("ResetPassword", "AccountController", new { userId = user.Id, code = code }, scheme);

            var msg = new NotifyMailMessage(model.Email, new Dictionary<string, dynamic> {
                { "full_name", user.FullName ?? "User" },
                { "reset_password_link", callbackUrl }
            });

            var template = configuration.GetSection("GOVNotify:EmailTemplates")["ResetPassword"];

            var success = emailService.Send(msg, template);

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
            return userManager.FindByEmailAsync(model.Email).GetAwaiter().GetResult() != null;
        }
    }
}
