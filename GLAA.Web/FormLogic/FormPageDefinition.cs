using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace GLAA.Web.FormLogic
{
    public class FormPageDefinition
    {
        public FormPageDefinition()
        {
            SubModelName = string.Empty;
            OverrideViewCondition = false;
        }

        [Obsolete]
        public FormPageDefinition(string subModelName, bool overrideViewCondition = false)
        {
            SubModelName = subModelName;
            OverrideViewCondition = overrideViewCondition;
        }

        public FormPageDefinition(string subModelName, string actionName, bool overrideViewCondition = false)
        {
            SubModelName = subModelName;
            ActionName = actionName;
            OverrideViewCondition = overrideViewCondition;
        }

        public string SubModelName { get; }

        public string ActionName { get; set; }

        public bool OverrideViewCondition { get; }

        public object GetViewModelExpressionForPage<TParent>(TParent parent, IQueryCollection query = null)
        {
            if (string.IsNullOrEmpty(SubModelName))
            {
                return parent;
            }

            var propExpression = Expression.Property(Expression.Constant(parent), SubModelName);
            var lambda = Expression.Lambda<Func<object>>(propExpression);
            return lambda.Compile()();
        }

        public bool MatchesName(string actionName)
        {
            return ActionName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}