using System.ComponentModel.DataAnnotations;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.Attributes
{
    public class RequiredIfUkAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as IUkOnly;

            if (model != null && model.IsUk && value == null)
            {
                return new ValidationResult($"{validationContext.DisplayName ?? validationContext.MemberName} is required if the address is UK based");
            }

            return ValidationResult.Success;
        }
    }
}