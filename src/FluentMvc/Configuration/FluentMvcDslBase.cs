namespace FluentMvc.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ActionResultFactories;
    using Constraints;
    using Registrations;

    public abstract class FluentMvcDslBase<TDsl> where TDsl : FluentMvcDslBase<TDsl>
    {
        protected IActionFilterRegistry actionFilterRegistry;
        protected readonly IDictionary<Type, HashSet<TransientRegistration>> constaintRegistrations = new Dictionary<Type, HashSet<TransientRegistration>>();
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
            var constraint = constraintDsl.GetConstraintRegistrations(objectFactory);

            factory.SetConstraint(constraint.Select(x => x.Constraint));

            return WithResultFactory(factory);
        }

        public TDsl WithResultFactory<TFactory>(ConstraintDsl constraintDsl)
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

        public TDsl WithResultFactory(IActionResultFactory resultFactory)
        {
            Convention.Factories.Enqueue(resultFactory);

            return (TDsl)this;
        }

        public TDsl WithResultFactory(IActionResultFactory defaultFactory, bool isDefault)
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
            constaintRegistrations.Add(typeof(TFilter), new HashSet<TransientRegistration> { new InstanceRegistration(new PredefinedConstraint(true), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance) });
            return (TDsl)this;
        }

        public TDsl WithFilter<T>(ConstraintDsl constraint)
        {
            return WithFilter<T>(constraint.GetConstraintRegistrations(objectFactory));
        }

        public TDsl WithFilter<T>(T filterInstance)
        {
            var registration = new FilterInstanceInstanceRegistration(new PredefinedConstraint(true), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance, filterInstance);

            RegisterFilter(filterInstance.GetType(), new[] {registration});

            return (TDsl)this;
        }

        public TDsl WithFilter<T>(T filterInstance, ConstraintDsl constraint)
        {
            IEnumerable<TransientRegistration> registrations = constraint.GetConstraintRegistrations(objectFactory);

            RegisterFilter(filterInstance.GetType(), registrations.Select(x => new FilterInstanceInstanceRegistration(x.Constraint, x.ActionDescriptor, x.ControllerDescriptor, filterInstance)).Cast<TransientRegistration>());

            return (TDsl)this;
        }

        public TDsl WithFilter<TFilter>(IEnumerable<TransientRegistration> registrations)
        {
            Type filterType = typeof(TFilter);
            RegisterFilter(filterType, registrations);

            return (TDsl)this;
        }

        private void RegisterFilter(Type filterType, IEnumerable<TransientRegistration> constraints)
        {
            if ( !constaintRegistrations.ContainsKey(filterType))
                constaintRegistrations.Add(filterType, new HashSet<TransientRegistration>());

            constaintRegistrations[filterType] = new HashSet<TransientRegistration>(constaintRegistrations[filterType].Concat(constraints));
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