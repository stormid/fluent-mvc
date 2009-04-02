namespace FluentMvc.Spec.Unit.FluentMvcControllerFactory
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentMvcControllerFactory=FluentMvc.FluentMvcControllerFactory;

    public abstract class FluentMvcControllerFactoryBase : SpecificationBase
    {
        protected IControllerFactory ControllerFactory;
        protected FluentMvcControllerFactory fluentMvcControllerFactory;
        protected Controller Controller;
        protected RequestContext RequestContext;
    }
}