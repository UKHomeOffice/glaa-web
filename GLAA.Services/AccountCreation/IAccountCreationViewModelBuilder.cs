using GLAA.ViewModels;

namespace GLAA.Services.AccountCreation
{
    public interface IAccountCreationViewModelBuilder
    {
        SignUpViewModel Build(string email);
    }
}
