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
            fluentMvcResolver.AddFiltersTo(baseFilters, GetActonFilterSelector(controllerContext, actionDescriptor));

            return baseFilters;
        }

        private ActionFilterSelector GetActonFilterSelector(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return new ActionFilterSelector(controllerContext, actionDescriptor, actionDescriptor.ControllerDescriptor);
        }

        protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
        {
            if (!(actionReturnValue is ActionResult))
                controllerContext.Controller.ViewData.Model = actionReturnValue;

            return fluentMvcResolver.CreateActionResult(ActionResultSelector(actionReturnValue, controllerContext, actionDescriptor));
        }

        private ActionResultSelector ActionResultSelector(object actionReturnValue, ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return new ActionResultSelector(actionReturnValue, controllerContext, actionDescriptor, actionDescriptor.ControllerDescriptor);
        }

        public static IActionInvoker Create(IActionResultResolver actionResultFactory)
        {
            return new FluentMvcActionInvoker(actionResultFactory);
        }
    }
}