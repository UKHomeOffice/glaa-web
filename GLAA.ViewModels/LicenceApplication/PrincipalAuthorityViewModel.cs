using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GLAA.Domain.Models;
using GLAA.ViewModels.Core;
using GLAA.ViewModels.Core.Attributes;

namespace GLAA.ViewModels.LicenceApplication
{
    public class PrincipalAuthorityViewModel : PersonViewModel
    {
        public PrincipalAuthorityViewModel()
        {
            IsDirector = new IsDirectorViewModel();
            PreviousExperience = new PreviousExperienceViewModel();
            PrincipalAuthorityConfirmation = new PrincipalAuthorityConfirmationViewModel();
            PrincipalAuthorityRightToWorkViewModel = new PrincipalAuthorityRightToWorkViewModel();
        }

        public int? Id { get; set; }

        public int? DirectorOrPartnerId { get; set; }

        public IsDirectorViewModel IsDirector { get; set; }
        public PrincipalAuthorityConfirmationViewModel PrincipalAuthorityConfirmation { get; set; }
        public PreviousExperienceViewModel PreviousExperience { get; set; }
        public PrincipalAuthorityRightToWorkViewModel PrincipalAuthorityRightToWorkViewModel { get; set; }
        public LegalStatusEnum? LegalStatus { get; set; }
    }

    public class IsDirectorViewModel : YesNoViewModel, ICanView<PrincipalAuthorityViewModel>
    {
        [Required]
        [Display(Name = "Are you a director of the company?")]
        public bool? IsDirector { get; set; }

        public bool CanView(PrincipalAuthorityViewModel parent)
        {
            return !parent.LegalStatus.HasValue ||
                   parent.LegalStatus.Value == LegalStatusEnum.RegisteredCompany ||
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
            return (!parent.LegalStatus.HasValue ||
                    parent.LegalStatus.Value == LegalStatusEnum.RegisteredCompany ||
                    parent.LegalStatus.Value == LegalStatusEnum.Partnership)
                   && IsDirector.HasValue && !IsDirector.Value;
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

    public class PrincipalAuthorityRightToWorkViewModel : IRequiredIf
    {
        public PrincipalAuthorityRightToWorkViewModel()
        {
            LeaveToRemainTo = new DateViewModel();
            LengthOfUKWork = new TimeSpanViewModel();
        }

        public List<PermissionToWork> AvailablePermissionToWork { get; set; } = new List<PermissionToWork>
        {
            new PermissionToWork { Id = 1, Name = "Yes - I am an EEA citizen", Checked = false, EnumMappedTo = PermissionToWorkEnum.EEACitizen },
            new PermissionToWork { Id = 2, Name = "Yes - I have a visa, work permit or other form of clearance to work", Checked = false, EnumMappedTo = PermissionToWorkEnum.HasVisa },
            new PermissionToWork { Id = 3, Name = "No - I do not have permission to work in the UK", Checked = false, EnumMappedTo = PermissionToWorkEnum.NoPermission }
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