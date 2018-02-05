using System;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Identity;

namespace GLAA.Services.AccountCreation
{
    public class AccountCreationViewModelBuilder : IAccountCreationViewModelBuilder
    {
        private readonly IMapper mapper;
        private readonly UserManager<GLAAUser> userManager;

        public AccountCreationViewModelBuilder(IMapper mp, UserManager<GLAAUser> um)
        {
            mapper = mp;
            userManager = um;
        }

        public EligibilityViewModel Build(string email)
        {
            return Build<EligibilityViewModel>(email);
        }

        public T Build<T>(string email) where T : new()
        {
            if (string.IsNullOrEmpty(email))
            {
                return new T();
            }

            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            var model = mapper.Map<T>(user);
            return model;
        }
    }
}
