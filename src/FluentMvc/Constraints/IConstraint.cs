namespace FluentMvc.Constraints
{
    using System.Web.Mvc;

    public interface IConstraint
    {
        bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector;
    }
}