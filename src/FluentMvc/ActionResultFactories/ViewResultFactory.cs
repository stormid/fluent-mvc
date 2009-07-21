namespace FluentMvc.ActionResultFactories
{
    using System.Web.Mvc;
    using Constraints;

    public class ViewResultFactory : AbstractActionResultFactory
    {
        public ViewResultFactory()
        {
            SetConstraint(new[] {new ExpectsHtml()});
        }

        protected override ActionResult CreateCore(ActionResultSelector selector)
        {
            return new ViewResult { ViewData = selector.ViewData };
        }
    }
}