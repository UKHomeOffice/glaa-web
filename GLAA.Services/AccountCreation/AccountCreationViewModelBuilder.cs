using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GLAA.Services.AccountCreation
{
    public class AccountCreationViewModelBuilder : IAccountCreationViewModelBuilder
    {
        private readonly IMapper mapper;
        private readonly UserManager<GLAAUser> userManager;
        private readonly IReferenceDataProvider referenceDataProvider;

        public AccountCreationViewModelBuilder(IMapper mp, UserManager<GLAAUser> um, IReferenceDataProvider rdp)
        {
            mapper = mp;
            userManager = um;
            referenceDataProvider = rdp;
        }

        public SignUpViewModel Build(string email)
        {
            var model = new SignUpViewModel
            {
                Countries = referenceDataProvider.GetCountries(),
                Counties = referenceDataProvider.GetCounties()
            };
            
            var user = userManager.FindCompleteUserByEmail(email);

            if (user != null)
            {
                model = mapper.Map(user, model);
            }

            return model;
        }
    }
}
