using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GLAA.Services.AccountCreation
{
    public interface IAccountCreationPostDataHandler
    {
        Task<bool> ExistsAsync(string email);

        Task DeleteIfUnconfirmedAsync(string email);

        Task UpdateAsync<T>(string email, T model);

        Task UpdateAddressAsync(string email, AddressViewModel model);

        Task<bool> SetPasswordAsync(string email, string password);

        Task SendConfirmationAsync(string email, IUrlHelper url);
    }
}
