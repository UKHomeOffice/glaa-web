using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace GLAA.Web.FormLogic
{
    public class FormPageDefinition
    {
        public FormPageDefinition(string actionName)
        {
            SubModelName = string.Empty;
            OverrideViewCondition = false;
            ActionName = actionName;
        }

        public FormPageDefinition(string subModelName, string actionName, bool overrideViewCondition = false)
        {
            SubModelName = subModelName;
            OverrideViewCondition = overrideViewCondition;
            ActionName = actionName;
        }

        public string SubModelName { get; }

        public bool OverrideViewCondition { get; }

        public string ActionName { get; }

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
    }
}