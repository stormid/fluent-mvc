namespace FluentMvc
{
    using System.Web.Mvc;

    public static class ControllerBuilderExtensions
    {
        public static void BuildFromFluentMcv(this ControllerBuilder controllerBuilder)
        {
            controllerBuilder.SetControllerFactory(FluentMvcConfiguration.ConfigureAndBuildControllerFactory());
        }
    }
}