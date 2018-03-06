using System;
using System.Collections.Generic;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GLAA.Services.AccountCreation
{
    public class AccountCreationPostDataHandler : IAccountCreationPostDataHandler
    {
        private readonly IMapper mapper;
        private readonly IEntityFrameworkRepository repository;
        private readonly UserManager<GLAAUser> userManager;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public AccountCreationPostDataHandler(IMapper mp, IEntityFrameworkRepository repo, UserManager<GLAAUser> um, IEmailService es, IConfiguration cs)
        {
            mapper = mp;
            repository = repo;
            userManager = um;
            emailService = es;
            configuration = cs;
        }

        public async Task<bool> ExistsAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user?.EmailConfirmed ?? false;
        }

        public async Task DeleteIfUnconfirmedAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null && !user.EmailConfirmed)
            {
                if (user.AddressId != null)
                {
                    repository.Delete<Address>(user.AddressId.Value);
                }

                await userManager.DeleteAsync(user);
            }
        }

        public async Task UpdateAsync<T>(string email, T model)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = mapper.Map<GLAAUser>(model);
                await userManager.CreateAsync(user);

                if(!await userManager.IsInRoleAsync(user, "Labour Provider"))
                {
                    await userManager.AddToRoleAsync(user, "Labour Provider");
                }
            }
            else
            {
                mapper.Map(model, user);
                await userManager.UpdateAsync(user);
            }
        }

        public async Task UpdateAddressAsync(string email, AddressViewModel model)
        {
            var user = userManager.FindCompleteUserByEmail(email);

            if (user.Address == null)
            {
                user.Address = repository.Create<Address>();
            }

            mapper.Map(model, user.Address);

            await userManager.UpdateAsync(user);
        }

        public async Task<bool> SetPasswordAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            var result = await userManager.AddPasswordAsync(user, password);

            return result.Succeeded;
        }

        public async Task SendConfirmationAsync(string email, IUrlHelper url)
        {
            if (string.IsNullOrEmpty(email) || url == null)
            {
                return;
            }

            var user = await userManager.FindByEmailAsync(email);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = url.Action("ConfirmEmail", "Account", new {userId = user.Id, code = token});

            var msg = new NotifyMailMessage(email, new Dictionary<string, dynamic>
            {
                {"full_name", user.FullName ?? "User"},
                {"confirm_email_link", callbackUrl}
            });

            var template = configuration.GetSection("GOVNotify:EmailTemplates")["ConfirmEmail"];

            emailService.Send(msg, template);
        }
    }
}
