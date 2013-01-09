using System;
using System.Web;
using System.Web.Mvc;
using FluentMvc;
using FluentMvc.ActionResultFactories;
using FluentMvc.Configuration;
using FluentMvc.Constraints;
using Quickstart.Controllers;

namespace Quickstart
{
    public class FilterConfig
    {
        public static void RegisterProvider(FilterProviderCollection providerCollection)
        {
            var filterProvider = FluentMvcConfiguration
                .ConfigureFilterProvider(x =>
                {
                    x.WithResultFactory<ActionResultFactory>()
                        .WithResultFactory<JsonResultFactory>()
                        .WithResultFactory<ViewResultFactory>(isDefault: true);

                    x.WithResultFactory<ErrorThrowingResultFactory>(
                        Apply.For<HomeController>(hc => hc.ErrorResultFactory()));

                    x.WithFilter<HandleErrorAttribute>();
                    x.WithFilter<AuthorizeAttribute>(
                        Except
                            .For<AccountController>(ac => ac.LogOn())
                            .AndFor<AccountController>(
                                ac => ac.LogOn(null, null, false, null))
                            .AndFor<HomeController>());

                    x.WithFilter<ErrorThrowingFilter>(
                        Apply.When<ExpectsHtml>().For<HomeController>(hc => hc.About()));
                });

            providerCollection.Insert(0, filterProvider);
        }
    }

    public class ErrorThrowingResultFactory : AbstractActionResultFactory
    {
        protected override ActionResult CreateCore(ActionResultSelector selector)
        {
            throw new Exception("ErrorThrowingResultFactory");
        }
    }

    public class ErrorThrowingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            throw new Exception("ErrorThrowingFilter");
        }
    }
}