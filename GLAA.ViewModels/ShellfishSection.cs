using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels
{
    public abstract class ShellfishSection : IShellfishSection
    {
        public bool IsShellfish { get; set; }
        public bool CanView(OrganisationViewModel parent)
        {
            return parent.IsShellfish;
        }
    }
}
