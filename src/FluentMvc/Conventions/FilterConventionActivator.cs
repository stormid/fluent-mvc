using System;

namespace FluentMvc.Conventions
{
    public abstract class FilterConventionActivator : IFilterConventionActivator
    {
        public static readonly IFilterConventionActivator Default = new DefaultFilterConventionActivator();

        public abstract IFilterConvention Activate(Type type);
    }
}