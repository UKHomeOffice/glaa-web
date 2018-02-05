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
        private readonly IEntityFrameworkRepository repository;
        private readonly ILicenceRepository licenceRepository;
        private readonly IMapper mapper;

        private List<SelectListItem> UkCountries { get; }

        public PublicRegisterViewModelBuilder(
            IMapper mapper,
            IEntityFrameworkRepository repository,
            ILicenceRepository licenceRepository)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.licenceRepository = licenceRepository;

            UkCountries = repository.GetAll<Country>().Select(x =>
                new SelectListItem { Value = x.Name, Text = x.Name }).ToList();
        }

        public PublicRegisterLicenceListViewModel BuildAllLicences()
        {
            return new PublicRegisterLicenceListViewModel
            {
                Title = "Public Register",
                //We only want licences with status that are allowed to be shown using the "ShowInPublicRegister" field.
                Licences = licenceRepository.GetAllLicences()
                    .Where(x => GetLatestStatus(x).Status.ShowInPublicRegister)
                    .Select(BuildSummary),
                PublicRegisterSearchViewModel = BuildPublicRegisterSearchViewModel()
            };
        }

        public PublicRegisterLicenceListViewModel BuildSearchForLicences(
            PublicRegisterSearchViewModel publicRegisterSearchViewModel)
        {
            var ukCountryNames = UkCountries.Select(x => x.Text);

            var licences = licenceRepository.GetAllLicences()
                .Where(x => GetLatestStatus(x).Status.ShowInPublicRegister
                            && (x.BusinessName.Contains(publicRegisterSearchViewModel.BusinessName) ||
                                string.IsNullOrWhiteSpace(publicRegisterSearchViewModel.BusinessName)));

            switch (publicRegisterSearchViewModel.SupplierWho.Value)
            {
                case "supply":
                    if (publicRegisterSearchViewModel.CountriesSelected.Any(x => x.Value == "UK"))
                        licences = licences.Where(x =>
                            ukCountryNames.Any(y => x.OperatingCountries.Any(z => z.Country.Name == y)));
                    else if (publicRegisterSearchViewModel.CountriesSelected.Any(x => x.Value == "Outside UK"))
                        licences = licences.Where(x =>
                            ukCountryNames.Any(y => x.OperatingCountries.All(z => z.Country.Name != y)));
                    else
                        licences = licences.Where(x => x.OperatingCountries.Any(y =>
                            publicRegisterSearchViewModel.CountriesSelected.Any(z => y.Country.Name.Contains(z.Text))));

                    break;
                case "arelocated":
                    if (publicRegisterSearchViewModel.CountriesSelected.Any(x => x.Value == "UK"))
                        licences = licences.Where(x => ukCountryNames.Any(y => x.Address.Country.Contains(y)));
                    else if (publicRegisterSearchViewModel.CountriesSelected.Any(x => x.Value == "Outside UK"))
                        licences = licences.Where(x => ukCountryNames.Any(y => x.Address.NonUK));
                    else
                        licences = licences.Where(x => x.OperatingCountries.Any(y =>
                            publicRegisterSearchViewModel.CountriesSelected.Any(z => y.Country.Name.Contains(z.Text))));

                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(publicRegisterSearchViewModel.SupplierWho));
            }

            return new PublicRegisterLicenceListViewModel
            {
                Title = "Public Register",
                Licences = licences.Select(BuildSummary)
            };
            //return new PublicRegisterLicenceListViewModel
            //{
            //    Title = "Public Register",
            //    //We only want licences with status that are allowed to be shown using the "ShowInPublicRegister" field.
            //    Licences = licenceRepository.GetAllLicences()
            //        .Where(x => GetLatestStatus(x).Status.ShowInPublicRegister
            //                    && (x.BusinessName.Contains(publicRegisterSearchViewModel.BusinessName) ||
            //                        string.IsNullOrWhiteSpace(publicRegisterSearchViewModel.BusinessName))
            //                    && (x.OperatingCountries.Any(y =>
            //                            y.Country.Name.Contains(publicRegisterSearchViewModel.)) ||
            //                        string.IsNullOrWhiteSpace(publicRegisterSearchViewModel.LicenceCountry))
            //                    && (x.Address.Country.Contains(publicRegisterSearchViewModel.OrganisationCountry) ||
            //                        string.IsNullOrWhiteSpace(publicRegisterSearchViewModel.OrganisationCountry)))
            //        .Select(BuildSummary)
            //};
        }

        public PublicRegisterLicenceSummaryViewModel BuildLicence(int id)
        {
            var licence = licenceRepository.GetById(id);

            return GetLatestStatus(licence).Status.IsLicence ? BuildSummary(licence) : null;
        }

        private PublicRegisterLicenceSummaryViewModel BuildSummary(Licence licence)
        {
            return mapper.Map<PublicRegisterLicenceSummaryViewModel>(licence);
        }

        public static LicenceStatusChange GetLatestStatus(Licence licence)
        {
            return licence.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).First();
        }

        public PublicRegisterSearchViewModel BuildPublicRegisterSearchViewModel()
        {
            return new PublicRegisterSearchViewModel(BuildAvailableCountries())
            {
                CountryAdded = "",
                SupplierWho = new SelectListItem { Value = "supply", Text = "Supply" },
                BusinessName = "",
                CountriesSelected = new List<SelectListItem>(),
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
