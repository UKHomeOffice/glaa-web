using System.Linq;
using GLAA.Services;
using GLAA.ViewModels;

namespace GLAA.Web.FormLogic
{
    public class LicenceApplicationFormDefinition : IFormDefinition
    {
        private readonly IFieldConfiguration fieldConfiguration;

        public LicenceApplicationFormDefinition(IFieldConfiguration fieldConfiguration)
        {
            this.fieldConfiguration = fieldConfiguration;
        }

        public object GetViewModel<TParent>(FormSection section, string actionName, TParent parent)
        {
            var page = GetPageDefinition(section, actionName);

            if (page == null || parent == null)
            {
                return null;
            }

            return page.GetViewModelExpressionForPage(parent);
        }

        public bool CanViewPage<TParent>(FormSection section, string actionName, TParent parent)
        {
            var page = GetPageDefinition(section, actionName);

            if (page.OverrideViewCondition)
            {
                return true;
            }

            var model = GetViewModel(page, parent) as ICanView<TParent>;

            return model == null || model.CanView(parent);
        }

        public int GetSectionLength(FormSection section)
        {
            return fieldConfiguration.Fields[section].Length;
        }

        private FormPageDefinition GetPageDefinition(FormSection section, string actionName)
        {
            if (!fieldConfiguration.Fields.ContainsKey(section) || fieldConfiguration.Fields[section].None(f => f.MatchesName(actionName)))
            {
                return null;
            }
            return fieldConfiguration.Fields[section].Single(f => f.MatchesName(actionName));
        }

        private static object GetViewModel<TParent>(FormPageDefinition page, TParent parent)
        {
            return page.GetViewModelExpressionForPage(parent);
        }
    }
}