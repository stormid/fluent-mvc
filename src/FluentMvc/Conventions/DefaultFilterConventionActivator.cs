using System;

namespace FluentMvc.Conventions
{
    public class DefaultFilterConventionActivator : FilterConventionActivator
    {
        public override IFilterConvention Activate(Type type)
        {
            return Activator.CreateInstance(type) as IFilterConvention;
        }
    }
}