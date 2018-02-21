using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.PublicRegister
{
    public class PublicRegisterLicenceDetailViewModel
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public string BusinessName { get; set; }
        public string TradingName { get; set; }
        public List<PrincipalAuthorityViewModel> PrincipalAuthorities { get; set; }
        public string BusinessType { get; set; }
        public List<LicenceIndustryViewModel> OperatingIndustries { get; set; }
        public string PublicRegisterStatus { get; set; }
        public bool IsApplication { get; set; }
        public bool IsLicence => !IsApplication;
        public List<NamedIndividualViewModel> NamedIndividuals { get; set; }
        public List<NamedJobTitleViewModel> NamedPosts { get; set; }
        //public string AuthorisedPersonsOrPostholders
        //{
        //    get
        //    {
        //        //var builder = new StringBuilder("Todo - ");
        //        var builder = new StringBuilder();

        //        builder.Append(string.Join("<br />", NamedIndividuals.Select(x => x.FullName.FullName)));
        //        if (NamedPosts.Count > 0) builder.Append("<br />");
        //        builder.Append(string.Join("<br />", NamedPosts.Select(x => x.JobTitle)));

        //        return builder.ToString();
        //    }
        //}
        public string BusinessPhoneNumber { get; set; }
        public bool CanOperateAcrossUk => Address.Countries.Single(c => c.Value == Address.CountryId.ToString()).Text.EndsWith("Northern Ireland") && OperatingCountries.Any(x => x.Country.Name != "Northern Ireland");
        public List<LicenceCountryViewModel> OperatingCountries { get; set; }
        public AddressViewModel Address { get; set; }
        public DateTime? ApplicationDate => MostRecentLicenceIssuedStatus?.DateCreated;
        public DateTime? CommencementDate => MostRecentLicenceSubmittedStatus?.DateCreated;
        public LicenceStatusViewModel MostRecentStatus { get; set; }
        public LicenceStatusViewModel MostRecentLicenceIssuedStatus { get; set; }
        public LicenceStatusViewModel MostRecentLicenceSubmittedStatus { get; set; }
    }
}
