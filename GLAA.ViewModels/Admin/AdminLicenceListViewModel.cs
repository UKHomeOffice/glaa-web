using System;
using System.Collections.Generic;
using System.Linq;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.Admin
{
    public class AdminLicenceListViewModel
    {
        public string Title { get; set; }
        public IEnumerable<AdminLicenceSummaryViewModel> Licences { get; set; }
        // TODO: Check these
        public int NewApplications => Licences.Count(l => l.MostRecentStatus.AdminCategory == StatusAdminCategory.NewApplications);
        // TODO: Check these
        public int RequireAttention => Licences.Count(l => l.MostRecentStatus.AdminCategory == StatusAdminCategory.RequireAttention);
        // TODO: Check these
        public int OutstandingFees => Licences.Count(l => l.MostRecentStatus.AdminCategory == StatusAdminCategory.OutstandingFees);
        // TODO: Check this
        public int NearlyExpired => Licences.Count(l =>
            l.IsApplication && DateTime.Now - l.ApplicationDate > new TimeSpan(60, 0, 0, 0) ||
            l.IsLicence && DateTime.Now - l.ApplicationDate > new TimeSpan(330, 0, 0, 0));
    }

    public class AdminLicenceSummaryViewModel
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        public string PrincipalAuthorityName { get; set; }
        public bool IsApplication { get; set; }
        public bool IsLicence => !IsApplication;
        public DateTime? ApplicationDate => MostRecentStatus?.DateCreated;
        public LicenceStatusViewModel MostRecentStatus { get; set; }
    }
}
