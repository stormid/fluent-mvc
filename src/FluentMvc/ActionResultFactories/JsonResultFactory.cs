namespace FluentMvc.ActionResultFactories
{
    using System.Web.Mvc;
    using Constraints;

    /// <summary>
    /// This ResultFactory will create a JsonResult with the Data set as the return value of the action.  Use this if you are not returning
    /// an ActionResult from your controller action.  If your controller actions return ActionResults, please us <see cref="JsonViewDataResultFactory"/>
    /// </summary>
    public class JsonResultFactory : AbstractActionResultFactory
    {
        public JsonResultFactory()
        {
            SetConstraints(new[] { new ExpectsJson() });
        }

        protected override ActionResult CreateCore(ActionResultSelector selector)
        {
            return new JsonResult {Data = selector.ReturnValue};
        }
    }

    /// <summary>
    /// This ResultFactory will create a JsonResult with the Data set as the ViewData.Model.  Use this if you are using standard actions that return
    /// ActionResults
    /// </summary>
    public class JsonViewDataResultFactory : JsonResultFactory
    {
        protected override ActionResult CreateCore(ActionResultSelector selector)
        {
            return new JsonResult { Data = selector.ViewData.Model };
        }
    }
}