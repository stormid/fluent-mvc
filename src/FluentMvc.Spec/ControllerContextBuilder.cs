namespace FluentMvc.Spec
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using MvcContrib.TestHelper;
    using Rhino.Mocks;

    // TODO: Refactor to use base type and extend
    public class ControllerContextBuilder
    {
        private Controller controller;
        private readonly NameValueCollection formValues = new NameValueCollection();

        public ControllerContextBuilder()
        {
            controller = MockRepository.GenerateStub<Controller>();
        }

        public ControllerContext Build()
        {
            ControllerContext context = GetControllerContext();

            context.HttpContext.Request.Form.Add(formValues);

            return context;
        }

        protected ControllerContext GetControllerContext()
        {
            var testHelper = new TestControllerBuilder();
            testHelper.InitializeController(controller);

            return controller.ControllerContext;
        }

        public ControllerContextBuilder SetPostData(KeyValuePair<string, object> pair)
        {
            formValues.Add(pair.Key, pair.Value.ToString());
            return this;
        }

        public ControllerContextBuilder SetPostData(KeyValuePair<string, object>[] pairs)
        {
            foreach (var pair in pairs)
            {
                SetPostData(pair);
            }
            return this;
        }

        public ControllerContextBuilder WithController(Controller newController)
        {
            controller = newController;
            return this;
        }
    }
}