using System.Web.Mvc;
using FluentMvc.Constraints;

namespace FluentMvc.Configuration.Registrations
{
    public class ExceptInstanceRegistration : InstanceRegistration
    {
        public ExceptInstanceRegistration(IConstraint guardConstraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, FilterScope filterScope)
            : base(new NotConstraint(guardConstraint), actionDescriptor, controllerDescriptor, filterScope)
        {
        }
    }
}