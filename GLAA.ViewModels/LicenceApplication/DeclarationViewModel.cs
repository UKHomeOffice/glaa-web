using System.ComponentModel.DataAnnotations;
using GLAA.ViewModels.Attributes;

namespace GLAA.ViewModels.LicenceApplication
{
    public class DeclarationViewModel : IValidatable
    {
        [Required]
        [Display(Name = "Signatory Name")]
        public string SignatoryName { get; set; }

        //[AssertThat("DateIsValid(SignatureDate)", ErrorMessage = "The Signature Date field is required.")]
        [Display(Name = "Signature Date", Description = "For example 31 3 1980")]
        [UIHint("_NullableDateTime")]
        public DateViewModel SignatureDate { get; set; }       

        public bool DateIsValid(DateViewModel date)
        {
            return date.Date.HasValue;
        }

        public void Validate()
        {
            var context = new ValidationContext(this);
            IsValid = Validator.TryValidateObject(this, context, null, true);
        }

        public bool IsValid { get; set; }
    }
}
