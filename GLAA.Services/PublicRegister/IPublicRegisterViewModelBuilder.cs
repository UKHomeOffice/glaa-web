using System.Collections.Generic;
using GLAA.ViewModels.PublicRegister;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services.PublicRegister
{
    public interface IPublicRegisterViewModelBuilder
    {
        PublicRegisterLicenceListViewModel BuildAllLicences();
        PublicRegisterLicenceSummaryViewModel BuildLicence(int id);
        PublicRegisterLicenceListViewModel BuildSearchForLicences(PublicRegisterSearchViewModel publicRegisterSearchViewModel);
        List<SelectListItem> BuildAvailableCountries();
    }
}