using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GLAA.ViewModels.LicenceApplication
{
    public class EligibilityViewModel : Validatable
    {
        public SuppliesWorkersViewModel SuppliesWorkers { get; set; } = new SuppliesWorkersViewModel();

        public OperatingIndustriesViewModel OperatingIndustries { get; set; } = new OperatingIndustriesViewModel();

        public TurnoverViewModel Turnover { get; set; } = new TurnoverViewModel();

        public EligibilitySummaryViewModel EligibilitySummary { get; set; }

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

    public class SuppliesWorkersViewModel : YesNoViewModel
    {
        [Required]
        public bool? SuppliesWorkers { get; set; }
    }
}
