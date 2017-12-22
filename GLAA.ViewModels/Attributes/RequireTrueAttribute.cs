using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels.Attributes
{
    public class RequireTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = value is bool && (bool)value;
            if (!isValid)
            {
                return new ValidationResult($"{validationContext.DisplayName ?? validationContext.MemberName} must be accepted");
            }
            return ValidationResult.Success;
        }
    }
}