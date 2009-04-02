namespace FluentMvc.Constraints
{
    using System.Linq;

    public class AcceptsJson : IConstraint
    {
        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return selector.AcceptTypes.Contains("application/json");
        }
    }
}