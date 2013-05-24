using System.Collections.Generic;
using System.Web.Mvc;
using FluentMvc.ActionResultFactories;
using FluentMvc.Configuration;

namespace FluentMvc
{
    public interface IFluentMvcResolver : IActionResultResolver, IActionFilterResolver
    {
        void BuildUpFilters(IEnumerable<Filter> attributeFilters);
    }

    public class NullMvcResolver : IFluentMvcResolver
    {
        public IActionResultPipeline ActionResultPipeLine { get; private set; }
        public IActionResultFactory DefaultFactory { get; private set; }
        public ActionResult CreateActionResult(ActionResultSelector selector)
        {
            throw new System.NotImplementedException();
        }

        public void SetDefaultFactory(IActionResultFactory factory)
        {
            throw new System.NotImplementedException();
        }

        public void SetActionResultRegistry(IActionResultRegistry registry)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterActionResultPipeline(IActionResultPipeline pipeline)
        {
            throw new System.NotImplementedException();
        }

        public void SetActionFilterRegistry(IActionFilterRegistry registry)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor,
                                      ControllerDescriptor controllerDescriptor)
        {
            throw new System.NotImplementedException();
        }

        public void BuildUpFilters(IEnumerable<Filter> attributeFilters)
        {
            throw new System.NotImplementedException();
        }
    }
}