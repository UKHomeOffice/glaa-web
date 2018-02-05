using GLAA.ViewModels.PublicRegister;

namespace GLAA.Services.PublicRegister
{
    public interface IPublicRegisterViewModelBuilder
    {
        PublicRegisterLicenceListViewModel BuildAllLicences();
        PublicRegisterLicenceSummaryViewModel BuildLicence(int id);
    }
}