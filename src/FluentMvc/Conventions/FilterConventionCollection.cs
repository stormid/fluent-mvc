using System;
using System.Collections.Generic;
using System.Reflection;
using FluentMvc.Configuration;

namespace FluentMvc.Conventions
{
    public class FilterConventionCollection : IFilterConventionCollection
    {
        private readonly IList<IFilterConventionSearcher> conventionFinders = new List<IFilterConventionSearcher>();

        public IFilterConventionCollection LoadFromAssemblyContaining<T>()
        {
            conventionFinders.Add(new AssemblySearcher(typeof (T).Assembly));
            return this;
        }

        public void ApplyConventions(IFilterRegistration filterRegistration)
        {
            var convetions = FindConventions();

            foreach (var filterConvention in convetions)
            {
                filterConvention.ApplyConvention(filterRegistration);
            }
        }

        private IEnumerable<IFilterConvention> FindConventions()
        {
            var conventions = new List<IFilterConvention>();
            foreach (var filterConventionSearcher in conventionFinders)
            {
                conventions.AddRange(filterConventionSearcher.Find());
            }

            return conventions;
        }
    }

    public class AssemblySearcher : IFilterConventionSearcher
    {
        private readonly Assembly assembly;

        public AssemblySearcher(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public IEnumerable<IFilterConvention> Find()
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (IsAFilterConvention(type))
                    yield return Activator.CreateInstance(type) as IFilterConvention; // TODO: Replace this simple implemntation
            }
        }

        private bool IsAFilterConvention(Type type)
        {
            return typeof (IFilterConvention).IsAssignableFrom(type);
        }
    }

    public interface IFilterConventionSearcher
    {
        IEnumerable<IFilterConvention> Find();
    }
}