namespace FluentMvc.Constraints
{
    using System.Linq;

    public class ExpectsJson : IConstraint
    {
        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return selector.AcceptTypes.Contains("application/json");
        }
    }

    public class ExpectsHtml : IConstraint
    {
        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return selector.AcceptTypes.Contains("text/html");
        }
    }
}