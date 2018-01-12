using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels
{
    public class EditEmailViewModel
    {
        [Display(Name = "Current email address")]
        public string Current { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "New email address")]
        public string New { get; set; }
    }
}
