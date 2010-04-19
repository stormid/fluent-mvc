using System.Web.Mvc;

namespace FluentMvc.Constraints
{
    public class AreaConstraint : IConstraint
    {
        private readonly string areaName;

        public AreaConstraint(AreaRegistration areaRegistration) : this(areaRegistration.AreaName) { }

        public AreaConstraint(string areaName)
        {
            this.areaName = areaName;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            object area;
            return selector.RouteData.DataTokens.TryGetValue("area", out area) && areaName == area.ToString();
        }
    }
}