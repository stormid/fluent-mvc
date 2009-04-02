namespace FluentMvc.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ActionResultFactories;
    using Constraints;

    public abstract class FluentMvcDslBase<TDsl> where TDsl : FluentMvcDslBase<TDsl>
    {
        protected IActionFilterRegistry actionFilterRegistry;
        protected readonly IDictionary<Type, HashSet<TransientConstraintRegistration>> constaintRegistrations = new Dictionary<Type, HashSet<TransientConstraintRegistration>>();
        protected IFluentMvcObjectFactory objectFactory;
        public FluentMvcConvention Convention { get; protected set; }

        public TDsl WithConvention(FluentMvcConvention convention)
        {
            Convention = convention;
            return (TDsl)this;
        }

        public virtual TDsl WithConvention(Action<FluentMvcConvention> editConvention)
        {
            editConvention.Invoke(Convention);
            return (TDsl)this;
        }

        public virtual TDsl WithControllerFactory(IControllerFactory defaultControllerFactory)
        {
            Convention.ControllerFactory = defaultControllerFactory;
            return (TDsl)this;
        }

        public virtual TDsl AddResultFactory<TFactory>()
            where TFactory : IActionResultFactory
        {
            return AddResultFactory(objectFactory.Resolve<TFactory>());
        }

        public virtual TDsl AddResultFactory(IActionResultFactory factory, ConstraintDsl constraintDsl)
        {
            var constraint = constraintDsl.CreateConstraints(objectFactory);

            factory.OverrideConstraint(constraint);

            return AddResultFactory(factory);
        }

        public virtual TDsl AddResultFactory<TFactory>(ConstraintDsl constraintDsl)
            where TFactory : IActionResultFactory
        {
            IActionResultFactory factory = objectFactory.Resolve<TFactory>();
            return AddResultFactory(factory, constraintDsl);
        }

        public virtual TDsl AddResultFactory(IActionResultFactory resultFactory)
        {
            Convention.Factories.Enqueue(resultFactory);
            return (TDsl)this;
        }

        public virtual TDsl WithDefaultFactory(IActionResultFactory defaultFactory)
        {
            Convention.DefaultFactory = defaultFactory;
            return (TDsl)this;
        }

        public TDsl AddFilter<TFilter>()
        {
            constaintRegistrations.Add(typeof(TFilter), new HashSet<TransientConstraintRegistration> { new TransientConstraintRegistration(new PredefinedConstraint(true), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance)});
            return (TDsl)this;
        }

        public TDsl AddFilter<T>(ConstraintDsl apply)
        {
            return AddFilter<T>(apply.CreateConstraints(objectFactory));
        }

        public TDsl AddFilter<TFilter>(IEnumerable<TransientConstraintRegistration> constraints)
        {
            if ( !constaintRegistrations.ContainsKey(typeof(TFilter)))
                constaintRegistrations.Add(typeof(TFilter), new HashSet<TransientConstraintRegistration>());

            constaintRegistrations[typeof(TFilter)] = new HashSet<TransientConstraintRegistration>(constaintRegistrations[typeof(TFilter)].Concat(constraints));
            return (TDsl)this;
        }

        public TDsl ResolveWith(IFluentMvcObjectFactory factory)
        {
            objectFactory = factory;
            actionFilterRegistry.SetObjectFactory(factory);
            return (TDsl)this;
        }
    }
}