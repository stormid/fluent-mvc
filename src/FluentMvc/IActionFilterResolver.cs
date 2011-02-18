using System.Collections.Generic;

namespace FluentMvc
{
    using System.Web.Mvc;

    public interface IActionFilterResolver
    {
        void SetActionFilterRegistry(IActionFilterRegistry registry);
        IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor);
    }
}