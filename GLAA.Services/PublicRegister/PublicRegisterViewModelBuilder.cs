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

        private readonly List<SelectListItem> _ukCountries;
        public List<SelectListItem> UkCountries => new List<SelectListItem>(_ukCountries);

        public PublicRegisterViewModelBuilder(
            IMapper mapper,
            IEntityFrameworkRepository repository,
            ILicenceRepository licenceRepository)
        {
            _mapper = mapper;
            _licenceRepository = licenceRepository;

            _ukCountries = repository.GetAll<Country>().Select(x =>
                new SelectListItem { Value = x.Name, Text = x.Name }).ToList();
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

            switch (Enum.Parse<SupplierWho>(publicRegisterSearchCriteria.SupplierWho))
            {
                case SupplierWho.Supply:
                    if (publicRegisterSearchCriteria.CountriesSelected.Any(x => x == "UK"))
                        licences = licences.Where(x =>
                            ukCountryNames.Any(y => x.OperatingCountries.Any(z => z.WorkerCountry.Name == y)));
                    else if (publicRegisterSearchCriteria.CountriesSelected.Any(x => x == "Outside UK"))
                        licences = licences.Where(x =>
                            ukCountryNames.Any(y => x.OperatingCountries.All(z => z.WorkerCountry.Name != y)));
                    else
                        licences = licences.Where(x => x.OperatingCountries.Any(y =>
                            publicRegisterSearchCriteria.CountriesSelected.Any(z => y.WorkerCountry.Name.Contains(z))));

                    break;
                case SupplierWho.AreLocated:
                    if (publicRegisterSearchCriteria.CountriesSelected.Any(x => x == "UK"))
                        licences = licences.Where(x => x.Address != null && ukCountryNames.Any(y => x.Address.Country.Name.Contains(y)));
                    else if (publicRegisterSearchCriteria.CountriesSelected.Any(x => x == "Outside UK"))
                        licences = licences.Where(x => x.Address != null && ukCountryNames.Any(y => x.Address.NonUK));
                    else
                        licences = licences.Where(x => x.Address != null && publicRegisterSearchCriteria.CountriesSelected.Any(y => y == x.Address.Country.Name));

                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(publicRegisterSearchCriteria.SupplierWho));
            }

            return new PublicRegisterLicenceListViewModel
            {
                Licences = licences.Distinct().Select(BuildSummary),
                PublicRegisterSearchCriteria = publicRegisterSearchCriteria,
                AvailableCountries = BuildAvailableCountries()
            };
        }

        public PublicRegisterLicenceDetailViewModel BuildLicence(int id)
        {
            var licence = _licenceRepository.GetById(id);

            return LicenceRepository.GetLatestStatus(licence).Status.IsLicence ? BuildDetail(licence) : null;
        }

        private PublicRegisterLicenceDetailViewModel BuildDetail(Licence licence)
        {
            return _mapper.Map<PublicRegisterLicenceDetailViewModel>(licence);
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
