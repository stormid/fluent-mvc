using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentMvc.Conventions
{
    public class AssemblySearcher : IFilterConventionSearcher
    {
        private readonly Assembly assembly;

        public AssemblySearcher(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public IEnumerable<Type> Find()
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (IsAFilterConvention(type))
                    yield return type;
            }
        }

        private bool IsAFilterConvention(Type type)
        {
            return typeof (IFilterConvention).IsAssignableFrom(type);
        }
    }
}