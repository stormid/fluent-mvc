namespace FluentMvc
{
    using System.Web.Mvc;
    using ActionResultFactories;
    using Configuration;

    public class FluentMvcResolver : IFluentMvcResolver
    {
        private readonly IActionResultResolver actionResultResolver;
        private readonly IActionFilterResolver actionFilterResolver;

        public FluentMvcResolver(IActionResultRegistry actionResultRegistry, IActionFilterRegistry actionFilterRegistry, IFluentMvcObjectFactory objectFactory)
        {
            actionFilterResolver = new ActionFilterResolver(actionFilterRegistry);
            actionResultResolver = new ActionResultResolver(actionResultRegistry, objectFactory);
        }

        #region IActionResultResolver

        public IActionResultFactory DefaultFactory
        {
            get { return actionResultResolver.DefaultFactory; }
        }

        public IActionResultPipeline ActionResultPipeLine
        {
            get { return actionResultResolver.ActionResultPipeLine; }
        }


        public void SetDefaultFactory(IActionResultFactory factory)
        {
            actionResultResolver.SetDefaultFactory(factory);
        }

        public ActionResult CreateActionResult(ActionResultSelector selector)
        {
            return actionResultResolver.CreateActionResult(selector);
        }

        public void SetActionResultRegistry(IActionResultRegistry registry)
        {
            actionResultResolver.SetActionResultRegistry(registry);
        }

        public void RegisterActionResultPipeline(IActionResultPipeline pipeline)
        {
            actionResultResolver.RegisterActionResultPipeline(pipeline);
        }
        #endregion

        #region IActionFilterResolver
        public void SetActionFilterRegistry(IActionFilterRegistry registry)
        {
            actionFilterResolver.SetActionFilterRegistry(registry);
        }

        public void AddFiltersTo(FilterInfo baseFilterInfo, ActionFilterSelector selector)
        {
            actionFilterResolver.AddFiltersTo(baseFilterInfo, selector);
        }
        #endregion
    }

    public class ActionFilterResolver : IActionFilterResolver
    {
        private IActionFilterRegistry actionFilterRegistry;

        public ActionFilterResolver(IActionFilterRegistry actionFilterRegistry)
        {
            this.actionFilterRegistry = actionFilterRegistry;
        }

        public void SetActionFilterRegistry(IActionFilterRegistry registry)
        {
            actionFilterRegistry = registry;
        }

        public void AddFiltersTo(FilterInfo filters, ActionFilterSelector actionFilterSelector)
        {
            actionFilterRegistry.AddFiltersTo(filters, actionFilterSelector);
        }
    }
}