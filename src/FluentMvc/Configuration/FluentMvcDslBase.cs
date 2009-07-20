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

        public virtual TDsl UsingControllerFactory(IControllerFactory defaultControllerFactory)
        {
            Convention.ControllerFactory = defaultControllerFactory;
            return (TDsl)this;
        }

        public virtual TDsl WithResultFactory<TFactory>()
            where TFactory : IActionResultFactory
        {
            return WithResultFactory(CreateFactory<TFactory>());
        }

        public virtual TDsl WithResultFactory(IActionResultFactory factory, ConstraintDsl constraintDsl)
        {
            var constraint = constraintDsl.CreateConstraints(objectFactory);

            factory.SetConstraint(constraint.Select(x => x.Constraint));

            return WithResultFactory(factory);
        }

        public virtual TDsl WithResultFactory<TFactory>(ConstraintDsl constraintDsl)
            where TFactory : IActionResultFactory
        {
            IActionResultFactory factory = CreateFactory<TFactory>();

            return WithResultFactory(factory, constraintDsl);
        }

        private IActionResultFactory CreateFactory<TFactory>()
            where TFactory : IActionResultFactory
        {
            return objectFactory.CreateFactory<TFactory>();
        }

        public virtual TDsl WithResultFactory(IActionResultFactory resultFactory)
        {
            Convention.Factories.Enqueue(resultFactory);

            return (TDsl)this;
        }

        public virtual TDsl WithResultFactory(IActionResultFactory defaultFactory, bool isDefault)
        {
            if (isDefault)
            {
                Convention.DefaultFactory = defaultFactory;
            }
            else
            {
                WithResultFactory(defaultFactory);
            }

            return (TDsl)this;
        }

        public TDsl WithResultFactory<TResultFactory>(bool isDefault) where TResultFactory : IActionResultFactory
        {
            return WithResultFactory(CreateFactory<TResultFactory>(), isDefault);
        }

        public TDsl WithFilter<TFilter>()
        {
            constaintRegistrations.Add(typeof(TFilter), new HashSet<TransientConstraintRegistration> { new TransientConstraintRegistration(new PredefinedConstraint(true), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance)});
            return (TDsl)this;
        }

        public TDsl WithFilter<T>(ConstraintDsl apply)
        {
            return WithFilter<T>(apply.CreateConstraints(objectFactory));
        }

        public TDsl WithFilter<T>(T filterInstance)
        {
            var registration = new TransientConstraintRegistration(filterInstance, new PredefinedConstraint(true), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance);

            RegisterFilter(filterInstance.GetType(), new[] {registration});

            return (TDsl)this;
        }

        public TDsl WithFilter<TFilter>(IEnumerable<TransientConstraintRegistration> constraints)
        {
            Type filterType = typeof(TFilter);
            RegisterFilter(filterType, constraints);

            return (TDsl)this;
        }

        private void RegisterFilter(Type filterType, IEnumerable<TransientConstraintRegistration> constraints)
        {
            if ( !constaintRegistrations.ContainsKey(filterType))
                constaintRegistrations.Add(filterType, new HashSet<TransientConstraintRegistration>());

            constaintRegistrations[filterType] = new HashSet<TransientConstraintRegistration>(constaintRegistrations[filterType].Concat(constraints));
        }

        public TDsl ResolveWith(IFluentMvcObjectFactory factory)
        {
            objectFactory = factory;
            if (actionFilterRegistry != null)
                actionFilterRegistry.SetObjectFactory(factory);

            return (TDsl)this;
        }
    }
}