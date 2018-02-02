using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.AccountCreation
{
    public interface IAccountCreationViewModelBuilder
    {
        EligibilityViewModel Build(string email);

        T Build<T>(string email) where T : new();
    }
}
