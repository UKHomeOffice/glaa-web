using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels
{
    public class EditNameViewModel
    {
        [Display(Name = "Current full name")]
        public string Current { get; set; }

        [Required]
        [Display(Name = "New full name")]
        public string New { get; set; }
    }
}
