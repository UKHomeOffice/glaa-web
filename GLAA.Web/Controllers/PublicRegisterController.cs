using System;
using System.Collections.Generic;
using System.Linq;
using GLAA.Services.PublicRegister;
using GLAA.ViewModels.PublicRegister;
using GLAA.Web.Attributes;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GLAA.Web.Controllers
{
    public class PublicRegisterController : Controller
    {
        private readonly ISessionHelper SessionHelper;
        private readonly IPublicRegisterViewModelBuilder publicRegisterViewModelBuilder;
        private static PublicRegisterSearchCriteria _currentPublicRegisterSearchCriteria;

        public PublicRegisterController(IPublicRegisterViewModelBuilder publicRegisterViewModelBuilder, ISessionHelper sessionHelper)
        {
            this.publicRegisterViewModelBuilder = publicRegisterViewModelBuilder;
            SessionHelper = sessionHelper;
        }

        [HttpPost]
        public IActionResult Index(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel, string submitButtonType)
        {
            SessionHelper.Set("publicRegisterSearchCriteria",publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria);
            SessionHelper.SetString("publicRegisterSearchCriteria_submitButtonType", submitButtonType);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var publicRegisterSearchCriteria = SessionHelper.Get<PublicRegisterSearchCriteria>("publicRegisterSearchCriteria");
            var submitButtonType = SessionHelper.GetString("publicRegisterSearchCriteria_submitButtonType");

            if (publicRegisterSearchCriteria != null && submitButtonType != null)
            {
                var publicRegisterLicenceListViewModel = publicRegisterViewModelBuilder.BuildEmptySearch();
                publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria = publicRegisterSearchCriteria;

                publicRegisterLicenceListViewModel = HandlePostActions(publicRegisterLicenceListViewModel, submitButtonType);

                return View(publicRegisterLicenceListViewModel);
            }
            else
            {
                var licences = publicRegisterViewModelBuilder.BuildEmptySearch();

                return View(licences);
            }
        }

        private PublicRegisterLicenceListViewModel HandlePostActions(
            PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel, string submit)
        {
            PublicRegisterSearchCriteria newPublicRegisterSearchCriteria = null;
            var searchViewModel = publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria;

            if (publicRegisterLicenceListViewModel.AvailableCountries == null)
                publicRegisterLicenceListViewModel.AvailableCountries =
                    publicRegisterViewModelBuilder.BuildAvailableCountries();

            switch (submit)
            {
                case "add":
                    newPublicRegisterSearchCriteria = AddCountry(publicRegisterLicenceListViewModel).PublicRegisterSearchCriteria;
                    break;
                case "search":
                    // we want to update teh 
                    _currentPublicRegisterSearchCriteria = searchViewModel;
                    searchViewModel.SearchActive = true;
                    break;
                default:
                    newPublicRegisterSearchCriteria =
                        RemoveCountry(publicRegisterLicenceListViewModel, submit).PublicRegisterSearchCriteria;
                    break;
            }

            if (searchViewModel.SearchActive)
            {
                publicRegisterLicenceListViewModel =
                    publicRegisterViewModelBuilder.BuildSearchForLicences(_currentPublicRegisterSearchCriteria);

                publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria =
                    newPublicRegisterSearchCriteria ?? searchViewModel;
            }
            else
                searchViewModel.SearchActive = false;

            //Ensure we remove any countries that are selected, from the list than are selectable.
            if (searchViewModel.CountriesSelected != null)
                publicRegisterLicenceListViewModel.AvailableCountries = publicRegisterLicenceListViewModel.AvailableCountries
                    .Where(x => !searchViewModel.CountriesSelected.Contains(x.Value)).ToList();
            else
                publicRegisterLicenceListViewModel.AvailableCountries = publicRegisterLicenceListViewModel.AvailableCountries.ToList();
            return publicRegisterLicenceListViewModel;
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
            if (publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria.CountriesSelected == null)
                publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria.CountriesSelected =
                    new List<string>();

            var countryAdded = publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria.CountryAdded;

            //add the country into the list of selected countries
            publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria.CountriesSelected.Add(
                publicRegisterLicenceListViewModel.AvailableCountries.FirstOrDefault(x => x.Value == countryAdded)?.Value);

            //remove them from the list of selectable countries
            foreach (var country in publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria.CountriesSelected.ToList())
            {
                publicRegisterLicenceListViewModel.AvailableCountries = RemoveFromList(
                    publicRegisterLicenceListViewModel.AvailableCountries, x => x.Value == country);
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
                if (publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria.CountriesSelected.Contains("UK"))
                    RemoveSelectedCountry(publicRegisterLicenceListViewModel, new SelectListItem { Text = "UK", Value = "UK" });
            }

            return publicRegisterLicenceListViewModel;
        }

        private void RemoveSelectedCountry(PublicRegisterLicenceListViewModel publicRegisterLicenceListViewModel,
            SelectListItem ukCountry)
        {
            //remove selected country
            publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria.CountriesSelected =
                RemoveFromList(
                    publicRegisterLicenceListViewModel.PublicRegisterSearchCriteria.CountriesSelected,
                    x => x == ukCountry.Value);

            //add selectable country
            if (publicRegisterLicenceListViewModel.AvailableCountries.All(x => x.Value != ukCountry.Value))
                publicRegisterLicenceListViewModel.AvailableCountries.Add(ukCountry);
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