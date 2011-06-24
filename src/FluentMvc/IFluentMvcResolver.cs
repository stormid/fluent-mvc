using System.Collections.Generic;
using System.Web.Mvc;

namespace FluentMvc
{
    public interface IFluentMvcResolver : IActionResultResolver, IActionFilterResolver
    {
        void BuildUpFilters(IEnumerable<Filter> attributeFilters);
    }
}