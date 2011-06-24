using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FluentMvc
{
    public class FluentMvcFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IFluentMvcResolver fluentMvcResolver;

        public FluentMvcFilterProvider(IFluentMvcResolver fluentMvcResolver)
        {
            this.fluentMvcResolver = fluentMvcResolver;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var attributeFilters =  base.GetFilters(controllerContext, actionDescriptor);
            fluentMvcResolver.BuildUpFilters(attributeFilters);

            var filterInfo = new FilterInfo();
            GetFiltersFromRegistry(controllerContext, actionDescriptor, filterInfo);

            return attributeFilters.Concat(CreateFilters(filterInfo));
        }

        private void GetFiltersFromRegistry(ControllerContext controllerContext, ActionDescriptor actionDescriptor, FilterInfo filterInfo)
        {
            var actionFilterSelector = new ActionFilterSelector(controllerContext, actionDescriptor, actionDescriptor.ControllerDescriptor);
            fluentMvcResolver.AddFiltersTo(filterInfo, actionFilterSelector);
        }

        private IEnumerable<Filter> CreateFilters(FilterInfo filterInfo)
        {
            foreach (var filter in filterInfo.AuthorizationFilters)
            {
                yield return new Filter(filter, FilterScope.Global, null);
            }

            foreach (var filter in filterInfo.ExceptionFilters)
            {
                yield return new Filter(filter, FilterScope.Global, null);
            }

            foreach (var filter in filterInfo.ActionFilters)
            {
                yield return new Filter(filter, FilterScope.Global, null);
            }

            foreach (var filter in filterInfo.ResultFilters)
            {
                yield return new Filter(filter, FilterScope.Global, null);
            }
        }
    }
}