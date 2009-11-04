using FluentMvc.Configuration;

namespace FluentMvc.Conventions
{
    public interface IFilterConvention
    {
        void Apply<TDsl>(IFilterRegistration<TDsl> filterRegistration);
    }
}