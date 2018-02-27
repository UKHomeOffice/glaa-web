using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.PublicRegister;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services.PublicRegister
{
    public class PublicRegisterViewModelBuilder : IPublicRegisterViewModelBuilder
    {
        private readonly ILicenceRepository _licenceRepository;
        private readonly IMapper _mapper;
        private readonly IReferenceDataProvider _referenceDataProvider;

        private readonly List<SelectListItem> _ukCountries;
        public List<SelectListItem> UkCountries => new List<SelectListItem>(_ukCountries);

        public PublicRegisterViewModelBuilder(
            IMapper mapper,
            IEntityFrameworkRepository repository,
            ILicenceRepository licenceRepository,
            IReferenceDataProvider referenceDataProvider)
        {
            _mapper = mapper;
            _licenceRepository = licenceRepository;
            _referenceDataProvider = referenceDataProvider;

            _ukCountries = repository.GetAll<Country>().Select(x =>
                new SelectListItem { Value = x.Name, Text = x.Name }).OrderBy(y => y.Text).ToList();
        }

        public PublicRegisterLicenceListViewModel BuildAllLicences()
        {
            return new PublicRegisterLicenceListViewModel
            {
                //We only want licences with status that are allowed to be shown using the "ShowInPublicRegister" field.
                Licences = _licenceRepository.GetAllLicences().Select(BuildSummary),
                PublicRegisterSearchCriteria = BuildPublicRegisterSearchViewModel(),
                AvailableCountries = BuildAvailableCountries()
            };
        }

        public PublicRegisterLicenceListViewModel BuildEmptySearch()
        {
            return new PublicRegisterLicenceListViewModel
            {
                Licences = new List<PublicRegisterLicenceSummaryViewModel>(),
                PublicRegisterSearchCriteria = BuildPublicRegisterSearchViewModel(),
                AvailableCountries = BuildAvailableCountries()
            };
        }

        public PublicRegisterLicenceListViewModel BuildSearchForLicences(
            PublicRegisterSearchCriteria publicRegisterSearchCriteria)
        {
            var ukCountryNames = UkCountries.Select(x => x.Text);

            var licences = _licenceRepository.GetAllLicencesForPublicRegister()
                .Where(x => string.IsNullOrWhiteSpace(publicRegisterSearchCriteria.BusinessName)
                            || x.BusinessName.Contains(publicRegisterSearchCriteria.BusinessName));

            var countries = _referenceDataProvider.GetCountries();

            if (publicRegisterSearchCriteria.CountriesSelected != null && publicRegisterSearchCriteria.CountriesSelected.Count > 0) { 
                switch (Enum.Parse<SupplierWho>(publicRegisterSearchCriteria.SupplierWho))
                {
                    case SupplierWho.Supply:
                        if (publicRegisterSearchCriteria.CountriesSelected.Any(x => x == "UK"))
                            licences = licences.Where(x =>
                                ukCountryNames.Any(y => x.OperatingCountries.Any(z => 
                                countries.Single(c => int.Parse(c.Value) == z.WorkerCountryId).Text == y)));
                        else if (publicRegisterSearchCriteria.CountriesSelected.Any(x => x == "Outside UK"))
                            licences = licences.Where(x =>
                                ukCountryNames.Any(y => x.OperatingCountries.All(z => 
                                countries.Single(c => int.Parse(c.Value) == z.WorkerCountryId).Text  != y)));
                        else
                            licences = licences.Where(x => x.OperatingCountries.Any(y => y != null &&
                                publicRegisterSearchCriteria.CountriesSelected.Any(z => 
                                countries.Single(c => 
                                int.Parse(c.Value) == y.WorkerCountryId).Text.Contains(z))));
                    
                        break;
                    case SupplierWho.AreLocated:
                        if (publicRegisterSearchCriteria.CountriesSelected.Any(x => x == "UK"))
                            licences = licences.Where(x =>
                                x.Address != null && ukCountryNames.Any(y =>
                                countries.Single(c => int.Parse(c.Value) == x.Address.CountryId).Text.Contains(y)));
                        else if (publicRegisterSearchCriteria.CountriesSelected.Any(x => x == "Outside UK"))
                            licences = licences.Where(x =>
                                x.Address != null && ukCountryNames.Any(y => !x.Address.Country.IsUk));
                        else
                            licences = licences.Where(x =>
                                x.Address != null &&
                                publicRegisterSearchCriteria.CountriesSelected.Any(y =>
                                    y == countries.Single(c => int.Parse(c.Value) == x.Address.CountryId).Text));

                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(publicRegisterSearchCriteria.SupplierWho));
                }   
            }

            return new PublicRegisterLicenceListViewModel
            {
                Licences = licences.Distinct().Select(BuildSummary),
                PublicRegisterSearchCriteria = publicRegisterSearchCriteria,
                AvailableCountries = BuildAvailableCountries(),
                Countries = countries,
                Counties = _referenceDataProvider.GetCounties()
            };
        }

        public PublicRegisterLicenceDetailViewModel BuildLicence(int id)
        {
            var licence = _licenceRepository.GetById(id);

            return LicenceRepository.GetLatestStatus(licence).Status.IsLicence ? BuildDetail(licence) : null;
        }

        private PublicRegisterLicenceDetailViewModel BuildDetail(Licence licence)
        {
            var publicRegisterLicenceDetailViewModel = _mapper.Map<PublicRegisterLicenceDetailViewModel>(licence);

            if (publicRegisterLicenceDetailViewModel.Address != null)
                publicRegisterLicenceDetailViewModel.Address.Countries = _referenceDataProvider.GetCountries();

            return publicRegisterLicenceDetailViewModel;
        }

        private PublicRegisterLicenceSummaryViewModel BuildSummary(Licence licence)
        {
            return _mapper.Map<PublicRegisterLicenceSummaryViewModel>(licence);
        }

        public PublicRegisterSearchCriteria BuildPublicRegisterSearchViewModel()
        {
            return new PublicRegisterSearchCriteria
            {
                CountryAdded = "",
                SupplierWho = "Supply",
                BusinessName = "",
                CountriesSelected = new List<string>(),
                CountryRemoved = "",
            };
        }

        public List<SelectListItem> BuildAvailableCountries()
        {
            var availableCountries = UkCountries;

            availableCountries.Add(new SelectListItem { Value = "UK", Text = "UK" });
            availableCountries.Add(new SelectListItem { Value = "Outside UK", Text = "Outside UK" });

            return availableCountries;
        }
    }
}
