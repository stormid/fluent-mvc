namespace Quickstart
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Controllers;
    using FluentMvc;
    using FluentMvc.ActionResultFactories;
    using FluentMvc.Configuration;
    using FluentMvc.Constraints;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }


        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

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

            FilterProviders.Providers.Add(filterProvider);
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