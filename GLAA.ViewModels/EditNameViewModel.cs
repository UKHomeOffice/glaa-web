using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels
{
    public class EditNameViewModel
    {
        [Display(Name = "Current title")]
        public string CurrentTitle { get; set; }

        [Required]
        [Display(Name = "New title")]
        public string NewTitle { get; set; }

        [Display(Name = "Current first name")]
        public string CurrentFirstName { get; set; }

        [Required]
        [Display(Name = "New first name")]
        public string NewFirstName { get; set; }

        [Display(Name = "Current middle names")]
        public string CurrentMiddleName { get; set; }

        [Required]
        [Display(Name = "New middle names")]
        public string NewMiddleName { get; set; }

        [Display(Name = "Current last name")]
        public string CurrentLastName { get; set; }

        [Required]
        [Display(Name = "New last name")]
        public string NewLastName { get; set; }

        public string FullName => (string.IsNullOrEmpty(CurrentTitle) ? string.Empty : $"{CurrentTitle} ") +
                                  (string.IsNullOrEmpty(CurrentFirstName) ? string.Empty : $"{CurrentFirstName} ") +
                                  (string.IsNullOrEmpty(CurrentMiddleName) ? string.Empty : $"{CurrentMiddleName} ") +
                                  (string.IsNullOrEmpty(CurrentLastName) ? string.Empty : $"{CurrentLastName} ");
    }
}
