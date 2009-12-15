using System.Collections.Generic;
using FluentMvc.Configuration.Registrations;

namespace FluentMvc.Configuration
{
    public interface IFilterRegistration<TDsl>
    {
        TDsl WithFilter<T>(ConstraintDsl constraint);
        TDsl WithFilter<TFilter>();
        TDsl WithFilter<T>(T filterInstance);
        TDsl WithFilter<T>(T filterInstance, ConstraintDsl constraint);
        TDsl WithFilter<TFilter>(IEnumerable<TransientRegistration> registrations);
    }
}