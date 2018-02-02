using GLAA.Web.Controllers;

namespace GLAA.Web.FormLogic
{
    public interface IFormDefinition
    {
        object GetViewModel<TParent>(FormSection section, int id, TParent parent);

        bool CanViewNextModel<TParent>(FormSection section, int id, TParent parent);

        int GetSectionLength(FormSection section);

        int GetViewNumber(FormSection section, string submittedViewName);

        int GetNextViewNumber(FormSection section, string submittedViewName);

        bool NextViewIsFinalView(FormSection section, string submittedViewName);

        string GetViewName(FormSection section, int id);
    }
}