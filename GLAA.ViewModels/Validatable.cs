using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GLAA.ViewModels
{
    public abstract class Validatable : IValidatable
    {
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
}
