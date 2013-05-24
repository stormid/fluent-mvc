using System.Reflection;
using FluentMvc.Configuration;

namespace FluentMvc.Conventions
{
    public interface IFilterConventionCollection
    {
        IFilterConventionCollection LoadFromAssemblyContaining<T>();
        IFilterConventionCollection LoadFromAssembly(Assembly assembly);
        void ApplyConventions(IFilterRegistration registration);
        void SetConventionActivator(IFilterConventionActivator filterConventionActivator);
    }
}