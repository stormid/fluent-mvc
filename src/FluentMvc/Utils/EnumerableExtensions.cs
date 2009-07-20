namespace FluentMvc.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static bool HasItems<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Count() > 0;
        }
    }
}