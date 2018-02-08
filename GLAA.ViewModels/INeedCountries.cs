using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels
{
    public interface INeedCountries
    {
        IEnumerable<SelectListItem> Countries { set; }
    }
}
