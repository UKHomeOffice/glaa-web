using System.Collections.Generic;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels
{
    public interface INeedStandards
    {
        List<CheckboxListItem> Standards { set; }
    }
}
