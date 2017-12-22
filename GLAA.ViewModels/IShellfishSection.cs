using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels
{
    public interface IShellfishSection : ICanView<OrganisationViewModel>
    {
        bool IsShellfish { get; }
    }
}
