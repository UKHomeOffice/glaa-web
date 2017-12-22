using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GLAA.Domain.Models;
using GLAA.ViewModels.Core;
using GLAA.ViewModels.Core.Attributes;

namespace GLAA.ViewModels.LicenceApplication
{
    public class PrincipalAuthorityViewModel : PersonViewModel, IValidatable
    {
        public PrincipalAuthorityViewModel()
        {
            IsDirector = new IsDirectorViewModel();            
            PreviousExperience = new PreviousExperienceViewModel();
            PrincipalAuthorityConfirmation = new PrincipalAuthorityConfirmationViewModel();
            PreviousTradingNames = new PreviousTradingNamesViewModel();
            PrincipalAuthorityRightToWorkViewModel = new PrincipalAuthorityRightToWorkViewModel();
        }
        
        public int? Id { get; set; }

        public int? DirectorOrPartnerId { get; set; }

        public IsDirectorViewModel IsDirector { get; set; }
        public PrincipalAuthorityConfirmationViewModel PrincipalAuthorityConfirmation { get; set; }
        public PreviousExperienceViewModel PreviousExperience { get; set; }
        public PreviousTradingNamesViewModel PreviousTradingNames { get; set; }
        public PrincipalAuthorityRightToWorkViewModel PrincipalAuthorityRightToWorkViewModel { get; set; }
        public LegalStatusEnum? LegalStatus { get; set; }

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

        public bool IsValid { get; set; }
    }

    public class IsDirectorViewModel : YesNoViewModel, ICanView<PrincipalAuthorityViewModel>
    {
        [Required]
        [Display(Name = "Are you a director of the company?")]
        public bool? IsDirector { get; set; }

        public bool CanView(PrincipalAuthorityViewModel parent)
        {
            return !parent.LegalStatus.HasValue ||
                   parent.LegalStatus.Value == LegalStatusEnum.LimitedCompany ||
                   parent.LegalStatus.Value == LegalStatusEnum.Partnership;
        }
    }

    public class PrincipalAuthorityConfirmationViewModel : ICanView<PrincipalAuthorityViewModel>, IRequiredIf
    {                        
        public bool? IsDirector { get; set; }

        [RequiredIf(ErrorMessage = "You must confirm that you will provide the required confirmation")]
        [Display(Name = "Please confirm that you will be providing the required confirmation")]
        public bool WillProvideConfirmation { get; set; }

        public bool CanView(PrincipalAuthorityViewModel parent)
        {
            return !parent.LegalStatus.HasValue ||
                   parent.LegalStatus.Value == LegalStatusEnum.LimitedCompany ||
                   parent.LegalStatus.Value == LegalStatusEnum.Partnership;
        }

        public bool IsRequired
        {
            get
            {
                if (IsDirector.HasValue && !IsDirector.Value && !WillProvideConfirmation)
                {
                    return false;
                }
                return true;
            }
        }
    }

    public class PreviousExperienceViewModel
    {
        [Required]
        [Display(Name = "Previous Experience", Description = "To assist the GLAA in assessing your competency to perform the role of Principal Authority (Licensing Standard 1.2 - critical), please provide details of your previous experience in managing a relevant business or businesses (preferably within the last 5 years). Please include dates")]
        public string PreviousExperience { get; set; }
    }

    public class PreviousTradingNamesViewModel : YesNoViewModel, IValidatable, ICanView<PrincipalAuthorityViewModel>
    {
        [Required]
        [Display(Name = "Have you or your organisation traded under any other name in the last 5 years?")]
        public bool? HasPreviousTradingNames { get; set; }

        [Display(Name = "Details of previous trading names")]
        public IEnumerable<PreviousTradingNameViewModel> PreviousTradingNames { get; set; }

        public PreviousTradingNamesViewModel()
        {
            PreviousTradingNames = new List<PreviousTradingNameViewModel>();
        }

        public void Validate()
        {
            if (!HasPreviousTradingNames.HasValue)
            {
                IsValid = false;
                return;
            }

            if (HasPreviousTradingNames.Value && !PreviousTradingNames.Any())
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

        public bool CanView(PrincipalAuthorityViewModel parent)
        {
            return parent.PreviousTradingNames.HasPreviousTradingNames ?? false;
        }
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

    public class PrincipalAuthorityRightToWorkViewModel : IRequiredIf
    {
        public PrincipalAuthorityRightToWorkViewModel()
        {
            LeaveToRemainTo = new DateViewModel();
            LengthOfUKWork = new TimeSpanViewModel();
        }

        public List<PermissionToWork> AvailablePermissionToWork { get; set; } = new List<PermissionToWork>
        {
            new PermissionToWork { Id = 1, Name = "Yes - I am an EEA citizen", Checked = false },
            new PermissionToWork { Id = 2, Name = "Yes - I have a visa, work permit or other form of clearance to work", Checked = false },
            new PermissionToWork { Id = 3, Name = "No - I do not have permission to work in the UK", Checked = false }
        };

        [Required]
        [Display(Name = "Do you have the right to work in the UK?")]
        public PermissionToWorkEnum? RightToWorkInUk { get; set; }
        
        [RequiredIf(ErrorMessage = "The Visa/permit number field is required.")]
        [Display(Name = "Visa/permit number")]
        public string VisaNumber { get; set; }
        
        [RequiredIf(ErrorMessage = "The Immigration Status field is required.")]
        [Display(Name = "Immigration Status")]
        public string ImmigrationStatus { get; set; }
        
        [RequiredIf(ErrorMessage = "The Date leave to remain is due to expire field is required")]
        [Display(Name = "Date leave to remain is due to expire")]
        [UIHint("_NullableDateTime")]
        public DateViewModel LeaveToRemainTo { get; set; }
        
        [TimeSpanRequiredIf(ErrorMessage = "The How long have you worked in the UK? field is required")]
        [Display(Name = "How long have you worked in the UK?")]
        public TimeSpanViewModel LengthOfUKWork { get; set; }

        public bool LengthOfUKWorkIsValid(TimeSpanViewModel timeSpan)
        {
            if (!RightToWorkInUk.HasValue || RightToWorkInUk.Value != PermissionToWorkEnum.HasVisa)
            {
                return true;
            }
            return timeSpan.IsValid();
        }

        public bool IsRequired => RightToWorkInUk == PermissionToWorkEnum.HasVisa;
    }
}