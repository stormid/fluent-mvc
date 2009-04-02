namespace FluentMvc.Configuration
{
    using System.Web.Mvc;
    using ActionResultFactories;

    public class MvcConvention : FluentMvcConvention
    {
        public MvcConvention()
        {
            SetControllerFactory(new DefaultControllerFactory());
            AddFactory(new ActionResultFactory());
            SetDefaultFactory(new ViewResultFactory());
        }
    }
}