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
            BusinessCredentialsViewModel = new BusinessCredentialsViewModel();
            PAYEStatus = new PAYEStatusViewModel();
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
        public BusinessCredentialsViewModel BusinessCredentialsViewModel { get; set; }
        public PAYEStatusViewModel PAYEStatus { get; set; }
        public VATStatusViewModel VATStatus { get; set; }
        public TaxReferenceViewModel TaxReference { get; set; }
    }


    public class BusinessNameViewModel : YesNoViewModel, IValidatable, IRequiredIf
    {
        [Required]
        [Display(Name = "Business Name", Description = "This is the name of the business you control")]
        public string BusinessName { get; set; }

        [Required]
        [Display(Name = "Do you have a trading name that is different from the business name?")]
        public bool? HasTradingName { get; set; }

        [Display(Name = "Current Trading Name")]
        [RequiredIf(ErrorMessage = "The Current Trading Name field is required")]
        public string TradingName { get; set; }

        [Display(Name = "Has your business traded under any other name in the last 5 years?")]
        [RequiredIf(ErrorMessage = "The Has Previous Trading Names field is required")]
        public bool? HasPreviousTradingName { get; set; }

        public List<PreviousTradingNameViewModel> PreviousTradingNames { get; set; }

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

        public bool IsRequired => HasTradingName ?? false;
    }

    public class PreviousTradingNameViewModel : Validatable, IId
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Previous Business Name field is required")]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "The Previous Business Town field is required")]
        [Display(Name = "Town")]
        public string Town { get; set; }

        [Required(ErrorMessage = "The Previous Business Country field is required")]
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
        [Compare("BusinessEmailAddress")]
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
        public List<LegalStatus> AvailableLegalStatuses { get; set; } = new List<LegalStatus>
        {
            new LegalStatus { Id = 1, Name = "Sole Trader", Checked = false, EnumMappedTo = LegalStatusEnum.SoleTrader },
            new LegalStatus { Id = 2, Name = "Registered Company", Checked = false, EnumMappedTo = LegalStatusEnum.RegisteredCompany },
            new LegalStatus { Id = 3, Name = "Partnership", Checked = false, EnumMappedTo = LegalStatusEnum.Partnership },
            new LegalStatus { Id = 4, Name = "Unincorporate Association", Checked = false, EnumMappedTo = LegalStatusEnum.UnincorporateAssociation },
            new LegalStatus { Id = 5, Name = "Other", Checked = false, EnumMappedTo = LegalStatusEnum.Other }
        };

        [RequiredIf]
        [Display(Name = "Other legal status")]
        public string Other { get; set; }

        [Required]
        [Display(Name = "Legal Status", Description = "What is the legal status of your business?")]
        public LegalStatusEnum? LegalStatus { get; set; }

        public bool IsRequired => LegalStatus.HasValue && LegalStatus.Value == LegalStatusEnum.Other;
    }


    public class PAYEStatusViewModel : YesNoViewModel, IRequiredIf, IValidatable
    {
        public PAYEStatusViewModel()
        {
            PAYENumbers = new List<PAYENumberRow>();
        }

        [Required]
        [Display(Name = "Do you have an PAYE Registration Number?", Description = "Please put the organisations PAYE registration number in the space provided.")]
        public bool? HasPAYENumber { get; set; }

        public List<PAYENumberRow> PAYENumbers { get; set; }

        public bool IsRequired => HasPAYENumber ?? false;

        public bool IsValid { get; set; }

        public void Validate()
        {

            if (HasPAYENumber.HasValue == false)
            {
                IsValid = false;
                return;
            }

            foreach (var number in PAYENumbers)
            {
                number.Validate();
            }

            IsValid = PAYENumbers.All(x => x.IsValid);

        }
    }

    public class PAYENumberRow : Validatable, IId
    {
        public PAYENumberRow()
        {
            PAYERegistrationDate = new DateViewModel();
        }

        public int Id { get; set; }

        // TODO: Check example numbers
        [Required]
        [RegularExpression(@"\d{3}\/[a-zA-Z]{1,2}\d{5}", ErrorMessage = "Please enter a valid PAYE Number")]
        [Display(Name = "PAYE Registration Number", Description = "For example 123/A12345 or 123/AB12345")]
        public string PAYENumber { get; set; }

        [UIHint("_NullableDateTime")]
        [Display(Name = "PAYE Registration Date")]
        public DateViewModel PAYERegistrationDate { get; set; }
    }

    public class VATStatusViewModel : YesNoViewModel, IRequiredIf, IValidatable
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
        [Display(Name = "VAT Registration Number", Description = "For example GB999 9999 73")]
        public string VATNumber { get; set; }

        [UIHint("_NullableDateTime")]
        [Display(Name = "VAT Registration Date")]
        public DateViewModel VATRegistrationDate { get; set; }

        public bool IsRequired => HasVATNumber ?? false;

        public bool IsValid { get; set; }

        public void Validate()
        {
            var invalidModelFields = new List<string>();
            foreach (var prop in GetType().GetProperties())
            {
                var obj = prop.GetValue(this) ?? string.Empty;

                var validatable = obj as IValidatable;

                bool propertyIsValid;

                if (validatable != null)
                {
                    // Use the defined validate method if one is defined
                    validatable.Validate();
                    propertyIsValid = validatable.IsValid;
                }
                else
                {
                    // Use the validation context for properties
                    var context = new ValidationContext(obj, null);
                    propertyIsValid = Validator.TryValidateObject(obj, context, null, true);
                }

                if (!propertyIsValid)
                {
                    invalidModelFields.Add(prop.Name);
                }
            }
            IsValid = !invalidModelFields.Any();
        }
    }

    public class TaxReferenceViewModel : Validatable
    {
        //[Required]
        //public bool? HasTaxReferenceNumber { get; set; }        

        [Required(ErrorMessage = "The Personal Unique Tax Reference number field is required")]
        [RegularExpression(@"\d{9}[\dkK]{1}", ErrorMessage = "Please enter a valid Personal Unique Tax Reference Number")]
        [Display(Name = "Personal Unique Tax Reference number", Description = "For example 1334404714")]
        public string SoleTraderTaxReference
        {
            get { return TaxReferenceNumber; }
            set { TaxReferenceNumber = value; }
        }

        [Required(ErrorMessage = "The Company Unique Tax Reference number field is required")]
        [RegularExpression(@"\d{9}[\dkK]{1}", ErrorMessage = "Please enter a valid Company Unique Tax Reference Number")]
        [Display(Name = "Company Unique Tax Reference number", Description = "For example 1334404714")]
        public string RegisteredCompanyTaxReference
        {
            get { return TaxReferenceNumber; }
            set { TaxReferenceNumber = value; }
        }

        [Required(ErrorMessage = "The Business Unique Tax Reference number field is required")]
        [RegularExpression(@"\d{9}[\dkK]{1}", ErrorMessage = "Please enter a valid Business Unique Tax Reference Number")]
        [Display(Name = "Business Unique Tax Reference number", Description = "For example 1334404714")]
        public string PartnershipTaxReference
        {
            get { return TaxReferenceNumber; }
            set { TaxReferenceNumber = value; }
        }

        // Only GOV.UK format guidance: https://www.gov.uk/find-lost-utr-number
        // X:\04PTW\38500\38548 - GLAA - Government Licensing System\TECHNICAL\HMRC_dummy_data.txt
        [Required(ErrorMessage = "The Tax Reference number field is required")]
        [RegularExpression(@"\d{9}[\dkK]{1}", ErrorMessage = "Please enter a valid Tax Reference Number")]
        [Display(Name = "Tax reference number", Description = "For example 1334404714")]
        public string TaxReferenceNumber { get; set; }

        //public bool IsRequired => HasTaxReferenceNumber.HasValue && HasTaxReferenceNumber.Value;
    }
}