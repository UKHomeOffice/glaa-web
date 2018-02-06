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

        public bool Exists(string email)
        {
            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            return user?.EmailConfirmed ?? false;
        }

        public void DeleteIfUnconfirmed(string email)
        {
            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            if (!user.EmailConfirmed)
            {
                if (user.AddressId != null)
                {
                    repository.Delete<Address>(user.AddressId.Value);
                }

                userManager.DeleteAsync(user).GetAwaiter().GetResult();
            }
        }

        public void Update<T>(string email, T model)
        {
            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            if (user == null)
            {
                user = mapper.Map<GLAAUser>(model);
                userManager.CreateAsync(user).GetAwaiter().GetResult();
            }
            else
            {
                mapper.Map(model, user);
                userManager.UpdateAsync(user).GetAwaiter().GetResult();
            }
        }

        public void UpdateAddress(string email, AddressViewModel model)
        {
            var user = userManager.FindCompleteUserByEmail(email);

            if (user.Address == null)
            {
                user.Address = repository.Create<Address>();
            }

            mapper.Map(model, user.Address);

            userManager.UpdateAsync(user).GetAwaiter().GetResult();
        }

        public void SetPassword(string email, string password)
        {
            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            userManager.AddPasswordAsync(user, password).GetAwaiter().GetResult();
        }

        public void SendConfirmation(string email, IUrlHelper url)
        {
            if (string.IsNullOrEmpty(email) || url == null)
            {
                return;
            }

            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            var token = userManager.GenerateEmailConfirmationTokenAsync(user).GetAwaiter().GetResult();
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
