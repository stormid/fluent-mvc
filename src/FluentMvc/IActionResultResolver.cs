namespace FluentMvc
{
    using System.Web.Mvc;
    using ActionResultFactories;
    using Configuration;

    public interface IActionResultResolver
    {
        IActionResultPipeline ActionResultPipeLine { get; }
        void SetDefaultFactory(IActionResultFactory factory);
        ActionResult CreateActionResult(ActionResultSelector selector);
        void SetActionResultRegistry(IActionResultRegistry registry);
        void SetActionFilterRegistry(IActionFilterRegistry registry);
        void AddFiltersTo(FilterInfo filters, ActionFilterSelector actionFilterSelector);
        void RegisterActionResultPipeline(IActionResultPipeline pipeline);
    }
}