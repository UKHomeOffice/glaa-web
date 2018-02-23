using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GLAA.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName => (string.IsNullOrEmpty(Title) ? string.Empty : $"{Title} ") +
                                  (string.IsNullOrEmpty(FirstName) ? string.Empty : $"{FirstName} ") +
                                  (string.IsNullOrEmpty(MiddleName) ? string.Empty : $"{MiddleName} ") +
                                  (string.IsNullOrEmpty(LastName) ? string.Empty : $"{LastName} ");

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }
}
