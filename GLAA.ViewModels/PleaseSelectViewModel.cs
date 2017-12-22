using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels
{
    public class PleaseSelectViewModel
    {        
        public IEnumerable<SelectListItem> PleaseSelectItem { get; set; } = new[]
        {
            new SelectListItem
            {
                Selected = false,
                Text = "Please select...",
                Value = string.Empty
            }
        };
    }
}
