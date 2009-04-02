namespace FluentMvc.ActionResultFactories
{
    using System.Linq;
    using System.Web.Mvc;

    public class ViewResultFactory : AbstractActionResultFactory
    {
        protected override ActionResult CreateResult(ActionResultSelector selector)
        {
            return new ViewResult { ViewData = selector.ViewData };
        }

        protected override bool ShouldApplyFactory(ActionResultSelector selector)
        {
            return selector.AcceptTypes.Contains("text/html");
        }
    }
}