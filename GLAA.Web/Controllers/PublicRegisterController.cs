using System.Collections.Generic;
using System.Linq;
using GLAA.Services.PublicRegister;
using GLAA.ViewModels.PublicRegister;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Web.Controllers
{
    public class PublicRegisterController : Controller
    {
        private readonly IPublicRegisterViewModelBuilder publicRegisterViewModelBuilder;

        public PublicRegisterController(IPublicRegisterViewModelBuilder publicRegisterViewModelBuilder)
        {
            this.publicRegisterViewModelBuilder = publicRegisterViewModelBuilder;
        }

        [HttpPost]
        public IActionResult Index(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel, string submit)
        {
            if (publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.AvailableCountries == null)
                publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.AvailableCountries =
                    publicRegisterViewModelBuilder.BuildAvailableCountries();

            switch (submit)
            {
                case "add":
                    publicRegisterLicenceListViewModel = addCountry(publicRegisterLicenceListViewModel);
                    break;
                case "remove":
                    publicRegisterLicenceListViewModel = removeCountry(publicRegisterLicenceListViewModel);
                    break;
                default:
                    publicRegisterLicenceListViewModel = publicRegisterViewModelBuilder.BuildSearchForLicences(publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel);
                    break;
            }

            return View(publicRegisterLicenceListViewModel);
        }

        private PublicRegisterLicenceListViewModel addCountry(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel)
        {
            if (publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected == null)
                publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected = new List<SelectListItem>();

            publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected.Add(
                publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.AvailableCountries
                    .FirstOrDefault(x =>
                        x.Value == publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel
                            .CountryAdded));

            return publicRegisterLicenceListViewModel;
        }

        private PublicRegisterLicenceListViewModel removeCountry(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel)
        {
            var countryToRemoveItem = publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected.FirstOrDefault(
                                    x => x.Value == publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel
                                             .CountryRemoved);

            publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected.Remove(
                countryToRemoveItem);

            return publicRegisterLicenceListViewModel;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var licences = publicRegisterViewModelBuilder.BuildAllLicences();

            return View(licences);
        }

        [HttpGet]
        [Route("PublicRegisterProfile/License/{id}")]
        public IActionResult Licence(int id)
        {
            var licence = publicRegisterViewModelBuilder.BuildLicence(id);

            return View(licence);
        }

    }
}