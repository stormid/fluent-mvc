namespace FluentMvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Configuration;
    using Configuration.Registrations;

    public class FluentMvcConfiguration : FluentMvcDslBase<FluentMvcConfiguration>
    {
        private IFluentMvcResolver fluentMvcResolver;

        public static Action<FluentMvcConfiguration> Configure { get; set; }

        private FluentMvcConfiguration()
        {
            pipeline = new ActionResultPipeline();
            Convention = new FluentMvcConvention();
            objectFactory = new FluentMvcObjectFactory();
        }

        private FluentMvcConfiguration(IFluentMvcResolver fluentMvcResolver, IActionFilterRegistry actionFilterRegistry, IActionResultRegistry actionResultRegistry)
            : this()
        {
            this.fluentMvcResolver = fluentMvcResolver;
            this.actionResultRegistry = actionResultRegistry;
            this.actionFilterRegistry = actionFilterRegistry;
        }

        public static FluentMvcConfiguration Create()
        {
            return Create(null, new ActionFilterRegistry(new FluentMvcObjectFactory()), new ActionResultRegistry());
        }

        public static FluentMvcConfiguration Create(IFluentMvcResolver fluentMvcResolver, IActionFilterRegistry actionFilterRegistry, IActionResultRegistry actionResultRegistry)
        {
            return new FluentMvcConfiguration(fluentMvcResolver, actionFilterRegistry, actionResultRegistry);
        }
        
        public virtual IControllerFactory BuildControllerFactory()
        {
            CreateDependencies();
            SetDefaultFactory();
            BuildActionResultFactoryPipeline();
            RegisterFilters();
            PrepareResolver();

            IControllerFactory controllerFactory = CreateControllerFactory();

            return controllerFactory;
        }

        private void PrepareResolver()
        {
            fluentMvcResolver.RegisterActionResultPipeline(pipeline);
            fluentMvcResolver.SetActionResultRegistry(actionResultRegistry);
            fluentMvcResolver.SetActionFilterRegistry(actionFilterRegistry);
        }

        private void CreateDependencies()
        {
            if ( actionFilterRegistry == null)
                actionFilterRegistry = new ActionFilterRegistry(objectFactory);

            if ( actionResultRegistry == null)
                actionResultRegistry = new ActionResultRegistry();

            if (fluentMvcResolver == null)
                fluentMvcResolver = new FluentMvcResolver(actionResultRegistry, actionFilterRegistry, objectFactory);
        }

        private void SetDefaultFactory()
        {
            fluentMvcResolver.SetDefaultFactory(Convention.DefaultFactory);
        }

        private void BuildActionResultFactoryPipeline()
        {
            if (Convention.Factories.Count == 0)
                return;

            pipeline.Register(Convention.Factories);
        }

        private void RegisterFilters()
        {
            foreach (var registration in filterConstaintRegistrations)
            {
                IEnumerable<ActionFilterRegistryItem> registryItem = CreateRegistryItem(registration);
                actionFilterRegistry.Add(registryItem);
            }
        }

        private IEnumerable<ActionFilterRegistryItem> CreateRegistryItem(KeyValuePair<Type, HashSet<TransientRegistration>> registration)
        {
            Type key = registration.Key;

            foreach (var reg in registration.Value)
            {
                yield return reg.CreateRegistryItem(key);
            }
        }

        protected IControllerFactory CreateControllerFactory()
        {
            return new FluentMvcControllerFactory(Convention.ControllerFactory, fluentMvcResolver);
        }

        public void ExposeConfiguration(Action<FluentMvcConfiguration> action)
        {
            action(this);
        }

        public static IControllerFactory ConfigureAndBuildControllerFactory()
        {
            var config = Create();
            Configure(config);

            return config.BuildControllerFactory();
        }
    }
}