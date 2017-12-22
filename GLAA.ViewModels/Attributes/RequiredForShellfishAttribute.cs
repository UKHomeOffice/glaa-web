using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels.Attributes
{
    public class RequiredForShellfishAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as IShellfishSection;

            if (model != null && model.IsShellfish && value == null)
            {
                return new ValidationResult($"{validationContext.DisplayName ?? validationContext.MemberName} is required if your are operating in the shellfish industry");
            }

            return ValidationResult.Success;
        }
    }
}