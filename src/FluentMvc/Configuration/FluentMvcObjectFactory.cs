namespace FluentMvc.Configuration
{
    using System;
    using System.Collections.Generic;
    using Constraints;

    public class FluentMvcObjectFactory : IFluentMvcObjectFactory
    {
        public virtual IConstraint CreateConstraint(Type type)
        {
            return (IConstraint)Activator.CreateInstance(type);
        }

        public T Resolve<T>()
        {
            return (T) Activator.CreateInstance(typeof (T));
        }

        public virtual T Resolve<T>(Type concreteFilter)
        {
            return (T)Activator.CreateInstance(concreteFilter);
        }
    }
}