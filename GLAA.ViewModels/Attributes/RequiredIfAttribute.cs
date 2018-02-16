using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels.Core.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        // TODO make this virtual and remove a few specific implementations
        protected virtual bool ValueIsValid(object value, ValidationContext context)
        {
            return !string.IsNullOrEmpty(value?.ToString());
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (context.ObjectInstance is IRequiredIf requiredIf)
            {
                if (requiredIf.IsRequired)
                {
                    return ValueIsValid(value, context) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }

            return new ValidationResult($"Incorrect usage of {GetType()}");
        }
    }
}
