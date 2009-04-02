namespace FluentMvc.Spec
{
    using System.Web.Mvc;

    internal class TestController : Controller
    {
        public ActionResult ReturnViewResult() { return View(); }
        public Post ReturnPost() { return new Post(); }
        public Post ReturnNull() { return null; }
    }

    public class Post
    {
    }
}