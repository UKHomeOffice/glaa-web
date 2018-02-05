using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.AccountCreation
{
    public interface IAccountCreationViewModelBuilder
    {
        EligibilityViewModel Build(string email);
    }
}
