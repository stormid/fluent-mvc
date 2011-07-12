using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FluentMvc
{
    public class FluentMvcFilterProvider : IFilterProvider
    {
        private readonly IFluentMvcResolver fluentMvcResolver;

        public FluentMvcFilterProvider(IFluentMvcResolver fluentMvcResolver)
        {
            this.fluentMvcResolver = fluentMvcResolver;
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            //var filterInfo = new FilterInfo();
            //GetFiltersFromRegistry(controllerContext, actionDescriptor, filterInfo);

            //var actionFilterSelector = new ActionFilterSelector(controllerContext, actionDescriptor, actionDescriptor.ControllerDescriptor);

            return fluentMvcResolver.GetFilters(controllerContext, actionDescriptor, actionDescriptor.ControllerDescriptor);
        }
    }
}