namespace FluentMvc
{
    using System.Web.Mvc;
    using ActionResultFactories;
    using Configuration;

    public interface IActionResultResolver
    {
        IActionResultPipeline ActionResultPipeLine { get; }
        IActionResultFactory DefaultFactory { get; }
        ActionResult CreateActionResult(ActionResultSelector selector);
        void SetDefaultFactory(IActionResultFactory factory);
        void SetActionResultRegistry(IActionResultRegistry registry);
        void RegisterActionResultPipeline(IActionResultPipeline pipeline);
    }
}