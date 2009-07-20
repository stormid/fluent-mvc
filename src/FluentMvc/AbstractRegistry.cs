namespace FluentMvc
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public abstract class AbstractRegistry<TRegistryItem, TSelector> : IRegistry<TRegistryItem, TSelector>
        where TSelector : RegistrySelector
        where TRegistryItem : RegistryItem
    {
        private HashSet<TRegistryItem> registry = new HashSet<TRegistryItem>();
        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        public virtual TRegistryItem[] Registrations
        {
            get { return registry.ToArray(); }
        }

        public virtual void Add(IEnumerable<TRegistryItem> registryItems)
        {
            rwLock.EnterWriteLock();
            try
            {
                registry = new HashSet<TRegistryItem>(registry.Concat(registryItems));
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public virtual void Add(TRegistryItem registryItem)
        {
            rwLock.EnterWriteLock();
            try
            {
                registry.Add(registryItem);
            }
            finally
            {
                rwLock.ExitWriteLock();
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
            rwLock.EnterReadLock();
            try
            {
                IEnumerable<TRegistryItem> items = registry;

                IEnumerable<TRegistryItem> applicable = FindApplicableItems(items, selector);
                IEnumerable<TRegistryItem> toRemove = FindItemsToRemove(items, selector);

                return applicable.Except(toRemove, new RegistryEqualityComparer<TRegistryItem>()).Distinct().ToArray();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
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