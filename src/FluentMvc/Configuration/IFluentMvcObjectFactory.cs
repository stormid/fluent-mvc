namespace FluentMvc.Configuration
{
    using System;
    using ActionResultFactories;
    using Constraints;

    public interface IFluentMvcObjectFactory
    {
        IConstraint CreateConstraint(Type type);
        T CreateFactory<T>() where T : IActionResultFactory;
        T Resolve<T>(Type concreteType);
        void BuildUpProperties<T>(T filter);
    }
}