namespace FluentMvc.Configuration
{
    using System;
    using ActionResultFactories;
    using Constraints;

    public interface IFluentMvcObjectFactory
    {
        // TODO: Review
        IConstraint CreateConstraint(Type type);
        T CreateFactory<T>() where T : IActionResultFactory;
        T CreateFilter<T>(Type concreteFilter);
    }
}