using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GLAA.ViewModels.Core.Attributes
{
    public class AssertThatAttribute : ValidationAttribute
    {
        public string MethodName { get; }

        public AssertThatAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public AssertThatAttribute(string methodName, Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
            MethodName = methodName;
        }
        public AssertThatAttribute(string methodName, string errorMessage) : base(errorMessage)
        {
            MethodName = methodName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var method = context.ObjectInstance.GetType().GetMethod(MethodName);

            if (method == null)
            {
                return new ValidationResult($"Incorrect usage of {nameof(AssertThatAttribute)}");
            }

            var result = method.Invoke(context.ObjectInstance, null);

            return (bool) result ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
