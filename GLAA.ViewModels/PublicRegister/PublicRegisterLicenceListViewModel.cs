using System.Collections.Generic;

namespace GLAA.ViewModels.PublicRegister
{
    public class PublicRegisterLicenceListViewModel
    {
        public string Title { get; set; }
        public IEnumerable<PublicRegisterLicenceSummaryViewModel> Licences { get; set; }
        public PublicRegisterSearchViewModel PublicRegisterSearchViewModel { get; set; }
    }
}
