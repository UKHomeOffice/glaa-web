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

        public FormPageDefinition GetLastPage(FormSection section)
        {
            return fieldConfiguration.Fields[section].Last();
        }

        public bool IsNextPageLastPage(FormSection section, string actionName)
        {
            return fieldConfiguration.Fields[section][GetSectionLength(section) - 2].MatchesName(actionName);
        }

        public FormPageDefinition GetNextPage(FormSection section, string actionName)
        {
            for (var i = 0; i < GetSectionLength(section) - 1; i++)
            {
                if (fieldConfiguration.Fields[section][i].MatchesName(actionName))
                {
                    return fieldConfiguration.Fields[section][i + 1];
                }
            }

            return fieldConfiguration.Fields[section][GetSectionLength(section) - 1];
        }

        public FormPageDefinition GetPreviousPage(FormSection section, string actionName)
        {
            for (var i = GetSectionLength(section) - 1; i > 0; i--)
            {
                if (fieldConfiguration.Fields[section][i].MatchesName(actionName))
                {
                    return fieldConfiguration.Fields[section][i - 1];
                }
            }

            return fieldConfiguration.Fields[section][GetSectionLength(section) - 1];
        }

        public FormPageDefinition GetNextPossibleAction<TParent>(FormSection section, string actionName, TParent parent)
        {
            var matchLocation = 0;
            for (var i = 0; i < GetSectionLength(section) - 1; i++)
            {
                if (fieldConfiguration.Fields[section][i].MatchesName(actionName))
                {
                    matchLocation = i;
                }

                if (i > matchLocation && CanViewPage(section, fieldConfiguration.Fields[section][i].ActionName, parent))
                {
                    return fieldConfiguration.Fields[section][i];
                }
            }

            return GetLastPage(section);
        }

        public FormPageDefinition GetPreviousPossibleAction<TParent>(FormSection section, string actionName, TParent parent)
        {
            var matchLocation = GetSectionLength(section) - 1;
            for (var i = GetSectionLength(section) - 1; i > 0; i--)
            {
                if (fieldConfiguration.Fields[section][i].MatchesName(actionName))
                {
                    matchLocation = i;
                }

                if (matchLocation < i && CanViewPage(section, fieldConfiguration.Fields[section][i].ActionName, parent))
                {
                    return fieldConfiguration.Fields[section][i];
                }
            }

            return GetLastPage(section);
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