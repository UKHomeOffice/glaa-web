using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GLAA.ViewModels.Core.Attributes
{
    public class CollectionRequiredIfAttribute : RequiredIfAttribute
    {
        protected override bool ValueIsValid(object value, ValidationContext context)
        {
            if (value is IEnumerable<object> collection)
            {
                return collection.Any();
            }
            return false;
        }
    }
}
