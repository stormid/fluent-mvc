namespace FluentMvc.Utils
{
    using System.Collections;

    public static class EnumerableExtensions
    {
        public static bool HasItems(this IEnumerable enumerable)
        {
            return enumerable != null && ((ICollection) enumerable).Count > 0;
        }
    }
}