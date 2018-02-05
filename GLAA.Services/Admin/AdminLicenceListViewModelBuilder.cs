using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels;
using GLAA.ViewModels.Admin;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Admin
{
    public class AdminLicenceListViewModelBuilder : IAdminLicenceListViewModelBuilder
    {
        private readonly ILicenceRepository licenceRepository;
        private readonly IMapper mapper;

        public AdminLicenceListViewModelBuilder(ILicenceRepository licenceRepository, IMapper mapper)
        {
            this.licenceRepository = licenceRepository;
            this.mapper = mapper;
        }

        public T New<T>() where T : new()
        {
            return new T();
        }

        public AdminLicenceListViewModel New()
        {
            return new AdminLicenceListViewModel();
        }

        public AdminLicenceListViewModel Build(LicenceOrApplication type)
        {
            return type == LicenceOrApplication.Application
                ? new AdminLicenceListViewModel
                {
                    Title = "Applications",
                    Licences = licenceRepository.GetAllApplications().Select(BuildSummary)
                }
                : new AdminLicenceListViewModel
                {
                    Title = "Licences",
                    Licences = licenceRepository.GetAllLicences().Select(BuildSummary)
                };
        }

        private AdminLicenceSummaryViewModel BuildSummary(Licence licence)
        {
            var latestStatus = licence.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).First();
            var statusModel = new LicenceStatusViewModel();
            return new AdminLicenceSummaryViewModel
            {
                Id = licence.Id,
                ApplicationId = licence.ApplicationId,
                MostRecentStatus = mapper.Map(latestStatus, statusModel),
                OrganisationName = licence.BusinessName,
                PrincipalAuthorityName = licence.PrincipalAuthorities.FirstOrDefault() != null ? licence.PrincipalAuthorities.First().FullName : "Not Set",
                IsApplication = latestStatus.Status.IsApplication
            };
        }
    }
}
