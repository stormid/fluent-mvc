namespace FluentMvc.ActionResultFactories
{
    using System.Web.Mvc;
    using Constraints;

    public class JsonResultFactory : AbstractActionResultFactory
    {
        public JsonResultFactory()
        {
            SetConstraint(new[] { new ExpectsJson() });
        }

        protected override ActionResult CreateCore(ActionResultSelector selector)
        {
            return new JsonResult {Data = selector.ReturnValue};
        }
    }
}