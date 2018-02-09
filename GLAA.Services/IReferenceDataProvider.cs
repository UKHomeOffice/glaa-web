using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services
{
    public interface IReferenceDataProvider
    {
        IEnumerable<SelectListItem> GetCountries();
        IEnumerable<SelectListItem> GetCounties();
    }
}
