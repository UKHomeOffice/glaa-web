using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Services.AccountCreation
{
    public interface IAccountCreationPostDataHandler
    {
        bool Exists(string email);

        void DeleteIfUnconfirmed(string email);

        void Update<T>(string email, T model);

        void UpdateAddress(string email, AddressViewModel model);

        void SetPassword(string email, string password);

        void SendConfirmation(string email, IUrlHelper url);
    }
}
