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
            QueryModelExpression = null;
        }

        public FormPageDefinition(string subModelName, bool overrideViewCondition = false)
        {
            SubModelName = subModelName;
            OverrideViewCondition = overrideViewCondition;
            QueryModelExpression = null;
        }

        public FormPageDefinition(Func<object, object, IQueryCollection> expression, bool overrideViewCondition = false)
        {
            SubModelName = string.Empty;
            OverrideViewCondition = overrideViewCondition;
            QueryModelExpression = expression;
        }

        public string SubModelName { get; }

        public bool OverrideViewCondition { get; }

        public Func<object, object, IQueryCollection> QueryModelExpression { get; }

        public object GetViewModelExpressionForPage<TParent>(TParent parent, IQueryCollection query = null)
        {
            if (string.IsNullOrEmpty(SubModelName) && QueryModelExpression == null)
            {
                return parent;
            }

            if (QueryModelExpression != null)
            {
                return QueryModelExpression(parent, query);
            }

            var propExpression = Expression.Property(Expression.Constant(parent), SubModelName);
            var lambda = Expression.Lambda<Func<object>>(propExpression);
            return lambda.Compile()();
        }
    }
}