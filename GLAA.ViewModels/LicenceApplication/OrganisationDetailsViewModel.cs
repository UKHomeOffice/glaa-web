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
            BusinessName = new BusinessNameViewModel();
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

        public BusinessNameViewModel BusinessName { get; set; }
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


    public class BusinessNameViewModel : IValidatable
    {
        [Required]
        [Display(Name = "", Description = "This is the name of the business you control")]
        public string BusinessName { get; set; }

        [Required]
        [Display(Name = "Do you have a trading name that is different from the business name?")]
        public bool? HasTradingName { get; set; }

        [Display(Name = "Current Trading Name")]
        public string TradingName { get; set; }

        [Display(Name = "Has your business traded under any other name in the last 5 years?")]
        public bool? HasPreviousTradingName { get; set; }

        public IEnumerable<PreviousTradingNameViewModel> PreviousTradingNames { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(BusinessName) || !HasTradingName.HasValue)
            {
                IsValid = false;
                return;
            }

            if (HasTradingName.Value && (!HasPreviousTradingName.HasValue || string.IsNullOrEmpty(TradingName)))
            {
                IsValid = false;
                return;
            }

            if ((HasPreviousTradingName ?? false) && !PreviousTradingNames.Any())
            {
                IsValid = false;
                return;
            }

            foreach (var business in PreviousTradingNames)
            {
                business.Validate();
            }
            IsValid = PreviousTradingNames.All(b => b.IsValid);
        }

        public bool IsValid { get; set; }
    }

    public class PreviousTradingNameViewModel : Validatable
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Required]
        [Display(Name = "Town")]
        public string Town { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
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
                new CheckboxListItem { Id = 3, Name = "Food Packaging and Processing"},
                new CheckboxListItem { Id = 4, Name = "Shellfish gathering"},
                new CheckboxListItem { Id = 5, Name = "Other"}
            };
        }

        [CheckboxRequired("Please select at least one sector")]
        [Display(Name = "Industry", Description = "Select all that apply")]
        public List<CheckboxListItem> OperatingIndustries { get; set; }

        [RequiredIf(ErrorMessage = "You must enter the sectors you intend to operate in")]
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
        [Display(Name = "Operating Country", Description = "Use the tick boxes to indicate which countries you intend to supply with workers")]
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
        [Display(Name = "Turnover Band", Description = "Please state what you estimate your annual gross turnover will be in the GLAA regulated sectors in your first year of trading should a licence be granted")]
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
        [Display(Name = "Communication Preference", Description = "Tick the box that indicates how you would like to receive written communication from the GLAA")]
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
        [RegularExpression(@"\w{2}\d{6}", ErrorMessage = "Companies House registration number")]
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
        [RegularExpression(@"\d{3}\/[a-zA-Z]{1,2}\d{5}", ErrorMessage = "Please enter a valid PAYE Number")]
        [RequiredIf(ErrorMessage = "The PAYE Number field is required")]
        [Display(Name = "PAYE Number", Description = "For example 123/A12345 or 123/AB12345")]
        public string PAYEERNNumber { get; set; }
        
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
        [Display(Name = "Do you have an VAT registration number?")]
        public bool? HasVATNumber { get; set; }

        // TODO: Check example numbers
        // May match some invalid formats, see below for more... :-/
        // https://en.wikipedia.org/wiki/VAT_identification_number
        [RegularExpression(@"[a-zA-Z]{2}[a-zA-Z0-9 ]{2,13}", ErrorMessage = "Please enter a valid VAT registration number")]
        [RequiredIf(ErrorMessage = "The VAT registration number field is required")]
        [Display(Name = "VAT Number", Description = "For example GB999 9999 73")]
        public string VATNumber { get; set; }

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