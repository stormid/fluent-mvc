namespace FluentMvc.Spec.Unit.ActionResultFactories
{
    using System.Web.Mvc;
    using FluentMvc.ActionResultFactories;

    public class ActionResultFactoryTester<TFilter> where TFilter : IActionResultFactory, new()
    {
        public readonly TFilter Child;
        private readonly ActionResultSelector factoryOptions;

        public object ReturnValue { get; protected set; }

        public ActionResultFactoryTester(string[] acceptTypes, object returnValue)
        {
            factoryOptions = new ActionResultSelector {AcceptTypes = acceptTypes, ReturnValue =  returnValue, ControllerContext =  new ControllerContext()};
            ReturnValue = returnValue;
            Child = new TFilter();
        }

        public bool GetShouldbeReturned()
        {
            return Child.ShouldBeReturnedFor(factoryOptions);
        }

        public ActionResult CreateResult()
        {
            return Child.Create(factoryOptions);
        }
    }
}