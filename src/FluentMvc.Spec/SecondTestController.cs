namespace FluentMvc.Spec
{
    using System;
    using System.Web.Mvc;

    public class SecondTestController : Controller
    {
        public object DoSomething()
        {
            return new object();
        }

        public Post ReturnPost() { return new Post(); }
    }

    public class ThirdTestController : Controller
    {
        public Post ReturnPost() { return new Post(); }

        public object ReturnNull()
        {
            return null;
        }
    }
}