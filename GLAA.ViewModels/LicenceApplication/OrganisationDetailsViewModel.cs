using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GLAA.Domain.Models;
using GLAA.ViewModels.Core;
using GLAA.ViewModels.Core.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class OrganisationDetailsViewModel : Validatable
    {
        public OrganisationDetailsViewModel()
        {
            OrganisationName = new OrganisationNameViewModel();
            TradingName = new TradingNameViewModel();
            OperatingIndustries = new OperatingIndustriesViewModel();
            OperatingCountries = new OperatingCountriesViewModel();
            Turnover = new TurnoverViewModel();
            CommunicationPreference = new CommunicationPreferenceViewModel();
            Address = new AddressViewModel();
            BusinessPhoneNumber = new BusinessPhoneNumberViewModel();
            BusinessMobileNumber = new BusinessMobileNumberViewModel();
            BusinessEmailAddress = new BusinessEmailAddressViewModel();
            BusinessWebsite = new BusinessWebsiteViewModel();
            LegalStatus = new LegalStatusViewModel();
            PAYEERNStatus = new PAYEERNStatusViewModel();
            VATStatus = new VATStatusViewModel();
            TaxReference = new TaxReferenceViewModel();
        }

        public OrganisationNameViewModel OrganisationName { get; set; }
        public TradingNameViewModel TradingName { get; set; }
        public OperatingIndustriesViewModel OperatingIndustries { get; set; }
        public OperatingCountriesViewModel OperatingCountries { get; set; }
        public TurnoverViewModel Turnover { get; set; }
        public CommunicationPreferenceViewModel CommunicationPreference { get; set; }
        public AddressViewModel Address { get; set; }
        public BusinessPhoneNumberViewModel BusinessPhoneNumber { get; set; }
        public BusinessMobileNumberViewModel BusinessMobileNumber { get; set; }
        public BusinessEmailAddressViewModel BusinessEmailAddress { get; set; }
        public BusinessWebsiteViewModel BusinessWebsite { get; set; }
        public LegalStatusViewModel LegalStatus { get; set; }
        public PAYEERNStatusViewModel PAYEERNStatus { get; set; }
        public VATStatusViewModel VATStatus { get; set; }
        public TaxReferenceViewModel TaxReference { get; set; }
    }


    public class OrganisationNameViewModel
    {
        [Required]
        [Display(Name = "Organisation Name", Description = "The name you registered with companies house")]
        public string OrganisationName { get; set; }
    }

    public class TradingNameViewModel
    {
        [Display(Name = "Trading Name", Description = "Enter this if it's different from your organisation name")]
        public string TradingName { get; set; }
    }

    public class OperatingIndustriesViewModel : ICollectionViewModel, IRequiredIf
    {
        public OperatingIndustriesViewModel()
        {
            // TODO: The value here should be set from db? existing model?
            OperatingIndustries = GetAvailableIndustries();
        }

        private List<CheckboxListItem> GetAvailableIndustries()
        {
            return new List<CheckboxListItem>
            {
                new CheckboxListItem { Id = 1, Name = "Agriculture"},
                new CheckboxListItem { Id = 2, Name = "Horticulture"},
                new CheckboxListItem { Id = 3, Name = "Processing and packaging of all fresh food, drinks and other produce"},
                new CheckboxListItem { Id = 4, Name = "Shellfish gathering"},
                new CheckboxListItem { Id = 5, Name = "Other"}
            };
        }

        [CheckboxRequired("Please select at least one industry")]
        [Display(Name = "Industry", Description = "Select all that apply")]
        public List<CheckboxListItem> OperatingIndustries { get; set; }

        [RequiredIf(ErrorMessage = "You must enter the industry you operate in")]
        [Display(Name = "Other")]
        public string OtherIndustry { get; set; }

        public bool IsRequired => OperatingIndustries.Any(x => x.Checked && x.Id == 5);
    }

    public class OperatingCountriesViewModel : ICollectionViewModel
    {
        public OperatingCountriesViewModel()
        {
            // TODO: The value here should be set from db? existing model?
            OperatingCountries = GetAvailableCountries();
        }

        private List<CheckboxListItem> GetAvailableCountries()
        {
            return new List<CheckboxListItem>
            {
                new CheckboxListItem { Id = 1, Name = "England" },
                new CheckboxListItem { Id = 2, Name = "Scotland" },
                new CheckboxListItem { Id = 3, Name = "Wales" },
                new CheckboxListItem { Id = 4, Name = "Northern Ireland" }
            };
        }

        [CheckboxRequired("Please select at least one country")]
        [Display(Name = "Operating Country", Description = "Select all that apply")]
        public List<CheckboxListItem> OperatingCountries { get; set; }
    }

    public class TurnoverViewModel
    {
        public List<SelectListItem> AvailableTurnoverBands { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "OverTenMillion", Text = "Over £10 Million" },
            new SelectListItem { Value = "FiveToTenMillion", Text = "£5 - £10 Million" },
            new SelectListItem { Value = "OneToFiveMillion", Text = "£1 - 5 Million" },
            new SelectListItem { Value = "UnderOneMillion", Text = "Under £1 Million" }
        };

        [Required(ErrorMessage = "Please select a turnover band")]
        [Display(Name = "Turnover Band", Description = "Only enter the turnover for regulated sectors, not your entire companies turnover")]
        public TurnoverBand? TurnoverBand { get; set; }
    }

    public class CommunicationPreferenceViewModel
    {
        public List<SelectListItem> AvailableCommunicationPreferences { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Email" },
            new SelectListItem { Value = "2", Text = "Letter" }
        };

        [Required]
        [Display(Name = "Communication Preference", Description = "Choose your preferred channel of communication. If you choose email we will contact you at the email address that you provide on the next page")]
        public CommunicationPreference? CommunicationPreference { get; set; }
    }

    public class BusinessMobileNumberViewModel
    {
        [Phone]
        [Display(Name = "Business Mobile Phone Number")]
        public string BusinessMobileNumber { get; set; }
    }

    public class BusinessEmailAddressViewModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Business Email Address")]
        public string BusinessEmailAddress { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [System.ComponentModel.DataAnnotations.Compare("BusinessEmailAddress")]
        [Display(Name = "Confirm Business Email Address")]
        public string BusinessEmailAddressConfirmation { get; set; }
    }

    public class BusinessWebsiteViewModel
    {
        [Url(ErrorMessage = "The Business Website address must be in a valid format")]
        [Display(Name = "Business Website", Description = "E.g http://www.example.com")]
        public string BusinessWebsite { get; set; }
    }

    public class LegalStatusViewModel : IRequiredIf
    {
        public LegalStatusViewModel()
        {
            CompanyRegistrationDate = new DateViewModel();
        }

        public List<LegalStatus> AvailableLegalStatuses { get; set; } = new List<LegalStatus>
        {
            new LegalStatus { Id = 1, Name = "Sole Trader", Checked = false },
            new LegalStatus { Id = 2, Name = "Limited Company", Checked = false },
            new LegalStatus { Id = 3, Name = "Partnership", Checked = false },
            new LegalStatus { Id = 4, Name = "Unincorporate Association", Checked = false },
            new LegalStatus { Id = 5, Name = "Other", Checked = false },
        };

        [Required]
        [Display(Name = "Legal Status", Description = "What is the legal status of your organisation?")]
        public LegalStatusEnum? LegalStatus { get; set; }

        // TODO: Check example numbers
        [RegularExpression(@"\w{2}\d{6}", ErrorMessage = "Please enter a valid Companies House Registration Number")]
        [RequiredIf(ErrorMessage = "The Companies House Registration Number is required")]
        [Display(Name = "Companies House Registration Number", Description = "For example 01234567 or OC012345")]
        public string CompaniesHouseNumber { get; set; }

        [RequiredIf(ErrorMessage = "The Registration Date field is required")]
        [UIHint("_NullableDateTime")]
        [Display(Name = "Registration Date", Description = "Please enter the date your organisation registered with Companies House.")]
        public DateViewModel CompanyRegistrationDate { get; set; }

        public bool IsRequired => (LegalStatus ?? LegalStatusEnum.Other) == LegalStatusEnum.LimitedCompany;
    }

    public class PAYEERNStatusViewModel : YesNoViewModel, IRequiredIf
    {
        public PAYEERNStatusViewModel()
        {
            PAYEERNRegistrationDate = new DateViewModel();
        }

        [Required]
        [Display(Name = "Do you have an ERN/PAYE Registion Number?")]
        public bool? HasPAYEERNNumber { get; set; }

        // TODO: Check example numbers
        [RegularExpression(@"\d{3}\/[a-zA-Z]{1,2}\d{5}", ErrorMessage = "Please enter a valid PAYE/ERN Number")]
        [RequiredIf(ErrorMessage = "The PAYE/ERN Number field is required")]
        [Display(Name = "PAYE/ERN Number", Description = "For example 123/A12345 or 123/AB12345")]
        public string PAYEERNNumber { get; set; }

        [RequiredIf(ErrorMessage = "The Registration Date field is required")]
        [UIHint("_NullableDateTime")]
        [Display(Name = "Employer Registration Date", Description = "Please enter the date you registered as an employer with HMRC.")]
        public DateViewModel PAYEERNRegistrationDate { get; set; }

        public bool IsRequired => HasPAYEERNNumber ?? false;
    }

    public class VATStatusViewModel : YesNoViewModel, IRequiredIf
    {
        public VATStatusViewModel()
        {
            VATRegistrationDate = new DateViewModel();
        }

        [Required]
        [Display(Name = "Do you have an VAT Registration Number?")]
        public bool? HasVATNumber { get; set; }

        // TODO: Check example numbers
        // May match some invalid formats, see below for more... :-/
        // https://en.wikipedia.org/wiki/VAT_identification_number
        [RegularExpression(@"[a-zA-Z]{2}[a-zA-Z0-9 ]{2,13}", ErrorMessage = "Please enter a valid VAT Number")]
        [RequiredIf(ErrorMessage = "The VAT Number field is required")]
        [Display(Name = "VAT Number", Description = "For example GB999 9999 73")]
        public string VATNumber { get; set; }

        [RequiredIf(ErrorMessage = "The Registration Date field is required")]
        [UIHint("_NullableDateTime")]
        [Display(Name = "VAT Registration Date", Description = "Please enter the date you registered for VAT.")]
        public DateViewModel VATRegistrationDate { get; set; }

        public bool IsRequired => HasVATNumber ?? false;
    }

    public class TaxReferenceViewModel
    {
        // Only GOV.UK format guidance: https://www.gov.uk/find-lost-utr-number
        // X:\04PTW\38500\38548 - GLAA - Government Licensing System\TECHNICAL\HMRC_dummy_data.txt
        [Required]
        [RegularExpression(@"\d{9}[\dkK]{1}", ErrorMessage = "Please enter a valid Tax Reference Number")]
        [Display(Name = "Tax reference number", Description = "For example 1334404714")]
        public string TaxReferenceNumber { get; set; }
    }
}