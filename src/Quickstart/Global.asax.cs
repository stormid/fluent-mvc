namespace Quickstart
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Controllers;
    using FluentMvc;
    using FluentMvc.ActionResultFactories;
    using FluentMvc.Configuration;

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
            var controllerFactory = FluentMvcConfiguration.Create()
                .UsingControllerFactory(new DefaultControllerFactory())  // Replace this with your ControllerFactory of factory
                .WithResultFactory<ActionResultFactory>()                // This supplies the backwards compatibility with standard Mvc controller actions
                .WithResultFactory<JsonResultFactory>()                  // This will automatically wrap a response in a JsonResult if the borwser is expecting a JSON response
                .WithResultFactory(new ViewResultFactory(), true)            // This will wrap the return of a controller in a ViewResult, this is the catchall factory of the pipeline
                .WithFilter<HandleErrorAttribute>()                      // This filter will apply to all controllers
                .WithFilter<AuthorizeAttribute>(Except
                                                   .For<AccountController>(ac => ac.LogOn())
                                                   .AndFor<AccountController>(ac => ac.LogOn(null, null, false, null))
                                                   .AndFor<HomeController>())
                .BuildControllerFactory();

            // New syntax, not fully tested yet...
            //FluentMvcConfiguration
            //    .Configure = x =>
            //                     {
            //                         x.UsingControllerFactory(new DefaultControllerFactory());

            //                         x.WithResultFactory<ActionResultFactory>()
            //                             .WithResultFactory<JsonResultFactory>()
            //                             .WithResultFactory<ViewResultFactory>(Is.Default);

            //                         x.WithFilter<HandleErrorAttribute>()
            //                             .WithFilter<AuthorizeAttribute>(
            //                             Except
            //                                 .For<AccountController>(ac => ac.LogOn())
            //                                 .AndFor<AccountController>(ac => ac.LogOn(null, null, false, null))
            //                                 .AndFor<HomeController>()
            //                             );
            //                     };
            //ControllerBuilder.Current.BuildFromFluentMcv();


            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}