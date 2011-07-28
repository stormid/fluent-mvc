using System;

namespace FluentMvc
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Configuration;

    public class ActionFilterRegistry : AbstractRegistry<ActionFilterRegistryItem, ActionFilterSelector>, IActionFilterRegistry
    {
        private IFluentMvcObjectFactory objectFactory;

        public ActionFilterRegistry(IFluentMvcObjectFactory objectFactory)
        {
            SetObjectFactory(objectFactory);
        }

        public void AddFiltersTo(FilterInfo baseFilters, ActionFilterSelector selector)
        {
            var applicableFilters = FindForSelector(selector);
            FilterAndAddFilters(baseFilters.ActionFilters, applicableFilters);
            FilterAndAddFilters(baseFilters.AuthorizationFilters, applicableFilters);
            FilterAndAddFilters(baseFilters.ExceptionFilters, applicableFilters);
            FilterAndAddFilters(baseFilters.ResultFilters, applicableFilters);
        }

        public void SetObjectFactory(IFluentMvcObjectFactory factory)
        {
            objectFactory = factory;
        }

        private void FilterAndAddFilters<T>(IList<T> baseFiltersList, IEnumerable<ActionFilterRegistryItem> applicableFilters)
        {
            IList<T> filtersToAdd = new List<T>();
            foreach (var item in FilterActionFilters<T>(applicableFilters))
            {
                var filter = CreateFilter<T>(item);
                BuildUp(filter);
                filtersToAdd.Add(filter);
            }

            foreach (var filter in filtersToAdd.Reverse())
            {
                baseFiltersList.Insert(0, filter);
            }
        }

        private void BuildUp<T>(T filter)
        {
            objectFactory.BuildUpProperties(filter);
        }

        private void AddFilter<T>(ICollection<T> baseFiltersList, T filter)
        {
            baseFiltersList.Add(filter);
        }

        private T CreateFilter<T>(ActionFilterRegistryItem item)
        {
            return item.Create<T>(objectFactory);
        }

        private IEnumerable<ActionFilterRegistryItem> FilterActionFilters<T>(IEnumerable<ActionFilterRegistryItem> applicableFilters)
        {
            return applicableFilters.Where(i => (typeof(T)).IsAssignableFrom(i.Type));
        }
    }
}