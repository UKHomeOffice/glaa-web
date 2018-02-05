using System;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.PublicRegister
{
    public class PublicRegisterLicenceSummaryViewModel
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public string BusinessName { get; set; }
        public string TradingName { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string PublicRegisterStatus { get; set; }
        public bool IsApplication { get; set; }
        public bool IsLicence => !IsApplication;
        public DateTime? ApplicationDate => MostRecentStatus?.DateCreated;
        public LicenceStatusViewModel MostRecentStatus { get; set; }

    }
}
