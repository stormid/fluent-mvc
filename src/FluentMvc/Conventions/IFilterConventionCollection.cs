using FluentMvc.Configuration;

namespace FluentMvc.Conventions
{
    public interface IFilterConventionCollection
    {
        IFilterConventionCollection LoadFromAssemblyContaining<T>();
        void ApplyConventions<TDsl>(IFilterRegistration<TDsl> registration);
    }
}