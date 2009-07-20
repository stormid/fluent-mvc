namespace FluentMvc.Utils
{
    using System;
    using System.Collections;

    public static class EnumerableExtensions
    {
        public static bool HasItems(this IEnumerable enumerable)
        {
            return enumerable != null && ((Array) enumerable).Length > 0;
        }
    }
}