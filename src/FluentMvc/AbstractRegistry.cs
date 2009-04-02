namespace FluentMvc
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class AbstractRegistry<TRegistryItem, TSelector> : IRegistry<TRegistryItem, TSelector>
        where TSelector : RegistrySelector
        where TRegistryItem : RegistryItem
    {
        private HashSet<TRegistryItem> registry = new HashSet<TRegistryItem>();
        private readonly object monitor = new object();

        public virtual TRegistryItem[] Registrations
        {
            get { return registry.ToArray(); }
        }

        public virtual void Add(IEnumerable<TRegistryItem> registryItems)
        {
            lock (monitor)
            {
                registry = new HashSet<TRegistryItem>(registry.Concat(registryItems));                
            }
        }

        public virtual TRegistryItem Create(TSelector selector)
        {
            TRegistryItem first = FindForSelector(selector).FirstOrDefault();
            return first;
        }

        public virtual bool CanSatisfy(TSelector selector)
        {
            return FindForSelector(selector).Length > 0;
        }

        public virtual TRegistryItem[] FindForSelector(TSelector selector)
        {
            IEnumerable<TRegistryItem> items = registry;

            IEnumerable<TRegistryItem> applicableFilters = FindApplicableFilters(items, selector);
            IEnumerable<TRegistryItem> filtersToRemove = FindFiltersToRemove(items, selector);

            return applicableFilters.Except(filtersToRemove, new RegistryEqualityComparer<TRegistryItem>()).Distinct().ToArray();
        }

        private IEnumerable<TRegistryItem> FindFiltersToRemove(IEnumerable<TRegistryItem> items, TSelector selector)
        {
            return items.Where(item => (item.AppliesToController(selector) && item.AppliesToAction(selector)) && !item.Satisfies(selector));
        }

        private IEnumerable<TRegistryItem> FindApplicableFilters(IEnumerable<TRegistryItem> items, TSelector selector)
        {
            return items.Where(item => item.AppliesToController(selector) && item.AppliesToAction(selector) && item.Satisfies(selector));
        }

        public virtual void Add(TRegistryItem registryItem)
        {
            lock (monitor)
            {
                registry.Add(registryItem);
            }
        }
    }

    public class RegistryEqualityComparer<T> : IEqualityComparer<T> where T : RegistryItem
    {
        public bool Equals(T x, T y)
        {
            return x.Type.Equals(y.Type);
        }

        public int GetHashCode(T obj)
        {
            return obj.Type.GetHashCode();
        }
    }
}