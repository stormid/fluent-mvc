namespace FluentMvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Configuration;

    public class FluentMvcConfiguration : FluentMvcDslBase<FluentMvcConfiguration>
    {
        private IActionResultRegistry actionResultRegistry;
        private IActionResultResolver actionResultResolver;
        private readonly IActionResultPipeline pipeline;

        public static Action<FluentMvcConfiguration> Configure { get; set; }

        private FluentMvcConfiguration()
        {
            pipeline = new ActionResultPipeline();
            Convention = new FluentMvcConvention();
            objectFactory = new FluentMvcObjectFactory();
        }

        private FluentMvcConfiguration(IActionResultResolver actionResultResolver, IActionFilterRegistry actionFilterRegistry, IActionResultRegistry actionResultRegistry)
            : this()
        {
            this.actionResultResolver = actionResultResolver;
            this.actionResultRegistry = actionResultRegistry;
            this.actionFilterRegistry = actionFilterRegistry;
        }

        public static FluentMvcConfiguration Create()
        {
            return Create(null, new ActionFilterRegistry(new FluentMvcObjectFactory()), new ActionResultRegistry());
        }

        public static FluentMvcConfiguration Create(IActionResultResolver actionResultResolver, IActionFilterRegistry actionFilterRegistry, IActionResultRegistry actionResultRegistry)
        {
            return new FluentMvcConfiguration(actionResultResolver, actionFilterRegistry, actionResultRegistry);
        }

        public virtual IControllerFactory BuildControllerFactory()
        {
            CreateDependencies();
            SetDefaultFactory();
            BuildActionResultFactoryPipeline();
            RegisterFilters();

            actionResultResolver.RegisterActionResultPipeline(pipeline);
            actionResultResolver.SetActionResultRegistry(actionResultRegistry);
            actionResultResolver.SetActionFilterRegistry(actionFilterRegistry);

            IControllerFactory controllerFactory = CreateControllerFactory();

            return controllerFactory;
        }

        private void CreateDependencies()
        {
            if ( actionFilterRegistry == null)
                actionFilterRegistry = new ActionFilterRegistry(objectFactory);

            if ( actionResultRegistry == null)
                actionResultRegistry = new ActionResultRegistry();

            if (actionResultResolver == null)
                actionResultResolver = new ActionResultResolver(actionResultRegistry, actionFilterRegistry, objectFactory);
        }

        private void SetDefaultFactory()
        {
            actionResultResolver.SetDefaultFactory(Convention.DefaultFactory);
        }

        private void BuildActionResultFactoryPipeline()
        {
            if (Convention.Factories.Count == 0)
                return;

            pipeline.Register(Convention.Factories);
        }

        private void RegisterFilters()
        {
            foreach (var registration in constaintRegistrations)
            {
                IEnumerable<ActionFilterRegistryItem> registryItem = CreateRegistryItem(registration);
                actionFilterRegistry.Add(registryItem);
            }
        }

        private IEnumerable<ActionFilterRegistryItem> CreateRegistryItem(KeyValuePair<Type, HashSet<TransientConstraintRegistration>> registration)
        {
            Type key = registration.Key;

            foreach (var reg in registration.Value)
            {
                yield return reg.CreateRegistryItem(key);
            }
        }

        protected IControllerFactory CreateControllerFactory()
        {
            return new FluentMvcControllerFactory(Convention.ControllerFactory, actionResultResolver);
        }

        public static IControllerFactory ConfigureAndBuildControllerFactory()
        {
            var config = new FluentMvcConfiguration();
            Configure(config);

            return config.BuildControllerFactory();
        }

        public void ExposeConfiguration(Action<FluentMvcConfiguration> action)
        {
            action(this);
        }
    }
}