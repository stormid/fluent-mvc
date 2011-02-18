using System;

namespace FluentMvc
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public abstract class AbstractRegistry<TRegistryItem, TSelector> : IRegistry<TRegistryItem, TSelector>
        where TSelector : RegistrySelector
        where TRegistryItem : RegistryItem<TSelector>
    {
        private HashSet<TRegistryItem> registry = new HashSet<TRegistryItem>();
        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        public virtual TRegistryItem[] Registrations
        {
            get { return registry.ToArray(); }
        }

        public virtual void Add(IEnumerable<TRegistryItem> registryItems)
        {
            PerformActionInLock(() => registry = new HashSet<TRegistryItem>(registry.Concat(registryItems)));
        }

        public virtual void Add(TRegistryItem registryItem)
        {
            PerformActionInLock(() => registry.Add(registryItem));
        }

        private void PerformActionInLock(Action action)
        {
            rwLock.EnterWriteLock();
            try
            {
                action();
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
            IEnumerable<TRegistryItem> applicable = null;
            IEnumerable<TRegistryItem> toRemove = null;

            PerformActionInLock(() =>
                                    {
                                        IEnumerable<TRegistryItem> items = registry;

                                        applicable = FindApplicableItems(items, selector);
                                        toRemove = FindItemsToRemove(items, selector);
                                    });

            return applicable.Except(toRemove, new RegistryEqualityComparer<TRegistryItem, TSelector>()).Distinct().ToArray();
        }

        public TRegistryItem[] FindForSelectors(params TSelector[] selectors)
        {
            var list = new HashSet<TRegistryItem>(new RegistryEqualityComparer<TRegistryItem, TSelector>());

            foreach (var registryItem in selectors.SelectMany(FindForSelector))
            {
                list.Add(registryItem);   
            }

            return list.ToArray();
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

    public class RegistryEqualityComparer<T, T1> : IEqualityComparer<T> where T : RegistryItem<T1> where T1 : RegistrySelector
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