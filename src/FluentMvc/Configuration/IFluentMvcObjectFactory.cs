namespace FluentMvc.Configuration
{
    using System;
    using Constraints;

    public interface IFluentMvcObjectFactory
    {
        // TODO: Review
        IConstraint CreateConstraint(Type type);
        T Resolve<T>();
        T Resolve<T>(Type concreteFilter);
    }
}