using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentMvc.Configuration;

namespace FluentMvc.Conventions
{
    public class FilterConventionCollection : IFilterConventionCollection
    {
        private readonly IList<IFilterConventionSearcher> conventionFinders = new List<IFilterConventionSearcher>();
        private IFilterConventionActivator filterConventionActivator;

        public FilterConventionCollection(IFilterConventionActivator filterConventionActivator)
        {
            this.filterConventionActivator = filterConventionActivator;
        }

        public IFilterConventionCollection LoadFromAssemblyContaining<T>()
        {
            return LoadFromAssembly(typeof (T).Assembly);
        }

        public IFilterConventionCollection LoadFromAssembly(Assembly assembly)
        {
            conventionFinders.Add(new AssemblySearcher(assembly));
            return this;
        }

        public void ApplyConventions(IFilterRegistration filterRegistration)
        {
            var convetions = FindConventions();

            foreach (var filterConvention in convetions.Select(type => ActivateConvention(type)))
            {
                filterConvention.ApplyConvention(filterRegistration);
            }
        }

        public void SetConventionActivator(IFilterConventionActivator activator)
        {
            filterConventionActivator = activator;
        }

        private IFilterConvention ActivateConvention(Type type)
        {
            return filterConventionActivator.Activate(type);
        }

        private IEnumerable<Type> FindConventions()
        {
            var conventions = new List<Type>();
            foreach (var filterConventionSearcher in conventionFinders)
            {
                conventions.AddRange(filterConventionSearcher.Find());
            }

            return conventions;
        }
    }
}