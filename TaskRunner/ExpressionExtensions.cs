using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests
{
    static class ExpressionExtensions
    {
        public static PropertyInfo GetPropertyInfo<T, P>(this Expression<Func<T, P>> func)
        {
            var body = func.Body as MemberExpression;

            if (body == null)
            {
                throw new Exception("PropertyExpression required");
            }

            var propertyInfo = body.Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new Exception("PropertyExpression required");
            }

            return propertyInfo;
        }
    }
}