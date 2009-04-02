namespace FluentMvc.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ActionResultFactories;

    public class ActionResultPipeline : IActionResultPipeline
    {
        private Queue<IActionResultFactory> pipeline = new Queue<IActionResultFactory>();

        public void Register(Queue<IActionResultFactory> actionResultFactory)
        {
            pipeline = actionResultFactory;
        }

        public ActionResult Create(ActionResultSelector selector)
        {
            var factory = pipeline.FirstOrDefault(x => x.ShouldBeReturnedFor(selector));

            return factory != null ? factory.Create(selector) : null;
        }
    }
}