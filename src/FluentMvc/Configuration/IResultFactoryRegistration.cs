using FluentMvc.ActionResultFactories;

namespace FluentMvc.Configuration
{
    public interface IResultFactoryRegistration<TDsl>
    {
        TDsl WithResultFactory<TFactory>()
            where TFactory : IActionResultFactory;

        TDsl WithResultFactory(IActionResultFactory factory, ConstraintDsl constraintDsl);

        TDsl WithResultFactory<TFactory>(ConstraintDsl constraintDsl)
            where TFactory : IActionResultFactory;

        TDsl WithResultFactory(IActionResultFactory resultFactory);
        TDsl WithResultFactory(IActionResultFactory defaultFactory, bool isDefault);
        TDsl WithResultFactory<TResultFactory>(bool isDefault) where TResultFactory : IActionResultFactory;
    }
}