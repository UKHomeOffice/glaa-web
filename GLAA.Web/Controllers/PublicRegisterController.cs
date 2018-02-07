using System;
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
        private static PublicRegisterSearchViewModel currentSearchViewModel;

        public PublicRegisterController(IPublicRegisterViewModelBuilder publicRegisterViewModelBuilder)
        {
            this.publicRegisterViewModelBuilder = publicRegisterViewModelBuilder;
        }

        [HttpPost]
        public IActionResult Index(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel, string submit)
        {
            PublicRegisterSearchViewModel newSearchViewModel = null;
            var searchViewModel = publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel;

            if (searchViewModel.AvailableCountries == null)
                searchViewModel.AvailableCountries =
                    publicRegisterViewModelBuilder.BuildAvailableCountries();

            switch (submit)
            {
                case "add":
                    newSearchViewModel = AddCountry(publicRegisterLicenceListViewModel).PublicRegisterSearchViewModel;
                    break;
                case "search":
                    // we want to update teh 
                    currentSearchViewModel = searchViewModel;
                    searchViewModel.SearchActive = true;
                    break;
                default:
                    newSearchViewModel = RemoveCountry(publicRegisterLicenceListViewModel, submit).PublicRegisterSearchViewModel;
                    break;
            }

            if (searchViewModel.SearchActive &&
                (searchViewModel.CountriesSelected != null && searchViewModel.CountriesSelected.Count > 0 ||
                 !string.IsNullOrWhiteSpace(searchViewModel.BusinessName)))
            {
                publicRegisterLicenceListViewModel =
                    publicRegisterViewModelBuilder.BuildSearchForLicences(currentSearchViewModel);

                publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel =
                    newSearchViewModel ?? searchViewModel;
            }
            else
                searchViewModel.SearchActive = false;

            //Ensure we remove any countries that are selected, from the list than are selectable.
            if (searchViewModel.CountriesSelected != null)
                searchViewModel.AvailableCountries = searchViewModel.AvailableCountries
                    .Where(x => !searchViewModel.CountriesSelected.Contains(x.Value)).ToList();
            else
                searchViewModel.AvailableCountries = searchViewModel.AvailableCountries.ToList();

            return View(publicRegisterLicenceListViewModel);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var licences = publicRegisterViewModelBuilder.BuildEmptySearch();

            return View(licences);
        }

        [HttpGet]
        [Route("PublicRegisterProfile/License/{id}")]
        public IActionResult Licence(int id)
        {
            var licence = publicRegisterViewModelBuilder.BuildLicence(id);

            return View(licence);
        }

        private PublicRegisterLicenceListViewModel AddCountry(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel)
        {
            if (publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected == null)
                publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected =
                    new List<string>();

            var countryAdded = publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountryAdded;

            //add the country into the list of selected countries
            publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected.Add(
                publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.AvailableCountries
                    .FirstOrDefault(x => x.Value == countryAdded)?.Value);

            //remove them from the list of selectable countries
            foreach (var country in publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected.ToList())
            {
                publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.AvailableCountries = RemoveFromList(
                    publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.AvailableCountries,
                    x => x.Value == country);
            }

            publicRegisterLicenceListViewModel = HandleUkSelected(publicRegisterLicenceListViewModel, countryAdded);

            return publicRegisterLicenceListViewModel;
        }

        private PublicRegisterLicenceListViewModel HandleUkSelected(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel, string countryAdded)
        {
            if (countryAdded == "UK")
            {
                //If UK is selected, then we want to remove each of the UK regions from the selected collection, and add them back in to the possible selectable collection
                foreach (var ukCountry in publicRegisterViewModelBuilder.UkCountries)
                    RemoveSelectedCountry(publicRegisterLicenceListViewModel, ukCountry);
            }
            else if (countryAdded != "Outside UK")
            {
                //if any other country than the UK, or Outside UK is added, then we want to remove the "UK" from the selected countries.
                if (publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected.Contains("UK"))
                    RemoveSelectedCountry(publicRegisterLicenceListViewModel, new SelectListItem { Text = "UK", Value = "UK" });
            }

            return publicRegisterLicenceListViewModel;
        }

        private void RemoveSelectedCountry(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel,
            SelectListItem ukCountry)
        {
            //remove selected country
            publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected =
                RemoveFromList(
                    publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.CountriesSelected,
                    x => x == ukCountry.Value);

            //add selectable country
            if (publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.AvailableCountries.All(x => x.Value != ukCountry.Value))
                publicRegisterLicenceListViewModel.PublicRegisterSearchViewModel.AvailableCountries.Add(ukCountry);
        }

        private PublicRegisterLicenceListViewModel RemoveCountry(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel, string countryToRemove)
        {
            RemoveSelectedCountry(publicRegisterLicenceListViewModel, new SelectListItem { Text = countryToRemove, Value = countryToRemove });

            return publicRegisterLicenceListViewModel;
        }

        private List<T> RemoveFromList<T>(List<T> list, Func<T, bool> expression)
        {
            var listItem = list.FirstOrDefault(expression);

            if (listItem != null)
                list.Remove(listItem);

            return list;
        }
    }
}