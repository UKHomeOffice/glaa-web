using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GLAA.ViewModels.Attributes;
using GLAA.ViewModels.Core;
using GLAA.ViewModels.Core.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class YesNoViewModel
    {
        public List<SelectListItem> YesNo { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true"},
            new SelectListItem { Text = "No", Value = "false"}
        };
    }

    public class ValidatableYesNoViewModel : Validatable
    {
        public List<SelectListItem> YesNo { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true"},
            new SelectListItem { Text = "No", Value = "false"}
        };
    }

    public class FullNameViewModel
    {
        [Required]
        [Display(Name = "Full name", Description = "Your given and surname<br/>E.g John Smith")]
        public string FullName { get; set; }
    }

    public class AlternativeFullNameViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Are you known by any alternative names?")]
        public bool? HasAlternativeName { get; set; }

        [RequiredIf(ErrorMessage = "The Alternative full name field is required.")]
        [Display(Name = "Alternative full name", Description = "E.g John Smith")]
        public string AlternativeName { get; set; }

        public bool IsRequired => HasAlternativeName ?? false;
    }

    public class DateOfBirthViewModel
    {
        [UIHint("_NullableDateTime")]
        [DateRequired(ErrorMessage = "The Date of birth field is required.")]
        [Display(Name = "Date of birth", Description = "For example 31 3 1980")]
        public DateViewModel DateOfBirth { get; set; }
    }

    public class TownOfBirthViewModel
    {
        [Required]
        [Display(Name = "Town of birth", Description = "This will be shown on your birth certificate")]
        public string TownOfBirth { get; set; }
    }

    public class CountryOfBirthViewModel : INeedCountries
    {
        [Required]
        [Display(Name = "Country of birth", Description = "This will be shown on your birth certificate")]
        public int? CountryOfBirthId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; } = new List<SelectListItem>();
    }

    public class BirthDetailsViewModel : Validatable, INeedCountries
    {
        public BirthDetailsViewModel()
        {
            TownOfBirthViewModel = new TownOfBirthViewModel();
            CountryOfBirthViewModel = new CountryOfBirthViewModel();
            NationalInsuranceNumberViewModel = new NationalInsuranceNumberViewModel();
            SocialSecurityNumberViewModel = new SocialSecurityNumberViewModel();
        }

        public TownOfBirthViewModel TownOfBirthViewModel { get; set; }
        public CountryOfBirthViewModel CountryOfBirthViewModel { get; set; }
        public NationalInsuranceNumberViewModel NationalInsuranceNumberViewModel { get; set; }
        public SocialSecurityNumberViewModel SocialSecurityNumberViewModel { get; set; }

        public IEnumerable<SelectListItem> Countries
        {
            get => CountryOfBirthViewModel.Countries;
            set => CountryOfBirthViewModel.Countries = value;
        }
    }

    public class JobTitleViewModel
    {
        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
    }

    public class NamedJobTitleViewModel : Validatable
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Number employed is this role")]
        public int? JobTitleNumber { get; set; }
    }

    public class BusinessPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Business Phone Number", Description = "Include the dialling code if you are a UK business<br/> E.g +44 1234 123456")]
        public string BusinessPhoneNumber { get; set; }
    }

    public class BusinessExtensionViewModel
    {
        [Display(Name = "Extension Number")]
        public string BusinessExtension { get; set; }
    }

    public class PersonalMobileNumberViewModel
    {
        [Phone]
        [Display(Name = "Personal Mobile Number")]
        public string PersonalMobileNumber { get; set; }
    }

    public class PersonalEmailAddressViewModel
    {
        [EmailAddress]
        [Display(Name = "Personal Email Address")]
        public string PersonalEmailAddress { get; set; }
    }

    public interface IUkOnly
    {
        bool IsUk { get; }
    }

    public class NationalInsuranceNumberViewModel : IUkOnly
    {
        // Matches QQ 12 34 56 A and QQ123456A
        // Also matches a few invalid prefix combinations but they would make the regex enormous
        // https://www.gov.uk/hmrc-internal-manuals/national-insurance-manual/nim39110
        // https://www.whatdotheyknow.com/request/178190/response/439137/attach/3/FOI%204543.pdf
        [RegularExpression(@"^[A-CEGHJ-PR-TW-Z]{1}[A-CEGHJ-NPR-TW-Z]{1}\s?\d{2}\s?\d{2}\s?\d{2}\s?[A-DFM]{0,1}$", ErrorMessage = "Please enter a valid National Insurance number")]
        [RequiredIfUkAddress]
        [Display(Name = "National Insurance Number", Description = "For example QQ123456A")]
        public string NationalInsuranceNumber { get; set; }
        public bool IsUk { get; set; }
    }

    public class SocialSecurityNumberViewModel
    {
        [Display(Name = "Social Security Number")]
        public string SocialSecurityNumber { get; set; }

    }

    public class NationalityViewModel
    {
        [Required]
        [Display(Name = "What is your nationality?")]
        public string Nationality { get; set; }
    }

    public class PassportViewModel : YesNoViewModel
    {
        [Required]
        [Display(Name = "Do you have a passport?")]
        public bool? HasPassport { get; set; }
    }

    public class RightToWorkViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Does this person require a visa or work permit to work in the UK?")]
        public bool? RequiresVisa { get; set; }

        [RequiredIf(ErrorMessage = "The description field is required.")]
        [Display(Name = "Brief description of the immigration status and the visa / work permit(s) held by this person")]
        public string VisaDescription { get; set; }

        public bool IsRequired => RequiresVisa ?? false;
    }

    public class UndischargedBankruptViewModel : YesNoViewModel, IRequiredIf
    {
        public UndischargedBankruptViewModel()
        {
            BankruptcyDate = new DateViewModel();
        }

        [Required]
        [Display(Name = "Are you an undischarged bankrupt?")]
        public bool? IsUndischargedBankrupt { get; set; }

        [UIHint("_NullableDateTime")]
        [RequiredIf(ErrorMessage = "The Bankruptcy Date field is required.")]
        [Display(Name = "Give the date of the bankruptcy")]
        public DateViewModel BankruptcyDate { get; set; }

        [RequiredIf(ErrorMessage = "The Bankruptcy Number field is required.")]
        [Display(Name = "Bankruptcy number", Description = "For example DRO1234567 or 1234567")]
        [RegularExpression(@"([a-zA-Z]{3}|[A-Z]{0})\d{7}", ErrorMessage = "Please enter a valid bankruptcy number.")]
        public string BankruptcyNumber { get; set; }

        public bool IsRequired => IsUndischargedBankrupt ?? false;
    }

    public class DisqualifiedDirectorViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Are you disqualified as a company director?")]
        public bool? IsDisqualifiedDirector { get; set; }

        [RequiredIf(ErrorMessage = "The Disqualification description field is required.")]
        [Display(Name = "Give a brief description of the disqualification")]
        public string DisqualificationDetails { get; set; }

        public bool IsRequired => IsDisqualifiedDirector ?? false;
    }

    public class RestraintOrdersViewModel : YesNoViewModel, IValidatable, ICanView<PrincipalAuthorityViewModel>,
        ICanView<DirectorOrPartnerViewModel>, ICanView<AlternativeBusinessRepresentativeViewModel>, ICanView<NamedIndividualViewModel>
    {
        [Required]
        [Display(Name =
            "Are you/have you been the subject of a restraint or confiscation order, or civil recovery under the Proceeds of Crime Act 2002?")]
        public bool? HasRestraintOrders { get; set; }

        [Display(Name = "Details of restraint or confiscation orders, or civil recoveries")]
        public IEnumerable<RestraintOrderViewModel> RestraintOrders { get; set; }

        public RestraintOrdersViewModel()
        {
            RestraintOrders = new List<RestraintOrderViewModel>();
        }

        public void Validate()
        {
            if (!HasRestraintOrders.HasValue)
            {
                IsValid = false;
                return;
            }

            if (HasRestraintOrders.Value && !RestraintOrders.Any())
            {
                IsValid = false;
                return;
            }

            foreach (var ro in RestraintOrders)
            {
                ro.Validate();
            }
            IsValid = RestraintOrders.All(ro => ro.IsValid);
        }

        public bool IsValid { get; set; }

        public bool CanView(PrincipalAuthorityViewModel parent)
        {
            return parent.RestraintOrdersViewModel.HasRestraintOrders ?? false;
        }

        public bool CanView(DirectorOrPartnerViewModel parent)
        {
            return parent.RestraintOrdersViewModel.HasRestraintOrders ?? false;
        }

        public bool CanView(AlternativeBusinessRepresentativeViewModel parent)
        {
            return parent.RestraintOrdersViewModel.HasRestraintOrders ?? false;
        }

        public bool CanView(NamedIndividualViewModel parent)
        {
            return parent.RestraintOrdersViewModel.HasRestraintOrders ?? false;
        }
    }

    public class RestraintOrderViewModel : Validatable
    {
        public int Id { get; set; }

        [UIHint("_NullableDateTime")]
        [DateRequired(ErrorMessage = "The date of the restraint, confiscation order or civil recovery field is required.")]
        [Display(Name = "Give the date of the restraint, confiscation order or civil recovery")]
        public DateViewModel Date { get; set; }

        [Required]
        [Display(Name = "Give details of the restraint, confiscation order or civil recovery")]
        public string Description { get; set; }
    }

    public class UnspentConvictionsViewModel : YesNoViewModel, IValidatable, ICanView<PrincipalAuthorityViewModel>,
        ICanView<DirectorOrPartnerViewModel>, ICanView<AlternativeBusinessRepresentativeViewModel>, ICanView<NamedIndividualViewModel>
    {
        [Required]
        [Display(Name = "Have you any unspent criminal convictions, or alternative sanctions or penalties for proven offences?")]
        public bool? HasUnspentConvictions { get; set; }

        [Display(Name = "Details of unspent criminal convictions, alternative sanctions or penalties for proven offences")]
        public IEnumerable<UnspentConvictionViewModel> UnspentConvictions { get; set; }

        public UnspentConvictionsViewModel()
        {
            UnspentConvictions = new List<UnspentConvictionViewModel>();
        }

        public void Validate()
        {
            if (!HasUnspentConvictions.HasValue)
            {
                IsValid = false;
                return;
            }

            if (HasUnspentConvictions.Value && !UnspentConvictions.Any())
            {
                IsValid = false;
                return;
            }

            foreach (var conviction in UnspentConvictions)
            {
                conviction.Validate();
            }
            IsValid = UnspentConvictions.All(c => c.IsValid);
        }

        public bool IsValid { get; set; }

        public bool CanView(PrincipalAuthorityViewModel parent)
        {
            return parent.UnspentConvictionsViewModel.HasUnspentConvictions ?? false;
        }

        public bool CanView(DirectorOrPartnerViewModel parent)
        {
            return parent.UnspentConvictionsViewModel.HasUnspentConvictions ?? false;
        }

        public bool CanView(AlternativeBusinessRepresentativeViewModel parent)
        {
            return parent.UnspentConvictionsViewModel.HasUnspentConvictions ?? false;
        }

        public bool CanView(NamedIndividualViewModel parent)
        {
            return parent.UnspentConvictionsViewModel.HasUnspentConvictions ?? false;
        }
    }

    public class UnspentConvictionViewModel : Validatable
    {
        public int Id { get; set; }

        [UIHint("_NullableDateTime")]
        [DateRequired(ErrorMessage = "The date of the convictions / sanctions / penalties field is required.")]
        [Display(Name = "Give the date of the convictions / sanctions / penalties")]
        public DateViewModel Date { get; set; }

        [Required]
        [Display(Name = "Give details of the convictions / sanctions / penalties")]
        public string Description { get; set; }
    }

    public class OffencesAwaitingTrialViewModel : YesNoViewModel, IValidatable, ICanView<PrincipalAuthorityViewModel>,
        ICanView<DirectorOrPartnerViewModel>, ICanView<AlternativeBusinessRepresentativeViewModel>, ICanView<NamedIndividualViewModel>
    {
        [Required]
        [Display(Name = "Have you been interviewed or charged with an offence that is awaiting trial or awaiting a decision on an alternative sanction or penalty?")]
        public bool? HasOffencesAwaitingTrial { get; set; }

        [Display(Name = "Details of offences awaiting trial or decision on an alternative sanction or penalty")]
        public IEnumerable<OffenceAwaitingTrialViewModel> OffencesAwaitingTrial { get; set; }

        public OffencesAwaitingTrialViewModel()
        {
            OffencesAwaitingTrial = new List<OffenceAwaitingTrialViewModel>();
        }

        public void Validate()
        {
            if (!HasOffencesAwaitingTrial.HasValue)
            {
                IsValid = false;
                return;
            }

            if (HasOffencesAwaitingTrial.Value && !OffencesAwaitingTrial.Any())
            {
                IsValid = false;
                return;
            }

            foreach (var offence in OffencesAwaitingTrial)
            {
                offence.Validate();
            }
            IsValid = OffencesAwaitingTrial.All(o => o.IsValid);
        }

        public bool IsValid { get; set; }

        public bool CanView(PrincipalAuthorityViewModel parent)
        {
            return parent.OffencesAwaitingTrialViewModel.HasOffencesAwaitingTrial ?? false;
        }

        public bool CanView(DirectorOrPartnerViewModel parent)
        {
            return parent.OffencesAwaitingTrialViewModel.HasOffencesAwaitingTrial ?? false;
        }

        public bool CanView(AlternativeBusinessRepresentativeViewModel parent)
        {
            return parent.OffencesAwaitingTrialViewModel.HasOffencesAwaitingTrial ?? false;
        }

        public bool CanView(NamedIndividualViewModel parent)
        {
            return parent.OffencesAwaitingTrialViewModel.HasOffencesAwaitingTrial ?? false;
        }
    }

    public class OffenceAwaitingTrialViewModel : Validatable
    {
        public int Id { get; set; }

        [UIHint("_NullableDateTime")]
        [DateRequired(ErrorMessage = "The date of the alleged offence / sanction / penalty field is required.")]
        [Display(Name = "Give the date of the alleged offence / sanction / penalty")]
        public DateViewModel Date { get; set; }

        [Required]
        [Display(Name = "Give details of the alleged offence / sanction / penalty")]
        public string Description { get; set; }
    }

    public class PreviousLicenceViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Have you previously held or currently hold a GLA/GLAA licence, been named on another GLA/GLAA licence, worked for another GLA/GLAA licence holder or advised another GLA/GLAA licence holder within the last 10 years?")]
        public bool? HasPreviouslyHeldLicence { get; set; }

        [RequiredIf(ErrorMessage = "The Licence Details field is required.")]
        [Display(Name = "Give details of the previously held GLAA licence")]
        public string PreviousLicenceDescription { get; set; }

        public bool IsRequired => HasPreviouslyHeldLicence ?? false;
    }
}