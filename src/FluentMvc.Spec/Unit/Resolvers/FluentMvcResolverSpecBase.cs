namespace FluentMvc.Spec.Unit.DefaultActionResultResolver
{
    using FluentMvc;

    public abstract class FluentMvcResolverSpecBase : SpecificationBase
    {
        protected IActionResultResolver actionResultResolver;
        protected IActionResultRegistry actionResultRegistry;
        protected IActionFilterRegistry actionFilterRegistry;

        public override void ForEach()
        {
            actionFilterRegistry = CreateStub<IActionFilterRegistry>();
            actionResultRegistry = CreateStub<IActionResultRegistry>();
        }
    }
}