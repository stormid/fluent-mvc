namespace FluentMvc.Spec.Unit.ActionResultRegistrySpecs
{
    using ActionResultRegistry=FluentMvc.ActionResultRegistry;

    public abstract class ActionResultRegistrySpecBase : SpecificationBase
    {
        protected ActionResultRegistryTester registryTester;
        protected ActionResultRegistry registry;
    }
}