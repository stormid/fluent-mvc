using System.Collections.Generic;

namespace FluentMvc
{
    using System.Web.Mvc;

    public interface IActionFilterResolver
    {
        void SetActionFilterRegistry(IActionFilterRegistry registry);
        void AddFiltersTo(FilterInfo filters, ActionFilterSelector actionFilterSelector);
    }
}