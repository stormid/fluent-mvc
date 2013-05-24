using System;

namespace FluentMvc.Conventions
{
    public interface IFilterConventionActivator
    {
        IFilterConvention Activate(Type type);
    }
}