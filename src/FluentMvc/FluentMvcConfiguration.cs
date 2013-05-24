using System.Linq;
using FluentMvc.Conventions;

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

        public IFilterConventionCollection FilterConventions { get; protected set; }

        public static Action<FluentMvcConfiguration> Configure { get; set; }

        private FluentMvcConfiguration()
        {
            pipeline = new ActionResultPipeline();
            Convention = new FluentMvcConvention();
            objectFactory = new FluentMvcObjectFactory();
        }

        private FluentMvcConfiguration(IFluentMvcResolver fluentMvcResolver, IActionFilterRegistry actionFilterRegistry, IActionResultRegistry actionResultRegistry, IFilterConventionCollection filterConventionCollection)
            : this()
        {
            this.fluentMvcResolver = fluentMvcResolver;
            this.actionResultRegistry = actionResultRegistry;
            this.actionFilterRegistry = actionFilterRegistry;
            FilterConventions = filterConventionCollection;
        }

        public static FluentMvcConfiguration Create()
        {
            return Create(new NullMvcResolver(), new ActionFilterRegistry(new FluentMvcObjectFactory()), new ActionResultRegistry(), new FilterConventionCollection(FilterConventionActivator.Default));
        }

        public static FluentMvcConfiguration Create(IFluentMvcResolver fluentMvcResolver, IActionFilterRegistry actionFilterRegistry, IActionResultRegistry actionResultRegistry, IFilterConventionCollection filterConventionCollection)
        {
            return new FluentMvcConfiguration(fluentMvcResolver, actionFilterRegistry, actionResultRegistry, filterConventionCollection);
        }

        private void RunFilterConventions()
        {
            FilterConventions.ApplyConventions(new MvcFilterConfigurationAdapter(this));
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

            if (fluentMvcResolver.GetType() == typeof(NullMvcResolver))
                fluentMvcResolver = new FluentMvcResolver(actionResultRegistry, objectFactory, new ActionFilterResolver(actionFilterRegistry, objectFactory));
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

            return registration.Value.Select(reg => reg.CreateRegistryItem(key));
        }

        public void ExposeConfiguration(Action<FluentMvcConfiguration> action)
        {
            action(this);
        }

        [Obsolete]
        public virtual IControllerFactory BuildControllerFactory()
        {
            throw new InvalidOperationException();
        }

        private IFilterProvider constructFilterProvider()
        {
            CreateDependencies();
            SetDefaultFactory();
            BuildActionResultFactoryPipeline();
            RunFilterConventions();
            RegisterFilters();
            PrepareResolver();

            return new FluentMvcFilterProvider(fluentMvcResolver);
        }

        public IFilterProvider BuildFilterProvider()
        {
            return constructFilterProvider();
        }

        public static IFilterProvider ConfigureAndBuildFilterProvider()
        {
            var config = Create();
            Configure(config);

            return config.BuildFilterProvider();
        }

        public static IFilterProvider ConfigureFilterProvider(Action<FluentMvcConfiguration> configureAction)
        {
            Configure = configureAction;

            return ConfigureAndBuildFilterProvider();
        }
    }

    internal class MvcFilterConfigurationAdapter : IFilterRegistration
    {
        private readonly FluentMvcConfiguration fluentMvcConfiguration;

        public MvcFilterConfigurationAdapter(FluentMvcConfiguration fluentMvcConfiguration)
        {
            this.fluentMvcConfiguration = fluentMvcConfiguration;
        }

        public void WithFilter<TFilter>(ConstraintDsl constraint)
        {
            fluentMvcConfiguration.WithFilter<TFilter>(constraint);
        }

        public void WithFilter<TFilter>()
        {
            fluentMvcConfiguration.WithFilter<TFilter>();
        }

        public void WithFilter<TFilter>(TFilter filterInstance)
        {
            fluentMvcConfiguration.WithFilter(filterInstance);
        }

        public void WithFilter<TFilter>(TFilter filterInstance, ConstraintDsl constraint)
        {
            fluentMvcConfiguration.WithFilter(filterInstance, constraint);
        }

        public void WithFilter<TFilter>(IEnumerable<TransientRegistration> registrations)
        {
            fluentMvcConfiguration.WithFilter<TFilter>(registrations);
        }
    }
}