namespace FluentMvc.ActionResultFactories
{
    using System.Web.Mvc;

    /// <summary>
    /// This Factory is designed to short circuit the pipeline to support standard controller
    /// actions that return ActionResults.  I didn't want to hard-code support, so I pulled it out
    /// as another IActionResultFactory
    /// </summary>
    public class ActionResultFactory : AbstractActionResultFactory
    {
        protected override bool ShouldApplyFactory(ActionResultSelector selector)
        {
            return selector.ReturnValue != null && typeof(ActionResult).IsAssignableFrom(selector.ReturnValue.GetType());
        }

        protected override ActionResult CreateResult(ActionResultSelector selector)
        {
            return selector.ReturnValue as ActionResult;
        }
    }
}