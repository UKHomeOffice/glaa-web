using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels.LicenceApplication
{
    public class ResumeApplicationViewModel
    {
        [Required]
        [RegularExpression(@"[a-zA-Z]{4,5}-\d{4}", ErrorMessage = "Please enter a valid application ID.")]
        [Display(Name = "Application ID", Description = "This is found in the email we sent you when you created you application")]
        public string ApplicationId { get; set; }
    }
}