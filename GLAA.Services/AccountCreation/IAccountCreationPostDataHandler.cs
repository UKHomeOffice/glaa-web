using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.AccountCreation
{
    public interface IAccountCreationPostDataHandler
    {
        void Update<T>(string email, T model);

        void UpdateAddress(string email, AddressViewModel model);

        void SetPassword(string email, string password);
    }
}
