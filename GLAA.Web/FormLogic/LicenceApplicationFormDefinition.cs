using System.Linq;
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

        [System.Obsolete]
        public object GetViewModel<TParent>(FormSection section, int id, TParent parent)
        {
            var page = GetPageDefinition(section, id);

            if (page == null || parent == null)
            {
                return null;
            }

            return page.GetViewModelExpressionForPage(parent);
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

        [System.Obsolete]
        public bool CanViewPage<TParent>(FormSection section, int id, TParent parent)
        {
            var page = GetPageDefinition(section, id);

            if (page.OverrideViewCondition)
            {
                return true;
            }

            var model = GetViewModel(page, parent) as ICanView<TParent>;

            return model == null || model.CanView(parent);
        }

        public bool CanViewPage<TParent>(FormSection section, string actionName, TParent parent)
        {
            var page = GetPageDefinition(section, actionName);

            if (page.OverrideViewCondition)
            {
                return true;
            }

            var model = page.GetViewModelExpressionForPage(parent) as ICanView<TParent>;

            return model == null || model.CanView(parent);
        }

        public int GetSectionLength(FormSection section)
        {
            return fieldConfiguration.Fields[section].Length;
        }

        [System.Obsolete]
        private FormPageDefinition GetPageDefinition(FormSection section, int id)
        {
            var index = id - 1;
            if (!fieldConfiguration.Fields.ContainsKey(section) || index >= fieldConfiguration.Fields[section].Length)
            {
                return null;
            }
            return fieldConfiguration.Fields[section][index];
        }

        private FormPageDefinition GetPageDefinition(FormSection section, string actionName)
        {
            return fieldConfiguration[section].FirstOrDefault(x => x.MatchesName(actionName));
        }

        private static object GetViewModel<TParent>(FormPageDefinition page, TParent parent)
        {
            return page.GetViewModelExpressionForPage(parent);
        }
    }
}