namespace FluentMvc
{
    using System.Web.Mvc;
    using Configuration;

    public interface IActionFilterRegistry : IRegistry<ActionFilterRegistryItem, ActionFilterSelector>
    {
        void AddFiltersTo(FilterInfo baseFilterInfo, ActionFilterSelector selector);
        void SetObjectFactory(IFluentMvcObjectFactory objectFactory);
    }
}