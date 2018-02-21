using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels
{
    public interface INeedCounties
    {
        IEnumerable<SelectListItem> Counties { set; }
    }
}
