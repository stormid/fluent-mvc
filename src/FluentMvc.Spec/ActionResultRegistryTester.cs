namespace FluentMvc.Spec
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Constraints;
    using FluentMvc;
    using Utils;
    using System.Linq;

    public class ActionResultRegistryTester
    {
        private IActionResultRegistry inner;

        public void Init(IActionResultRegistry registry)
        {
            inner = registry;
        }

        public void ItemCountShouldBe(int count)
        {
            inner.Registrations.Length.ShouldEqual(count);
        }

        public void ShouldContain(ActionResultRegistryItem registryItem)
        {
            inner.Registrations.Contains(registryItem);
        }

        public ActionDescriptor AddItemWithActionDescriptor<T>(Expression<Func<T, object>> func)
            where T : Controller
        {
            var actionDescriptor = func.CreateActionDescriptor();
            var constraint = new PredefinedConstraint(true);
            inner.Add(new ActionResultRegistryItem(typeof(TestActionResultFactory), constraint, actionDescriptor, actionDescriptor.ControllerDescriptor));

            return actionDescriptor;
        }
    }
}