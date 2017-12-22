using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels
{
    public class CheckboxRequiredAttribute : ValidationAttribute
    {
        public CheckboxRequiredAttribute()
        {
            ErrorMessage = "Please select at least one item";
        }

        public CheckboxRequiredAttribute(string msg)
        {
            ErrorMessage = msg;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var lists = value as IEnumerable<ICheckboxList>;

            if (lists != null && lists.Any(c => c.Checked))
            {
                return ValidationResult.Success;
            }
            
            return new ValidationResult(ErrorMessage);
        }
    }
}
