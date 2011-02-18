using System.Collections.Generic;
using System.Linq;

namespace FluentMvc
{
    using System.Web.Mvc;
    using ActionResultFactories;
    using Configuration;

    public class FluentMvcResolver : IFluentMvcResolver
    {
        private readonly IFluentMvcObjectFactory objectFactory;
        private readonly IActionResultResolver actionResultResolver;
        private readonly IActionFilterResolver actionFilterResolver;

        public FluentMvcResolver(IActionResultRegistry actionResultRegistry, IFluentMvcObjectFactory objectFactory, IActionFilterResolver filterResolver)
        {
            this.objectFactory = objectFactory;
            actionFilterResolver = filterResolver;
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

        public void BuildUpFilters(IEnumerable<Filter> attributeFilters)
        {
            foreach (var attributeFilter in attributeFilters)
            {
                objectFactory.BuildUpProperties(attributeFilter);
            }
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            return actionFilterResolver.GetFilters(controllerContext, actionDescriptor, controllerDescriptor);
        }

        #endregion
    }

    public class ActionFilterResolver : IActionFilterResolver
    {
        private IActionFilterRegistry actionFilterRegistry;
        private readonly IFluentMvcObjectFactory fac;

        public ActionFilterResolver(IActionFilterRegistry actionFilterRegistry, IFluentMvcObjectFactory fac)
        {
            this.actionFilterRegistry = actionFilterRegistry;
            this.fac = fac;
        }

        public void SetActionFilterRegistry(IActionFilterRegistry registry)
        {
            actionFilterRegistry = registry;
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            var globalActionFilterSelector = new GlobalActionFilterSelector(controllerContext, actionDescriptor, controllerDescriptor);
            var controllerFilterSelector = new ControllerFilterSelector(controllerContext, controllerDescriptor, actionDescriptor);
            var controllerActionFilterSelector = new ControllerActionFilterSelector(controllerContext, actionDescriptor, controllerDescriptor);

            var actionFilterRegistryItems = actionFilterRegistry.FindForSelectors(globalActionFilterSelector, controllerFilterSelector, controllerActionFilterSelector);

            return GetFilters(actionFilterRegistryItems);
        }

        private IEnumerable<Filter> GetFilters(IEnumerable<ActionFilterRegistryItem> list)
        {
            return list.Select(x => new Filter(x.Create(fac), x.Scope, null));
        }
    }

    public class ControllerActionFilterSelector : ActionFilterSelector
    {
        public ControllerActionFilterSelector(ControllerContext context, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(context, actionDescriptor, controllerDescriptor)
        {
            Scope = FilterScope.Action;
        }
    }

    public class ControllerFilterSelector : ActionFilterSelector
    {
        public ControllerFilterSelector(ControllerContext context, ControllerDescriptor controllerDescriptor, ActionDescriptor actionDescriptor)
            : base(context, actionDescriptor, controllerDescriptor)
        {
            Scope = FilterScope.Controller;
        }
    }

    public class GlobalActionFilterSelector : ActionFilterSelector
    {
        public GlobalActionFilterSelector(ControllerContext context, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(context, actionDescriptor, controllerDescriptor)
        {
            Scope = FilterScope.Global;
        }
    }
}