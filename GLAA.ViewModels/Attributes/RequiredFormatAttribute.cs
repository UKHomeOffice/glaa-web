using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GLAA.ViewModels.Attributes
{
    public class RequiredFormatAttribute : RequiredAttribute
    {
        private string templateFormat;
        private object[] interpolationCandidates;
        public RequiredFormatAttribute(string templateFormat, params object[] interpolationCandidates)
        {
            this.templateFormat = templateFormat;
            this.interpolationCandidates = interpolationCandidates;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = value != null;

            if (!isValid)
            {
                return new ValidationResult(string.Format(templateFormat, interpolationCandidates));
            }
            return ValidationResult.Success;
        }
    }
}
