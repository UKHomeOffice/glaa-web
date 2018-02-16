using System.Collections.Generic;
using GLAA.ViewModels.PublicRegister;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services.PublicRegister
{
    public interface IPublicRegisterViewModelBuilder
    {
        List<SelectListItem> UkCountries { get; }
        PublicRegisterLicenceListViewModel BuildAllLicences();
        PublicRegisterLicenceListViewModel BuildEmptySearch();
        PublicRegisterLicenceDetailViewModel BuildLicence(int id);
        PublicRegisterLicenceListViewModel BuildSearchForLicences(PublicRegisterSearchCriteria publicRegisterSearchCriteria);
        List<SelectListItem> BuildAvailableCountries();
    }
}