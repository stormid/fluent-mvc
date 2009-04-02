namespace FluentMvc.Utils
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    public static class DescriptorExtensions
    {
        public static ActionDescriptor CreateActionDescriptor<T>(this Expression<Func<T, object>> action)
            where T : Controller
        {
            var methodCall = ReflectionHelper.GetMethod(action);
            var controllerDescriptor = new ReflectedControllerDescriptor(typeof(T));
            return new ReflectedActionDescriptor(methodCall, methodCall.Name, controllerDescriptor);
        }
    }
}