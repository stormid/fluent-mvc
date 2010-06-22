using System.Web.Mvc;
using FluentMvc.Constraints;

namespace FluentMvc.Configuration.Registrations
{
    public class ExceptInstanceRegistration : InstanceRegistration
    {
        public ExceptInstanceRegistration(IConstraint guardConstraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(new NotConstraint(guardConstraint), actionDescriptor, controllerDescriptor)
        {
        }
    }
}