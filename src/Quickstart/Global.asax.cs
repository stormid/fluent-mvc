using System.Web.Mvc;
using System.Web.Routing;

namespace Quickstart
{
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
                .WithControllerFactory(new DefaultControllerFactory())  // Replace this with your ControllerFactory of factory
                .AddResultFactory<ActionResultFactory>()                // This supplies the backwards compatibility with standard Mvc controller actions
                .AddResultFactory<JsonResultFactory>()                  // This will automatically wrap a response in a JsonResult if the borwser is expecting a JSON response
                .WithDefaultFactory(new ViewResultFactory())            // This will wrap the return of a controlelr in a ViewResult, this is the catchall factory of the pipeline
                .AddFilter<HandleErrorAttribute>()                      // This filter will apply to all controllers
                .AddFilter<AuthorizeAttribute>(Except
                                                   .For<AccountController>(ac => ac.LogOn()) // Applies AuthorizeAttribute to the ChangePassword action on the AccountController  
                                                   .AndFor<AccountController>(ac => ac.LogOn(null, null, false, null))
                                                   .AndFor<HomeController>())
                .BuildControllerFactory();

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}