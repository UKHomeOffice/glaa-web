using System;
using GLAA.ViewModels;
using GLAA.Web.Controllers;

namespace GLAA.Web.FormLogic
{
    public class LicenceApplicationFormDefinition : IFormDefinition
    {
        private readonly IFieldConfiguration fieldConfiguration;

        public LicenceApplicationFormDefinition(IFieldConfiguration fieldConfiguration)
        {
            this.fieldConfiguration = fieldConfiguration;
        }

        public object GetViewModel<TParent>(FormSection section, int id, TParent parent)
        {
            var page = GetPageDefinition(section, id);

            if (page == null || parent == null)
            {
                return null;
            }

            return page.GetViewModelForPage(parent);
        }

        public bool CanViewNextModel<TParent>(FormSection section, int id, TParent parent)
        {
            var page = GetPageDefinition(section, id);

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

        public int GetViewNumber(FormSection section, string submittedViewName)
        {
            return Array.FindIndex(fieldConfiguration.Fields[section],
                       f => f.ViewName.Equals(submittedViewName, StringComparison.InvariantCultureIgnoreCase)) + 1;
        }

        public int GetNextViewNumber(FormSection section, string submittedViewName)
        {
            return GetViewNumber(section, submittedViewName) + 1;
        }

        public bool NextViewIsFinalView(FormSection section, string submittedViewName)
        {
            return GetNextViewNumber(section, submittedViewName) + 1 == GetSectionLength(section);
        }

        public string GetViewName(FormSection section, int id)
        {
            //id is from 1
            return fieldConfiguration.Fields[section][id - 1].ViewName;
        }

        private FormPageDefinition GetPageDefinition(FormSection section, int id)
        {
            var index = id - 1;
            if (!fieldConfiguration.Fields.ContainsKey(section) || index >= fieldConfiguration.Fields[section].Length)
            {
                return null;
            }
            return fieldConfiguration.Fields[section][index];
        }

        private static object GetViewModel<TParent>(FormPageDefinition page, TParent parent)
        {
            return page.GetViewModelForPage(parent);
        }
    }
}