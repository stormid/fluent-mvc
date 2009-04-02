namespace FluentMvc.Windsor
{
    using Castle.Windsor;
    using Configuration;
    using MvcContrib.Castle;

    public class WindsorMvcConvention : FluentMvcConvention
    {
        public WindsorMvcConvention(IWindsorContainer container)
        {
            SetControllerFactory(new WindsorControllerFactory(container));
        }
    }
}