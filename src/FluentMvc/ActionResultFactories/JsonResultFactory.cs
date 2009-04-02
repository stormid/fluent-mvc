namespace FluentMvc.ActionResultFactories
{
    using System.Linq;
    using System.Web.Mvc;

    public class JsonResultFactory : AbstractActionResultFactory
    {
        protected override ActionResult CreateResult(ActionResultSelector selector)
        {
            return new JsonResult {Data = selector.ReturnValue};
        }

        protected override bool ShouldApplyFactory(ActionResultSelector selector)
        {
            return selector.AcceptTypes.Contains("application/json");
        }
    }
}