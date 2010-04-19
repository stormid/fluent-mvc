using System.Web.Mvc;

namespace FluentMvc.Constraints
{
    public class AreaConstraint : IConstraint
    {
        private readonly AreaRegistration areaRegistration;

        public AreaConstraint(AreaRegistration areaRegistration)
        {
            this.areaRegistration = areaRegistration;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            var areaName = selector.RouteData.GetRequiredString("area");

            return areaRegistration.AreaName == areaName;
        }
    }
}