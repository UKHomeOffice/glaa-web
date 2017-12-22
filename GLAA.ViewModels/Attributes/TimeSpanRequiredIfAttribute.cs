using System.ComponentModel.DataAnnotations;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.Core.Attributes
{
    public class TimeSpanRequiredIfAttribute : RequiredIfAttribute
    {
        protected override bool ValueIsValid(object value, ValidationContext context)
        {
            if (value is TimeSpanViewModel val)
            {
                return val.IsValid();
            }
            return false;
        }
    }
}
