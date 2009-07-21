namespace FluentMvc
{
    using System.Web.Mvc;
    using ActionResultFactories;
    using Configuration;

    public class ActionResultResolver : IActionResultResolver
    {
        private IActionResultFactory defaultFactory;
        private IActionResultPipeline actionResultPipeLine;
        private IActionResultRegistry actionResultRegistry;
        private readonly IFluentMvcObjectFactory objectFactory;

        public IActionResultFactory DefaultFactory
        {
            get { return defaultFactory ?? EmptyActionResultFactory.Instance; }
        }

        public IActionResultPipeline ActionResultPipeLine
        {
            get { return actionResultPipeLine ?? new ActionResultPipeline(); }
            set { actionResultPipeLine = value; }
        }

        public ActionResultResolver(IActionResultRegistry actionResultRegistry, IFluentMvcObjectFactory objectFactory)
        {
            this.actionResultRegistry = actionResultRegistry;
            this.objectFactory = objectFactory;
        }

        public void SetDefaultFactory(IActionResultFactory factory)
        {
            defaultFactory = factory;
        }

        public ActionResult CreateActionResult(ActionResultSelector selector)
        {
            if (actionResultRegistry.CanSatisfy(selector))
            {
                return CreateActionResultFromRegistry(selector);
            }

            return ActionResultPipeLine.Create(selector) ?? DefaultFactory.Create(selector);
        }

        private ActionResult CreateActionResultFromRegistry(ActionResultSelector selector)
        {
            var registryItem = actionResultRegistry.Create(selector);
            var resultFactory = registryItem.Create<IActionResultFactory>(objectFactory);

            return resultFactory.Create(selector);
        }

        public void SetActionResultRegistry(IActionResultRegistry registry)
        {
            actionResultRegistry = registry;
        }

        public void RegisterActionResultPipeline(IActionResultPipeline pipeline)
        {
            ActionResultPipeLine = pipeline;
        }
    }
}