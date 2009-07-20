namespace FluentMvc.Configuration
{
    using System;
    using ActionResultFactories;
    using Constraints;

    public class FluentMvcObjectFactory : IFluentMvcObjectFactory
    {
        public virtual IConstraint CreateConstraint(Type type)
        {
            return (IConstraint)Activator.CreateInstance(type);
        }

        public T CreateFactory<T>() where T : IActionResultFactory
        {
            return (T) Activator.CreateInstance(typeof (T));
        }

        public virtual T CreateFilter<T>(Type concreteFilter)
        {
            return (T)Activator.CreateInstance(concreteFilter);
        }
    }
}