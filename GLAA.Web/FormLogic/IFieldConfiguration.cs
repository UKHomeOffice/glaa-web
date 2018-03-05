using System.Collections.Generic;

namespace GLAA.Web.FormLogic
{
    public interface IFieldConfiguration
    {
        IDictionary<FormSection, FormPageDefinition[]> Fields { get; set; }

        FormPageDefinition[] this[FormSection section] { get; }
    }
}