namespace FluentMvc.Configuration
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ActionResultFactories;

    public class FluentMvcConvention
    {
        public virtual IControllerFactory ControllerFactory { get; set; }
        private Queue<IActionResultFactory> factories = new Queue<IActionResultFactory>();
        public virtual Queue<IActionResultFactory> Factories
        {
            get { return factories; }
            protected set { factories = value; }
        }

        public virtual IActionResultFactory DefaultFactory { get; set; }

        protected virtual void AddFactory(IActionResultFactory factory)
        {
            Factories.Enqueue(factory);
        }

        protected void SetDefaultFactory(IActionResultFactory factory)
        {
            DefaultFactory = factory;
        }

        protected void SetControllerFactory(IControllerFactory controllerFactory)
        {
            ControllerFactory = controllerFactory;
        }
    }
}