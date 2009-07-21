namespace FluentMvc.Constraints
{
    public interface IConstraint
    {
        bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector;
    }
}