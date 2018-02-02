using System;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Identity;

namespace GLAA.Services.AccountCreation
{
    public class AccountCreationPostDataHandler : IAccountCreationPostDataHandler
    {
        private readonly IMapper mapper;
        private readonly IEntityFrameworkRepository repository;
        private readonly UserManager<GLAAUser> userManager;

        public AccountCreationPostDataHandler(IMapper mp, IEntityFrameworkRepository repo, UserManager<GLAAUser> um)
        {
            mapper = mp;
            repository = repo;
            userManager = um;
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
            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            if (user.Address == null)
            {
                user.Address = repository.Create<Address>();
            }

            mapper.Map(model, user.Address);

            userManager.UpdateAsync(user).GetAwaiter().GetResult();
        }
    }
}
