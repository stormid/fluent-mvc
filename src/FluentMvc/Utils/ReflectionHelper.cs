namespace FluentMvc.Utils
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ReflectionHelper
    {
        public static MethodInfo GetMethod<T>(Expression<Func<T, object>> expression)
        {
            MethodCallExpression methodCall = (MethodCallExpression)expression.Body;
            return methodCall.Method;
        }

        public static bool CanBeCastTo<T>(this Type type)
        {
            return type.CanBeCastTo(typeof (T));
        }

        public static bool CanBeCastTo(this Type type, Type expected)
        {
            return expected.IsAssignableFrom(type);
        }

        public static bool CanBeCastTo<T>(this object instance)
        {
            return instance.GetType().CanBeCastTo<T>();
        }
    }
}