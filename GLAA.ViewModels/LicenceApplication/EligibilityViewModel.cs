using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GLAA.ViewModels.LicenceApplication
{
    public class EligibilityViewModel : Validatable
    {
        public EligibilityViewModel()
        {
            FullName = new PrincipalAuthorityFullNameViewModel();
            EmailAddress = new PrincipalAuthorityEmailAddressViewModel();
            Address = new AddressViewModel();
            CommunicationPreference = new CommunicationPreferenceViewModel();
            Password = new PasswordViewModel();
        }

        public PrincipalAuthorityFullNameViewModel FullName { get; set; }
        public PrincipalAuthorityEmailAddressViewModel EmailAddress { get; set; }
        public AddressViewModel Address { get; set; }
        public CommunicationPreferenceViewModel CommunicationPreference { get; set; }
        public PasswordViewModel Password { get; set; }
    }

    //TODO Replace FullNameViewModel with this
    public class PrincipalAuthorityFullNameViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }

    public class PrincipalAuthorityEmailAddressViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "This will be the email address you use to log in to the system once it has been verified.")]
        public string EmailAddress { get; set; }
    }

    public class PasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-type password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class EligibilitySummaryViewModel : RegistrationViewModel, IValidatable
    {
        [Required]
        public bool? ContinueApplication { get; set; }
        public string SuppliesWorkersText{ get; set; }
        public string IndustriesText { get; set; }
        public int ApplicationFee { get; set; }
        public int InspectionFee { get; set; }
        public bool EmailAlreadyRegistered { get; set; }

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

    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
