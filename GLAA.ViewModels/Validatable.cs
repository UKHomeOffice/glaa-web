using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace GLAA.ViewModels
{
    public abstract class Validatable : IValidatable
    {
        public virtual void Validate()
        {
            var invalidModelFields = new List<string>();
            foreach (var prop in GetType().GetProperties())
            {
                try
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
                catch (TargetInvocationException tie)
                {
                    var e = tie;
                }
            }
            IsValid = !invalidModelFields.Any();
        }

        public bool IsValid { get; set; }
    }
}
