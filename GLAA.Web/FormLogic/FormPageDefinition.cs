using System;
using System.Linq;
using System.Linq.Expressions;

namespace GLAA.Web.FormLogic
{
    public class FormPageDefinition
    {

        public FormPageDefinition(string viewName, Type viewModelType, bool overrideViewCondition = false)
        {
            ViewName = viewName;
            ViewModelType = viewModelType;
            OverrideViewCondition = overrideViewCondition;
        }

        public bool OverrideViewCondition { get; }

        public Type ViewModelType { get; }

        public string ViewName { get; }

        public object GetViewModelForPage<TParent>(TParent parent)
        {
            if (ViewModelType == null)
            {
                return parent;
            }

            var prop = typeof(TParent).GetProperties().Single(p => p.PropertyType == ViewModelType);

            var propExpression = Expression.Property(Expression.Constant(parent), prop);
            var lambda = Expression.Lambda<Func<object>>(propExpression);
            return lambda.Compile()();
        }
    }
}