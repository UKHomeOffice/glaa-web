using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.Admin
{
    public class AdminUserViewModel : UserViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        public IEnumerable<SelectListItem> AvailableRoles { get; set; }
    }
}
