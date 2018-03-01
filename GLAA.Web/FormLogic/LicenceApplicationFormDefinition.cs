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
            return fieldConfiguration[section].Length;
        }

        public FormPageDefinition GetLastPage(FormSection section)
        {
            return fieldConfiguration[section].Last();
        }

        public bool IsNextPageLastPage(FormSection section, string actionName)
        {
            return fieldConfiguration[section][GetSectionLength(section) - 2].MatchesName(actionName);
        }

        public FormPageDefinition GetNextPage(FormSection section, string actionName)
        {
            // Get the index of the page that matches this action name
            for (var i = 0; i < GetSectionLength(section); i++)
            {
                if (fieldConfiguration[section][i].MatchesName(actionName))
                {
                    // return the next page in the sequence
                    return fieldConfiguration[section][i + 1];
                }
            }

            // if in doubt, return the final (summary) screen
            return GetLastPage(section);
        }

        public FormPageDefinition GetPreviousPage(FormSection section, string actionName)
        {
            // Counting back from the end, find the page that matches this action name
            for (var i = GetSectionLength(section) - 1; i > 0; i--)
            {
                if (fieldConfiguration[section][i].MatchesName(actionName))
                {
                    // return the page before the match
                    return fieldConfiguration[section][i - 1];
                }
            }

            // if in doubt, return the final (summary) screen
            return GetLastPage(section);
        }

        public FormPageDefinition GetNextPossibleAction<TParent>(FormSection section, string actionName, TParent parent)
        {
            var matchLocation = 0;
            // Search through the list of pages for this section until we find the current one
            for (var i = 0; i < GetSectionLength(section) - 1; i++)
            {
                // Only start looking for the next possible action once we know where to start looking *from*
                if (fieldConfiguration[section][i].MatchesName(actionName))
                {
                    matchLocation = i;
                }

                // If we're past the current page, can we view this one?
                if (i > matchLocation && CanViewPage(section, fieldConfiguration[section][i].ActionName, parent))
                {
                    // If so, return it. If not, try the next page.
                    return fieldConfiguration[section][i];
                }
            }

            // if in doubt, return the final (summary) screen
            return GetLastPage(section);
        }

        public FormPageDefinition GetPreviousPossibleAction<TParent>(FormSection section, string actionName, TParent parent)
        {
            var matchLocation = GetSectionLength(section) - 1;
            // Search back through the list of pages until we find the current one
            for (var i = GetSectionLength(section) - 1; i > 0; i--)
            {
                // Only start looking for the previous possible action once we know where to start looking from
                if (fieldConfiguration[section][i].MatchesName(actionName))
                {
                    matchLocation = i;
                }
                // If we've gone back past the current page, can we view this one?
                if (matchLocation < i && CanViewPage(section, fieldConfiguration[section][i].ActionName, parent))
                {
                    // If so, return it. If not, try the previous page.
                    return fieldConfiguration[section][i];
                }
            }

            // if in doubt, return the final (summary) screen
            return GetLastPage(section);
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