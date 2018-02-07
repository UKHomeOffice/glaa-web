using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GLAA.ViewModels.Attributes;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels
{
    public class SignUpViewModel : Validatable
    {
        public SignUpViewModel()
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

        [HiddenOptional]
        [Display(Name = "Middle names")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Full name")]
        public string FullName => (string.IsNullOrEmpty(Title) ? string.Empty : $"{Title} ") +
                                  (string.IsNullOrEmpty(FirstName) ? string.Empty : $"{FirstName} ") +
                                  (string.IsNullOrEmpty(MiddleName) ? string.Empty : $"{MiddleName} ") +
                                  (string.IsNullOrEmpty(LastName) ? string.Empty : $"{LastName} ");
    }

    public class PrincipalAuthorityEmailAddressViewModel
    {
        [Required(ErrorMessage = "The Email Address field is required")]
        [EmailAddress(ErrorMessage = "The Email Address field is not a valid e-mail address.")]
        [Display(Name = "This will be the email address you use to log in to the system once it has been verified.")]
        public string EmailAddress { get; set; }
    }

    public class PasswordViewModel : IValidatable
    {
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,}$", ErrorMessage = "Your password must be at least 8 characters long and contain at least one upper-case letter and at least one number.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-type password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Does the user have a password set in the DB?
        public bool HasPassword { get; set; }

        public void Validate()
        {
            IsValid = HasPassword;
        }

        public bool IsValid { get; set; }
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
