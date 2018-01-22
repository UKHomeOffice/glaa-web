using System;
using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels.Core.Attributes
{
    public class AtLeast18Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {            
            var isDate = DateTime.TryParse(value.ToString(), out var dateValue);

            if(!isDate)
            {
                return new ValidationResult("Value is not a DateTime.");
            }


            if (dateValue >= DateTime.MinValue && dateValue <= DateTime.Now.AddYears(-18))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date is not at least 18 years");
            }

        }
    }
}
