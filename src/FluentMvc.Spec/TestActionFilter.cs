namespace FluentMvc.Spec
{
    using System.Web.Mvc;

    internal class TestActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            throw new System.NotImplementedException();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new System.NotImplementedException();
        }
    }
}