namespace FluentMvc.Spec.Unit.ActionFilterRegistrySpecs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using NUnit.Framework;
    using Utils;

    internal class ActionFilterRegistryTester
    {
        private readonly IActionFilterRegistry actionFilterRegistry;

        public ActionFilterRegistryTester(IActionFilterRegistry actionFilterRegistry)
        {
            this.actionFilterRegistry = actionFilterRegistry;
        }

        public int RegistryCount
        {
            get { return actionFilterRegistry.Registrations.Count(); }
        }

        private IList<IActionFilter> ReturnedFilters()
        {
            var filterInfo = new FilterInfo();
            actionFilterRegistry.AddFiltersTo(filterInfo, new EmptyActionFilterSelector());
            return filterInfo.ActionFilters;
        }

        public void RegisterFilter(ActionFilterRegistryItem actionFilterRegistryItem)
        {
            actionFilterRegistry.Add(actionFilterRegistryItem);
        }

        public void AssertFilterIsNotReturned(IActionFilter actionFilter)
        {
            Assert.That(ReturnedFilters().Contains(actionFilter), Is.False, "Filter was not returned");
        }

        public int CountReturnedForControllerAndAction(ActionDescriptor actionDescriptor)
        {
            return actionFilterRegistry
                .FindForSelector(new ControllerActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor))
                .Count();
        }

        public int CountReturnedForControllerAndAction<TController>(Expression<Func<TController, object>> func) where TController : Controller
        {
            var actionDescriptor = func.CreateActionDescriptor();

            return actionFilterRegistry
                .FindForSelector(new ControllerActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor))
                .Count();
            
        }

    }
}