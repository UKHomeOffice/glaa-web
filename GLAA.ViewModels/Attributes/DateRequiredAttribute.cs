using System.ComponentModel.DataAnnotations;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.Core.Attributes
{
    public class DateRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is DateViewModel val)
            {
                return val.Date.HasValue
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage);
            }

            return new ValidationResult($"Incorrect usage of {nameof(DateRequiredAttribute)}");
        }
    }
}
