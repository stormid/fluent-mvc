namespace FluentMvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public sealed class EmptyActionDescriptor : ActionDescriptor
    {
        public static readonly ActionDescriptor Instance = new EmptyActionDescriptor();

        private EmptyActionDescriptor()
        {
        }

        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            return new object();
        }

        public override ParameterDescriptor[] GetParameters()
        {
            return new ParameterDescriptor[] {};
        }

        public override string ActionName
        {
            get { return "Empty"; }
        }

        public override ControllerDescriptor ControllerDescriptor
        {
            get { return EmptyControllerDescriptor.Instance; }
        }
    }

    public sealed class EmptyControllerDescriptor : ControllerDescriptor
    {
        public static readonly ControllerDescriptor Instance = new EmptyControllerDescriptor();

        private EmptyControllerDescriptor()
        {
        }

        public override ActionDescriptor FindAction(ControllerContext controllerContext, string actionName)
        {
            throw new NotImplementedException();
        }

        public override ActionDescriptor[] GetCanonicalActions()
        {
            throw new NotImplementedException();
        }

        public override Type ControllerType
        {
            get { return null; }
        }
    }
}