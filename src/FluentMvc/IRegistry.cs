namespace FluentMvc
{
    using System.Collections.Generic;

    public interface IRegistry<TRegistryItem, TSelector>
        where TSelector : RegistrySelector
        where TRegistryItem : RegistryItem
    {
        TRegistryItem[] Registrations { get; }
        void Add(IEnumerable<TRegistryItem> registryItems);
        void Add(TRegistryItem registryItem);
        TRegistryItem Create(TSelector selector);
        bool CanSatisfy(TSelector selector);
        TRegistryItem[] FindForSelector(TSelector selector);
    }
}