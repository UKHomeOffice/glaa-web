using System.ComponentModel.DataAnnotations;
using GLAA.ViewModels.Attributes;

namespace GLAA.ViewModels.LicenceApplication
{
    public class DeclarationViewModel : IValidatable
    {
        [Display(Name = "Declaration part 1", Description = "I understand and accept that the information and personal data contained in this document may also be shared and checked with other Government Departments and their agencies, enforcement bodies plus overseas agencies where necessary and where there is a legal basis to do so.")]
        [RequireTrue(ErrorMessage = "You must agree to all declaration statements")]        
        public bool AgreedToStatementOne { get; set; }

        [Display(Name = "Declaration part 2", Description = "I declare that the information and personal data given in this form and any supporting information is correct to the best of my knowledge and belief and that I have not deliberately omitted any necessary information or made an incorrect statement.  I understand that if deliberate omissions or incorrect statements have been made, my application may be refused without further consideration or, if a licence has been issued, it may be liable to immediate revocation.  I further understand that deliberate omissions or incorrect statements may leave me liable to prosecution and/or sanction.")]
        [RequireTrue(ErrorMessage = "You must agree to all declaration statements")]
        public bool AgreedToStatementTwo { get; set; }

        [Display(Name = "Declaration part 3", Description = "I understand that the GLAA may contact me by telephone and ask for further relevant details regarding the information and personal data I have provided.  I understand that this subsequent information may be used in the assessment of my application and ongoing management of the licence.")]
        [RequireTrue(ErrorMessage = "You must agree to all declaration statements")]
        public bool AgreedToStatementThree { get; set; }

        [Display(Name = "Declaration part 4", Description = "I understand I must notify the GLAA within 20 working days of any significant changes to the information recorded on this form, except for changes to the annual turnover which must be declared at renewal.")]
        [RequireTrue(ErrorMessage = "You must agree to all declaration statements")]
        public bool AgreedToStatementFour { get; set; }

        [Display(Name = "Declaration part 5", Description = "I declare that I am the person who has reviewed this form.")]
        [RequireTrue(ErrorMessage = "You must agree to all declaration statements")]
        public bool AgreedToStatementFive { get; set; }

        [Display(Name = "Declaration part 6", Description = "I declare that I am the Principle Authority and that I have reviewed this Application Form in its entirety, agree with and am bound by the information provided.")]
        [RequireTrue(ErrorMessage = "You must agree to all declaration statements")]
        public bool AgreedToStatementSix { get; set; }

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
