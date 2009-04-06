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

        public virtual void Add(TRegistryItem registryItem)
        {
            lock (monitor)
            {
                registry.Add(registryItem);
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

            IEnumerable<TRegistryItem> applicable = FindApplicableItems(items, selector);
            IEnumerable<TRegistryItem> toRemove = FindItemsToRemove(items, selector);

            return applicable.Except(toRemove, new RegistryEqualityComparer<TRegistryItem>()).Distinct().ToArray();
        }

        private IEnumerable<TRegistryItem> FindItemsToRemove(IEnumerable<TRegistryItem> items, TSelector selector)
        {
            return items.Where(item => (AppliesToCurrentControllerAndAction(item, selector)) && !item.Satisfies(selector));
        }

        private bool AppliesToCurrentControllerAndAction(TRegistryItem item, TSelector selector)
        {
            return item.AppliesToController(selector) && item.AppliesToAction(selector);
        }

        private IEnumerable<TRegistryItem> FindApplicableItems(IEnumerable<TRegistryItem> items, TSelector selector)
        {
            return items.Where(item => AppliesToCurrentControllerAndAction(item, selector) && item.Satisfies(selector));
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