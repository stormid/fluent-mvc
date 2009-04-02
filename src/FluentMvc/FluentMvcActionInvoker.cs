namespace FluentMvc
{
    using System.Web.Mvc;

    public class FluentMvcActionInvoker : ControllerActionInvoker
    {
        private readonly IActionResultResolver fluentMvcResolver;

        private FluentMvcActionInvoker(IActionResultResolver fluentMvcResolver)
        {
            this.fluentMvcResolver = fluentMvcResolver;
        }

        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            FilterInfo baseFilters = base.GetFilters(controllerContext, actionDescriptor);
            fluentMvcResolver.AddFiltersTo(baseFilters, new ActionFilterSelector(controllerContext, actionDescriptor, actionDescriptor.ControllerDescriptor));

            return baseFilters;
        }

        protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
        {
            if (!(actionReturnValue is ActionResult))
                controllerContext.Controller.ViewData.Model = actionReturnValue;

            return fluentMvcResolver.CreateActionResult(new ActionResultSelector(actionReturnValue, controllerContext, actionDescriptor, actionDescriptor.ControllerDescriptor));
        }

        public static IActionInvoker Create(IActionResultResolver actionResultFactory)
        {
            return new FluentMvcActionInvoker(actionResultFactory);
        }
    }
}