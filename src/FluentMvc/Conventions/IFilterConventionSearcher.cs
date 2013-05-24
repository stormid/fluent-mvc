using System;
using System.Collections.Generic;

namespace FluentMvc.Conventions
{
    public interface IFilterConventionSearcher
    {
        IEnumerable<Type> Find();
    }
}