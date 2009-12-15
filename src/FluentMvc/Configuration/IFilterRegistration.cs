using System.Collections.Generic;
using FluentMvc.Configuration.Registrations;

namespace FluentMvc.Configuration
{
    public interface IFilterRegistration
    {
        void WithFilter<TFilter>(ConstraintDsl constraint);
        void WithFilter<TFilter>();
        void WithFilter<TFilter>(TFilter filterInstance);
        void WithFilter<TFilter>(TFilter filterInstance, ConstraintDsl constraint);
        void WithFilter<TFilter>(IEnumerable<TransientRegistration> registrations);
    }
}