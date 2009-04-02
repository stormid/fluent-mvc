namespace FluentMvc.Configuration
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ActionResultFactories;

    public interface IActionResultPipeline
    {
        void Register(Queue<IActionResultFactory> actionResultFactory);
        ActionResult Create(ActionResultSelector selector);
    }
}