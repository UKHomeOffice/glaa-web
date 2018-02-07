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

        public AccountCreationViewModelBuilder(IMapper mp, UserManager<GLAAUser> um)
        {
            mapper = mp;
            userManager = um;
        }

        public SignUpViewModel Build(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new SignUpViewModel();
            }

            var user = userManager.FindCompleteUserByEmail(email);
            var model = mapper.Map<SignUpViewModel>(user);

            return model;
        }
    }
}
