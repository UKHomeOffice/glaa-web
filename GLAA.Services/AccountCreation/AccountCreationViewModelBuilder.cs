using System;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GLAA.Services.AccountCreation
{
    public class AccountCreationViewModelBuilder : IAccountCreationViewModelBuilder
    {
        private readonly IMapper mapper;
        private readonly UserManager<GLAAUser> userManager;
        private readonly IEntityFrameworkRepository repository;

        public AccountCreationViewModelBuilder(IMapper mp, UserManager<GLAAUser> um, IEntityFrameworkRepository repo)
        {
            mapper = mp;
            userManager = um;
            repository = repo;
        }

        public EligibilityViewModel Build(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new EligibilityViewModel();
            }

            var user = userManager.FindCompleteUserByEmail(email);
            var model = mapper.Map<EligibilityViewModel>(user);

            return model;
        }
    }
}
