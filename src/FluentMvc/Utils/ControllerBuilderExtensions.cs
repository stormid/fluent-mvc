using System;

namespace FluentMvc
{
    using System.Web.Mvc;

    public static class ControllerBuilderExtensions
    {
        [Obsolete("Please use FluentMvcConfiguration.ConfigureFilterProvider() to instead")]
        public static void BuildFromFluentMcv(this ControllerBuilder controllerBuilder)
        {
        }
    }
}