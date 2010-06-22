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
            return (T)Activator.CreateInstance(typeof (T));
        }

        public virtual T Resolve<T>(Type concreteType)
        {
            return (T)Activator.CreateInstance(concreteType);
        }

        public virtual void BuildUpProperties<T>(T filter)
        {
            // Default implentation does nothing
        }
    }
}