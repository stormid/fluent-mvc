using FluentMvc.Configuration;

namespace FluentMvc.Conventions
{
    public interface IFilterConvention
    {
        void ApplyConvention<TDsl>(IFilterRegistration<TDsl> filterRegistration);
    }
}