using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GLAA.Services
{
    public static class LinqHelpers
    {
        // I'm fed up with ReSharper suggesting `All` for this, it's not readable
        public static bool None<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return !collection.Any(predicate);
        }

        // from https://stackoverflow.com/questions/9601707/how-to-set-property-value-using-expressions
        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberExpression,
            TValue value)
        {
            var memberSelectorExpression = memberExpression.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }
    }
}
